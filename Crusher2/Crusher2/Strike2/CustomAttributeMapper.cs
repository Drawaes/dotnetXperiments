using System;
using System.Collections.Generic;
using System.Text;
using Mono.Cecil;
using Mono.Collections.Generic;

namespace Crusher2.Strike2
{
    public class CustomAttributeMapper
    {
        private ModuleRebuilder _builder;

        public CustomAttributeMapper(ModuleRebuilder builder) => _builder = builder;

        public CustomAttribute Map(CustomAttribute customAttribute, IGenericParameterProvider context)
        {
            var attributeType = _builder.Map(customAttribute.Constructor, context);
            var newAttribute = new CustomAttribute(attributeType, customAttribute.GetBlob());

            foreach (var c in customAttribute.ConstructorArguments)
            {
                newAttribute.ConstructorArguments.Add(Map(c, context));
            }
            foreach (var c in customAttribute.Fields)
            {
                newAttribute.Fields.Add(Map(c, context));
            }
            foreach (var p in customAttribute.Properties)
            {
                newAttribute.Properties.Add(Map(p, context));
            }
            return newAttribute;
        }

        public void Map(Collection<CustomAttribute> input, Collection<CustomAttribute> output, IGenericParameterProvider context)
        {
            foreach(var c in input)
            {
                output.Add(Map(c, context));
            }
        }

        private CustomAttributeArgument Map(CustomAttributeArgument arg, IGenericParameterProvider context)
        {
            var type = _builder.Map(arg.Type, context);
            return new CustomAttributeArgument(type, arg.Value);
        }

        private CustomAttributeNamedArgument Map(CustomAttributeNamedArgument namedArg, IGenericParameterProvider context)
        {
            var newArg = new CustomAttributeNamedArgument(namedArg.Name, Map(namedArg.Argument, context));
            return newArg;
        }
    }
}
