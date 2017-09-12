using System;
using System.Collections.Generic;
using System.Text;
using Mono.Cecil;
using Mono.Collections.Generic;

namespace Crusher2.Strike2
{
    public class ParameterMapper
    {
        private ModuleRebuilder _builder;

        public ParameterMapper(ModuleRebuilder builder) => _builder = builder;

        public ParameterReference Map(ParameterReference parameter, IGenericParameterProvider context)
        {
            var resolved = parameter.Resolve();
            //if(_builder.Module.TryImportParameter(parameter, context, out ParameterReference importedRef))
            //{
            //    return importedRef;
            //}
            return Map(resolved, context);
        }

        public ParameterDefinition Map(ParameterDefinition param, IGenericParameterProvider context)
        {
            var newDef = new ParameterDefinition(param.Name, param.Attributes, _builder.Map(param.ParameterType, context));
            
            if (newDef.HasConstant || newDef.HasConstant)
            {
                newDef.Constant = param.Constant;
            }
            if(newDef.HasMarshalInfo)
            {
                newDef.MarshalInfo = param.MarshalInfo;
            }

            _builder.Map(param.CustomAttributes, newDef.CustomAttributes, context);
            return newDef;
        }

        public void Map(Collection<ParameterDefinition> input, Collection<ParameterDefinition> output, IGenericParameterProvider context)
        {
            foreach (var p in input)
            {
                output.Add(Map(p, context));
            }
        }
    }
}
