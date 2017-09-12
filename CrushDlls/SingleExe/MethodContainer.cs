using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace SingleExe
{
    public class MethodContainer
    {
        private MethodDefinition _definition;
        private MethodDefinition _oldDefinition;
        private Key _key;

        public MethodContainer(MethodDefinition oldDef)
        {
            _key = oldDef.GetKey();
            _oldDefinition = oldDef;
        }

        public MethodDefinition Definition => _definition;
        public Key Key => _key;

        public void Init(TypeDefinition declaringType)
        {
            _definition = new MethodDefinition(
                _oldDefinition.Name,
                _oldDefinition.Attributes,
                TypeReferences.ResolveTypeReference(_oldDefinition.ReturnType, declaringType));
        }

        public void Finish()
        {
            foreach(var p in _oldDefinition.Parameters)
            {
                var newParam = new ParameterDefinition(p.Name, p.Attributes, ResolveTypeReference(p.ParameterType))
                {
                    IsReturnValue = p.IsReturnValue,
                    IsOut = p.IsOut,
                    IsOptional = p.IsOptional,
                    Constant = p.Constant
                };
                _definition.Parameters.Add(p);
            }
            //Todo generic parameters
            foreach (var p in _oldDefinition.GenericParameters)
            {
                var param = new GenericParameter(p.Name, _definition)
                {
                    Attributes = p.Attributes
                };
                _definition.GenericParameters.Add(param);
            }

            WriteBody();
        }

        private void WriteBody()
        {
            if (!_oldDefinition.HasBody)
            {
                return;
            }

            var body = _oldDefinition.Body;

            foreach(var e in body.ExceptionHandlers)
            {
                if(e.CatchType != null)
                {
                    e.CatchType = ResolveTypeReference(e.CatchType);
                }
            }

            foreach(var e in body.Variables)
            {
                e.VariableType = ResolveTypeReference(e.VariableType);
            }

            foreach(var i in body.Instructions)
            {
                ResolveInstruction(i);
            }
        }

        private TypeReference ResolveTypeReference(TypeReference oldRef)
        {
            switch(oldRef)
            {
                case GenericParameter genParm:
                    return GetGenericParameter(genParm);
            }
            return TypeReferences.ResolveTypeReference(oldRef, _definition);
        }

        private GenericParameter GetGenericParameter(GenericParameter genParm)
        {
            var r = _definition.GenericParameters.SingleOrDefault(g => g.Name == genParm.Name);
            if(r == null)
            {
                r = _definition.DeclaringType.GenericParameters.Single(g => g.Name == genParm.Name);
            }
            return r;
        }

        private void ResolveInstruction(Instruction instruction)
        {
            switch(instruction.Operand)
            {
                case TypeDefinition typeDef:
                    instruction.Operand = FutureModule.ResolveTypeDefinition(typeDef);
                    break;
                case FieldDefinition fieldDef:
                    instruction.Operand = FutureModule.ResolveFieldDefinition(fieldDef);
                    break;
                case TypeReference typeRef:
                    instruction.Operand = TypeReferences.ResolveTypeReference(typeRef, _definition);
                    break;
                //case MethodDefinition methodDef:
                //    instruction.Operand = FutureModule.ResolveMethodDefinition(methodDef);
                //    break;
                case null:
                case Instruction instr:
                    break;
                //default:
                //    throw new NotImplementedException();

            }
        }
    }
}