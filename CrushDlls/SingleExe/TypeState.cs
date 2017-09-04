using System;
using System.Collections.Generic;
using System.Text;
using Mono.Cecil;

namespace SingleExe
{
    public class TypeState
    {
        private Dictionary<string, GenericParameter> _genericParameters = new Dictionary<string, GenericParameter>();
        private Dictionary<string, FieldReference> _fieldReference = new Dictionary<string, FieldReference>();
        private TypeDefinition _oldType;
        private TypeDefinition _newType;
        private MergeState _mergeState;

        public TypeState(TypeDefinition oldType, TypeDefinition newType, MergeState mergeState)
        {
            _mergeState = mergeState;
            _oldType = oldType;
            _newType = newType;
            ProcessGenericParameters();
            _newType.BaseType = GetTypeReference(oldType.BaseType);
        }

        private void ProcessGenericParameters()
        {
            foreach (var gParam in _oldType.GenericParameters)
            {
                var newParam = new GenericParameter(gParam.Name, _newType);
                foreach (var c in gParam.Constraints)
                {
                    newParam.Constraints.Add(GetTypeReference(c));
                }
                _genericParameters.Add(gParam.Name, newParam);
                _newType.GenericParameters.Add(newParam);
            }
        }

        internal IDictionary<string, GenericParameter> GetGenericParameters() => _genericParameters;

        public void ProcessFields()
        {
            foreach (var f in _oldType.Fields)
            {
                TypeReference typeRef;
                if (f.FieldType == _oldType)
                {
                    typeRef = _newType;
                }
                else
                {
                    typeRef = GetTypeReference(f.FieldType);
                }
                var fieldDef = new FieldDefinition(f.Name, f.Attributes, typeRef);
                _newType.Fields.Add(fieldDef);
                _fieldReference.Add(f.Name, fieldDef);
            }

        }

        public void ProcessMethods()
        {
            foreach(var m in _oldType.Methods)
            {
                var mState = new MethodState(m, this);
                mState.ProcessGenericParameters();
                mState.ProcessReturnType();
                mState.ProcessParameters();
                mState.ProcessBody();
                _newType.Methods.Add(mState.Definition);
            }
        }
    
        public TypeReference GetPlaceHolderTypeRef()
        {
            return _mergeState.GetPlaceHolderTypeRef();
        }

        private TypeReference GetTypeReference(TypeReference oldTypeRef)
        {
            return GetTypeReference(oldTypeRef, _genericParameters);
        }

        public TypeReference GetTypeReference(TypeReference oldTypeRef, Dictionary<string, GenericParameter> genericParams)
        {
            if (oldTypeRef.IsArray)
            {
                var newArray = new ArrayType(GetTypeReference(oldTypeRef.GetElementType(), genericParams));
                return newArray;
            }

            if (oldTypeRef is GenericInstanceType genType)
            {
                var genInstance = new GenericInstanceType(GetTypeReference(oldTypeRef.GetElementType(), genericParams));
                foreach (var gp in genType.GenericParameters)
                {
                    var newGp = new GenericParameter(gp.Name, genInstance);
                    foreach (var c in gp.Constraints)
                    {
                        newGp.Constraints.Add(GetTypeReference(c, genericParams));
                    }
                    genericParams.Add(gp.Name, newGp);
                }
                foreach(var ga in genType.GenericArguments)
                {
                    genInstance.GenericArguments.Add(GetTypeReference(ga, genericParams));
                }
                return genInstance;
            }

            if (oldTypeRef.IsGenericParameter)
            {
                return genericParams[oldTypeRef.Name];
            }
            
            return _mergeState.GetTypeRef(oldTypeRef);
        }

    }
}
