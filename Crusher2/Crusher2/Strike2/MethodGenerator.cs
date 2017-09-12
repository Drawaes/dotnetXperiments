using System;
using System.Collections.Generic;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Crusher2.Strike2
{
    public class MethodGenerator
    {
        private MethodDefinition _oldDefinition;
        private MethodDefinition _newDefinition;
        private TypeGenerator _typeGen;

        public MethodGenerator(MethodDefinition oldDef, TypeGenerator parentType)
        {
            _oldDefinition = oldDef;
            _newDefinition = new MethodDefinition(oldDef.Name, oldDef.Attributes, parentType.Builder.DummyType)
            {
                HasThis = _oldDefinition.HasThis,
                CallingConvention = _oldDefinition.CallingConvention,
                ExplicitThis = _oldDefinition.ExplicitThis,
                ImplAttributes = _oldDefinition.ImplAttributes,
                IsAddOn = _oldDefinition.IsAddOn,
                IsRemoveOn = _oldDefinition.IsRemoveOn,
                IsGetter = _oldDefinition.IsGetter,
                IsSetter = _oldDefinition.IsSetter,
            };

            _typeGen = parentType;
            _typeGen.Definition.Methods.Add(_newDefinition);
            Builder.Map(_oldDefinition.GenericParameters, _newDefinition.GenericParameters, _newDefinition);
            Builder.Map(_oldDefinition.Parameters, _newDefinition.Parameters, _newDefinition);
        }

        public MethodDefinition Definition => _newDefinition;
        private ModuleRebuilder Builder => _typeGen.Builder;

        internal void Finish()
        {
            if (_oldDefinition.HasPInvokeInfo)
            {
                var pinvokeInfo = new PInvokeInfo(_oldDefinition.PInvokeInfo.Attributes, _oldDefinition.PInvokeInfo.EntryPoint, Builder.Module.GetPInvokeModule(_oldDefinition.PInvokeInfo.Module));
                _newDefinition.PInvokeInfo = pinvokeInfo;
            }

            foreach(var m in _oldDefinition.Overrides)
            {
                _newDefinition.Overrides.Add(Builder.Map(m, _newDefinition));
            }

            if (_oldDefinition.HasBody)
            {

                foreach (var v in _oldDefinition.Body.Variables)
                {
                    v.VariableType = Builder.Map(v.VariableType, _newDefinition);
                }

                Builder.Map(_oldDefinition.Body.Instructions, _newDefinition);

                foreach (var e in _oldDefinition.Body.ExceptionHandlers)
                {
                    if (e.HandlerType == ExceptionHandlerType.Catch)
                    {
                        e.CatchType = Builder.Map(e.CatchType, _newDefinition);
                    }
                }

                _newDefinition.Body = _oldDefinition.Body;
            }
        }

        internal void FinishArguments()
        {

            _newDefinition.ReturnType = Builder.Map(_oldDefinition.ReturnType, _newDefinition);
            _newDefinition.Attributes = _oldDefinition.Attributes;
            if (_oldDefinition.MethodReturnType.HasConstant)
            {
                _newDefinition.MethodReturnType.Constant = _oldDefinition.MethodReturnType.Constant;
            }
            if (_oldDefinition.MethodReturnType.HasMarshalInfo)
            {
                _newDefinition.MethodReturnType.MarshalInfo = _oldDefinition.MethodReturnType.MarshalInfo;
            }

            Builder.Map(_oldDefinition.CustomAttributes, _newDefinition.CustomAttributes, _newDefinition);
        }
    }
}
