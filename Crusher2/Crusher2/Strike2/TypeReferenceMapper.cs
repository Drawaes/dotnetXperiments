using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Mono.Collections.Generic;

namespace Crusher2.Strike2
{
    public class TypeReferenceMapper
    {
        private ModuleRebuilder _builder;

        public TypeReferenceMapper(ModuleRebuilder builder) => _builder = builder;

        public void Map(Collection<TypeReference> input, Collection<TypeReference> output, IGenericParameterProvider context)
        {
            foreach (var t in input)
            {
                output.Add(Map(t, context));
            }
        }

        public TypeReference Map(TypeReference typeRef, IGenericParameterProvider context)
        {
            //If for some reason we try to map something that we already moved just return
            if (typeRef.Module == _builder.Module.Module)
            {
                return typeRef;
            }
            switch (typeRef)
            {
                case TypeSpecification spec:
                    return MapSpec(spec, context);
                case GenericParameter genParam:
                    return _builder.Map(genParam, context);
                case TypeDefinition typeDef:
                    //Check that it isn't part of the CLR if so we will import
                    if (_builder.Module.TryImportType(typeRef, context, out TypeReference importedRef))
                    {
                        return importedRef;
                    }
                    return _builder.TypeDefinitions.Map(typeDef).Definition;
                default:
                    //Check that it isn't part of the CLR if so we will import
                    if (_builder.Module.TryImportType(typeRef, context, out importedRef))
                    {
                        return importedRef;
                    }
                    var resolved = typeRef.Resolve();
                    return Map(resolved, context);
            }
            throw new InvalidOperationException("Should never get here");
        }

        private TypeSpecification MapSpec(TypeSpecification typeSpec, IGenericParameterProvider context)
        {
            switch (typeSpec)
            {
                case GenericInstanceType genType:
                    return MapGenInstance(genType, context);
                case PinnedType pinned:
                    return new PinnedType(Map(pinned.ElementType, context));
                case PointerType pointer:
                    return new PointerType(Map(pointer.ElementType, context));
                case ArrayType array:
                    return MapArray(array, context);
                case ByReferenceType byRef:
                    return new ByReferenceType(Map(byRef.ElementType, context));
                case RequiredModifierType reqType:
                    return new RequiredModifierType(Map(reqType.ModifierType, context), Map(reqType.ElementType, context));
            }
            throw new NotImplementedException();
        }

        private TypeSpecification MapArray(ArrayType array, IGenericParameterProvider context)
        {
            var a = new ArrayType(Map(array.ElementType, context));
            if (array.IsVector)
            {
                return a;
            }
            a.Dimensions.Clear();
            for (int i = 0; i < array.Dimensions.Count; i++)
            {
                a.Dimensions.Add(new ArrayDimension(array.Dimensions[i].LowerBound, array.Dimensions[i].UpperBound));
            }
            return a;
        }

        private GenericInstanceType MapGenInstance(GenericInstanceType genType, IGenericParameterProvider context)
        {
            var newType = new GenericInstanceType(_builder.Map(genType.ElementType, context));
            _builder.Map(genType.GenericArguments, newType.GenericArguments, context);
            return newType;
        }
    }
}
