using System;
using System.Collections.Generic;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace SingleExe
{
    public class MethodState
    {
        private MethodDefinition _oldMethod;
        private TypeState _newType;
        private MethodDefinition _newMethod;
        private Dictionary<string, GenericParameter> _genericParameters = new Dictionary<string, GenericParameter>();

        public MethodState(MethodDefinition oldMethod, TypeState newType)
        {
            _oldMethod = oldMethod;
            _newType = newType;
            _newMethod = new MethodDefinition(oldMethod.Name, oldMethod.Attributes, _newType.GetPlaceHolderTypeRef());

        }

        public MethodDefinition Definition => _newMethod;

        public void ProcessGenericParameters()
        {
            _genericParameters = new Dictionary<string, GenericParameter>(_newType.GetGenericParameters());
            foreach (var genericParam in _oldMethod.GenericParameters)
            {
                var newGenParam = new GenericParameter(genericParam.Name, _newMethod);
                foreach (var c in genericParam.Constraints)
                {
                    newGenParam.Constraints.Add(GetTypeReference(c));
                }
                _genericParameters.Add(newGenParam.Name, newGenParam);
                _newMethod.GenericParameters.Add(newGenParam);
            }
        }

        public void ProcessParameters()
        {
            foreach (var param in _oldMethod.Parameters)
            {
                TypeReference tRef;
                if (param.ParameterType.IsByReference)
                {
                    tRef = new ByReferenceType(GetTypeReference(param.ParameterType.GetElementType()));
                }
                else
                {
                    tRef = GetTypeReference(param.ParameterType);
                }
                var newParam = new ParameterDefinition(param.Name, param.Attributes, tRef);
                _newMethod.Parameters.Add(newParam);
            }
        }

        public void ProcessBody()
        {
            if (_oldMethod.HasBody)
            {
                foreach (var v in _oldMethod.Body.Variables)
                {
                    _newMethod.Body.Variables.Add(new VariableDefinition(GetTypeReference(v.VariableType)));
                }
                var oldGenDictionary = _genericParameters;

                _genericParameters = new Dictionary<string, GenericParameter>(_genericParameters);

                int counter = 0;

                var newProcessor = _newMethod.Body.GetILProcessor();
                foreach (var i in _oldMethod.Body.Instructions)
                {
                    var operand = i.Operand;
                    if (i.OpCode.OperandType == OperandType.InlineType)
                    {
                        //var typeDef = 
                    }
                    else
                    {
                        switch (operand)
                        {
                            case MethodReference mRef:
                                mRef = new MethodReference(mRef.Name, GetTypeReference(mRef.DeclaringType));
                                operand = mRef;
                                break;
                            case FieldReference fRef:
                                operand = new FieldReference(fRef.Name, GetTypeReference(fRef.FieldType), GetTypeReference(fRef.DeclaringType));
                                break;
                            case TypeReference tRef:
                                operand = GetTypeReference(tRef);
                                break;
                        }
                        i.Operand = operand;
                        newProcessor.Append(i);
                    }
                }
                _newMethod.Body = newProcessor.Body;
            }
        }

        public void ProcessReturnType()
        {
            _newMethod.ReturnType = GetTypeReference(_oldMethod.ReturnType);
        }

        private TypeReference GetTypeReference(TypeReference oldTypeRef)
        {
            return _newType.GetTypeReference(oldTypeRef, _genericParameters);
        }
    }
}
