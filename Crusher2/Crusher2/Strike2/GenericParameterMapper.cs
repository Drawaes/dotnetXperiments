using System;
using System.Collections.Generic;
using System.Text;
using Mono.Cecil;
using Mono.Collections.Generic;

namespace Crusher2.Strike2
{
    public class GenericParameterMapper
    {
        private ModuleRebuilder _builder;

        public GenericParameterMapper(ModuleRebuilder builder) => _builder = builder;

        public GenericParameter Map(GenericParameter p, IGenericParameterProvider context)
        {
            var parm = new GenericParameter(p.Name, context)
            {
                HasNotNullableValueTypeConstraint = p.HasNotNullableValueTypeConstraint,
                HasReferenceTypeConstraint = p.HasReferenceTypeConstraint,
                IsContravariant = p.IsContravariant,
                IsCovariant = p.IsCovariant,
                position = p.position,
                etype = p.etype,
            };
            Map(p.GenericParameters, parm.GenericParameters, context);
            _builder.Map(p.Constraints, parm.Constraints, context);
            return parm;
        }

        public void Map(Collection<GenericParameter> input, Collection<GenericParameter> output, IGenericParameterProvider context)
        {
            foreach (var p in input)
            {
                var parm = new GenericParameter(p.Name, context)
                {
                    HasNotNullableValueTypeConstraint = p.HasNotNullableValueTypeConstraint,
                    HasReferenceTypeConstraint = p.HasReferenceTypeConstraint,
                    IsContravariant = p.IsContravariant,
                    IsCovariant = p.IsCovariant
                };
            }

            for (var i = 0; i < output.Count; i++)
            {
                Map(input[i].GenericParameters, output[i].GenericParameters, context);
                _builder.Map(input[i].Constraints, output[i].Constraints, context);
            }

        }
    }
}
