using System;
using System.Collections.Generic;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;

namespace Crusher2.Strike2
{
    public class ModuleRebuilder
    {
        private ModuleHandler _module;
        private TypeReferenceMapper _typeMapper;
        private TypeDefinitionHandler _typeDefHandler;
        private GenericParameterMapper _genMapper;
        private MethodMapper _methodMapper;
        private ParameterMapper _parameterMapper;
        private InstructonMapper _instructionMapper;
        private FieldReferenceMapper _fieldMapper;
        private CustomAttributeMapper _customAttributeMapper;
        private Compare.CompareMethod _methodComparer;
        
        public ModuleRebuilder(string name)
        {
            _methodComparer = new Compare.CompareMethod(this);
            _customAttributeMapper = new CustomAttributeMapper(this);
            _fieldMapper = new FieldReferenceMapper(this);
            _module = new ModuleHandler(name, ModuleKind.Console);
            _typeMapper = new TypeReferenceMapper(this);
            _instructionMapper = new InstructonMapper(this);
            _parameterMapper = new ParameterMapper(this);
            _typeDefHandler = new TypeDefinitionHandler(this);
            _genMapper = new GenericParameterMapper(this);
            _methodMapper = new MethodMapper(this);
            DummyType = _module.Module.TypeSystem.Char;
        }

        internal void MergeTypes()
        {
            _typeDefHandler.MergeTypes();
        }

        public ModuleHandler Module => _module;
        public TypeDefinitionHandler TypeDefinitions => _typeDefHandler;
        public FieldReferenceMapper FieldMapper => _fieldMapper;
        public TypeReference DummyType { get; }
        public Compare.CompareMethod MethodComparer => _methodComparer;

        public void AddTypeToModule(TypeDefinition typeDef)
        {
            _module.Module.Types.Add(typeDef);
        }

        public ParameterReference Map(ParameterReference param, IGenericParameterProvider context) => _parameterMapper.Map(param, context);
        public ParameterDefinition Map(ParameterDefinition param, IGenericParameterProvider context) => _parameterMapper.Map(param, context);
        public FieldReference Map(FieldReference field, IGenericParameterProvider context) => _fieldMapper.Map(field, context);
        public void Map(Collection<ParameterDefinition> input, Collection<ParameterDefinition> output, IGenericParameterProvider context) => _parameterMapper.Map(input, output, context);
        public TypeReference Map(TypeReference typeRef, IGenericParameterProvider context) => _typeMapper.Map(typeRef, context);
        public MethodReference Map(MethodReference methodRef, IGenericParameterProvider context) => _methodMapper.Map(methodRef, context);
        public GenericParameter Map(GenericParameter genParm, IGenericParameterProvider context) => _genMapper.Map(genParm, context);
        public void Map(Collection<TypeReference> input, Collection<TypeReference> output, IGenericParameterProvider context) => _typeMapper.Map(input, output, context);
        public void Map(Collection<Instruction> instructions, IGenericParameterProvider context) => _instructionMapper.Map(instructions, context);
        public void Map(Collection<GenericParameter> input, Collection<GenericParameter> output, IGenericParameterProvider context) => _genMapper.Map(input, output, context);
        public void Map(Collection<CustomAttribute> input, Collection<CustomAttribute> output, IGenericParameterProvider context) => _customAttributeMapper.Map(input, output, context);
    }
}
