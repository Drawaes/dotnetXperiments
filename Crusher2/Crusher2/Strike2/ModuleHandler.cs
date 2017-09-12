using System;
using System.Collections.Generic;
using System.Text;
using Mono.Cecil;

namespace Crusher2.Strike2
{
    public class ModuleHandler
    {
        private ModuleDefinition _module;
        private string _coreLib;
        private Dictionary<Key, TypeReference> _importedReferences = new Dictionary<Key, TypeReference>();
        private Dictionary<Key, FieldReference> _importedFieldReferences = new Dictionary<Key, FieldReference>();
        private Dictionary<Key, MethodReference> _importedMethodReferences = new Dictionary<Key, MethodReference>();
        private Dictionary<Key, ParameterReference> _importedParameterReferences = new Dictionary<Key, ParameterReference>();
        private Dictionary<string, ModuleReference> _importedPInvokeModule = new Dictionary<string, ModuleReference>();

        public ModuleHandler(string name, ModuleKind kind)
        {
            _module = ModuleDefinition.CreateModule(name, kind);
            _coreLib = "System.Private.CoreLib.dll";// _module.TypeSystem.CoreLibrary.Name;
        }

        public ModuleDefinition Module => _module;
        public string CoreLib => _coreLib;

        public void Write(string file) => _module.Write(file);

        public bool TryImportType(TypeReference typeReference, IGenericParameterProvider context, out TypeReference importedRef)
        {
            if (!(typeReference is TypeDefinition))
            {
                typeReference = typeReference.Resolve();
            }
            if (typeReference.Module.Name == _coreLib || (typeReference.Namespace == "System" &&
                (typeReference.Name == "Object" || typeReference.Name == "ValueType" || typeReference.Name == "Enum")))
            {
                if (_importedReferences.TryGetValue(typeReference.GetKey(), out importedRef))
                {
                    return true;
                }
                importedRef = _module.ImportReference(typeReference, context);
                _importedReferences.Add(typeReference.GetKey(), importedRef);
                return true;
            }
            importedRef = null;
            return false;
        }
        
        public ModuleReference GetPInvokeModule(ModuleReference moduleRef)
        {
            if(_importedPInvokeModule.TryGetValue(moduleRef.Name, out ModuleReference modRef))
            {
                return modRef;
            }
            _module.ModuleReferences.Add(moduleRef);
            _importedPInvokeModule.Add(moduleRef.Name, moduleRef);
            return moduleRef;
        }

        public bool TryImportMethod(MethodReference methodRef, IGenericParameterProvider context, out MethodReference importedRef)
        {
            if(!(methodRef is MethodDefinition methodDef))
            {
                methodDef = methodRef.Resolve();
            }
            if(methodDef.Module.Name == _coreLib)
            {
                if (_importedMethodReferences.TryGetValue(methodDef.GetKey(), out importedRef))
                {
                    return true;
                }
                importedRef = _module.ImportReference(methodDef, context);
                _importedMethodReferences.Add(methodDef.GetKey(), importedRef);
                return true;
            }
            importedRef = null;
            return false;
        }

        //public bool TryImportParameter(ParameterDefinition parameter, IGenericParameterProvider context, out ParameterReference importedRef)
        //{
        //    if (parameter.Method is MethodReference mDef)
        //    {
        //        var refDef = mDef.Resolve();
        //        if(refDef.Module.Name == _coreLib)
        //        {
        //            return _module.ImportReference(parameter)
        //        }
        //    }
        //    else
        //    {
        //        throw new InvalidOperationException();
        //    }

        //    {
        //        if (_importedFieldReferences.TryGetValue(parameter.GetKey(), out importedRef))
        //        {
        //            return true;
        //        }
        //        importedRef = _module.ImportReference(fieldReference, context);
        //        _importedFieldReferences.Add(fieldDef.GetKey(), importedRef);
        //        return true;
        //    }
        //    importedRef = null;
        //    return false;
        //}

        public bool TryImportField(FieldReference fieldReference, IGenericParameterProvider context, out FieldReference importedRef)
        {
            if (!(fieldReference is FieldDefinition fieldDef))
            {
                fieldDef = fieldReference.Resolve();
            }
            if (fieldDef.Module.Name == _coreLib)
            {
                if (_importedFieldReferences.TryGetValue(fieldDef.GetKey(), out importedRef))
                {
                    return true;
                }
                importedRef = _module.ImportReference(fieldReference, context);
                _importedFieldReferences.Add(fieldDef.GetKey(), importedRef);
                return true;
            }
            importedRef = null;
            return false;
        }
    }
}
