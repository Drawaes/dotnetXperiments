using System;
using System.Collections.Generic;
using System.Text;
using Mono.Cecil;

namespace Crusher2.Strike2
{
    public class MethodMapper
    {
        private ModuleRebuilder _builder;

        public MethodMapper(ModuleRebuilder builder) => _builder = builder;

        public MethodReference Map(MethodReference methodReference, IGenericParameterProvider context)
        {
            if(methodReference is GenericInstanceMethod genDef)
            {
                var newType = new GenericInstanceMethod(_builder.Map(genDef.GetElementMethod(),context));
                _builder.Map(genDef.GenericArguments, newType.GenericArguments, context);
                //_builder.Map(genDef.GenericParameters, newType.GenericParameters, context);
                return newType;
            }
            if (_builder.Module.TryImportMethod(methodReference, context, out MethodReference returnRef))
            {
                return returnRef;
            }
            var methodDef = methodReference.Resolve();
            var t = _builder.TypeDefinitions.Map(methodDef.DeclaringType);
            return t.GetMethod(methodDef);
        }
    }
}
