using System;
using System.Collections.Generic;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;

namespace Crusher2.Strike2
{
    public class InstructonMapper
    {
        private ModuleRebuilder _builder;

        public InstructonMapper(ModuleRebuilder builder) => _builder = builder;

        public void Map(Collection<Instruction> instructions, IGenericParameterProvider context)
        {
            foreach (var i in instructions)
            {
                Map(i, context);
            }
        }

        public Instruction Map(Instruction instruction, IGenericParameterProvider context)
        {
            switch (instruction.Operand)
            {
                case CallSite callsite:
                    callsite.ReturnType = _builder.Map(callsite.ReturnType, context);
                    var copy = new Collection<ParameterDefinition>(callsite.Parameters);
                    callsite.Parameters.Clear();
                    _builder.Map(copy, callsite.Parameters, context);
                    break;
                case TypeReference typeRef:
                    instruction.Operand = _builder.Map(typeRef, context);
                    break;
                case ParameterReference parmRef:
                    instruction.Operand = _builder.Map(parmRef, context);
                    break;
                case FieldDefinition fieldDef:
                    instruction.Operand = _builder.Map(fieldDef, context);
                    break;
                case Instruction instr:
                    break;
                case MethodReference methodRef:
                    instruction.Operand = _builder.Map(methodRef,context);
                    break;
                case FieldReference fieldRef:
                    instruction.Operand = _builder.Map(fieldRef, context);
                    break;
                case VariableDefinition varDef:
                    varDef.VariableType = _builder.Map(varDef.VariableType, context);
                    break;
            }
            return instruction;
        }
    }
}
