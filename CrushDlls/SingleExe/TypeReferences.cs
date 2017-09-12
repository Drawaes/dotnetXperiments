using System;
using System.Collections.Generic;
using System.Text;
using Mono.Cecil;
using Mono.Collections.Generic;

namespace SingleExe
{
    public static class TypeReferences
    {
        public static TypeReference ResolveTypeReference(TypeReference returnType, IGenericParameterProvider genProvider)
        {
            switch (returnType)
            {
                case PointerType pointer:
                    return new PointerType(ResolveTypeReference(pointer.ElementType, genProvider));
                case ByReferenceType byRef:
                    return new ByReferenceType(ResolveTypeReference(byRef.ElementType, genProvider));
                case PinnedType pinned:
                    return new PinnedType(ResolveTypeReference(pinned.ElementType, genProvider));
                case SentinelType sentinel:
                case GenericParameter genParam:
                case FunctionPointerType func:
                    throw new NotImplementedException();
                case RequiredModifierType modType:
                    return ResolveRequiredModifierType(modType, genProvider);
                case ArrayType arrayType:
                    return ResolveArrayType(arrayType, genProvider);
                case TypeDefinition typeDef:
                    return FutureModule.ResolveTypeDefinition(typeDef);
                case GenericInstanceType genType:
                    return ResolveGenericInstanceType(genType, genProvider);
                case TypeSpecification typeSpec:
                    throw new NotImplementedException();
            }
            if (returnType.Namespace == "System")
            {
                return FutureModule.Import(returnType);
            }
            var resolved = returnType.Resolve();
            if (resolved != null)
            {
                
                return FutureModule.ResolveTypeDefinition(resolved);
            }
            throw new NotImplementedException();
        }

        private static ArrayType ResolveArrayType(ArrayType arrayType, IGenericParameterProvider genProvider)
        {
            //TODO Dimensions
            var newArray = new ArrayType(ResolveTypeReference(arrayType.ElementType, genProvider));
            return newArray;
        }

        private static RequiredModifierType ResolveRequiredModifierType(RequiredModifierType redMod, IGenericParameterProvider genProvider)
        {
            var modifierType = ResolveTypeReference(redMod.ModifierType, genProvider);
            var type = ResolveTypeReference(redMod.ElementType, genProvider);
            var newModifier = new RequiredModifierType(modifierType, type);
            return newModifier;
        }

        private static GenericInstanceType ResolveGenericInstanceType(GenericInstanceType genType, IGenericParameterProvider provider)
        {
            var newGenInstance = new GenericInstanceType(ResolveTypeReference(genType.ElementType, provider));
            ResolveTypeReferences(genType.GenericArguments, newGenInstance.GenericArguments, provider);
            return newGenInstance;
        }

        public static void ResolveTypeReferences(Collection<TypeReference> oldList, Collection<TypeReference> newList, IGenericParameterProvider provider)
        {
            foreach (var t in oldList)
            {
                newList.Add(ResolveTypeReference(t, provider));
            }
        }

    }
}
