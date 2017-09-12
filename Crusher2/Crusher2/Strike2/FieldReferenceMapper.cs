using System;
using System.Collections.Generic;
using System.Text;
using Mono.Cecil;

namespace Crusher2.Strike2
{
    public class FieldReferenceMapper
    {
        private ModuleRebuilder _builder;

        public FieldReferenceMapper(ModuleRebuilder rebuilder) => _builder = rebuilder;

        public FieldReference Map(FieldReference fieldRef, IGenericParameterProvider provider)
        {
            var resolved = fieldRef.Resolve();
            if (resolved.Module.Name == _builder.Module.Module.Name)
            {
                return fieldRef;
            }

            if (_builder.Module.TryImportField(resolved, provider, out FieldReference returnRef))
            {
                return returnRef;
            }
            
            var generator = _builder.TypeDefinitions.Map(resolved.DeclaringType);
            return generator.GetField(resolved);
        }

        public FieldDefinition Copy(FieldDefinition fieldDef, IGenericParameterProvider context)
        {
            var newFieldDef = new FieldDefinition(fieldDef.Name, fieldDef.Attributes, _builder.Map(fieldDef.FieldType, context));

            if(fieldDef.HasLayoutInfo)
            {
                newFieldDef.Offset = fieldDef.Offset;
            }

            if (fieldDef.HasMarshalInfo)
            {
                newFieldDef.MarshalInfo = fieldDef.MarshalInfo;
            }

            if (fieldDef.HasConstant)
            {
                newFieldDef.Constant = fieldDef.Constant;
            }

            if (fieldDef.InitialValue?.Length > 0)
            {
                newFieldDef.InitialValue = fieldDef.InitialValue;
            }

            _builder.Map(fieldDef.CustomAttributes, newFieldDef.CustomAttributes, context);
            return newFieldDef;
        }
    }
}
