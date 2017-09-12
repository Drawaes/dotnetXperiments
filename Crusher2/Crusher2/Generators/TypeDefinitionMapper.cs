using System;
using System.Collections.Generic;
using System.Text;
using Mono.Cecil;

namespace Crusher2.Generators
{
    //public class TypeDefinitionMapper
    //{
    //    private Dictionary<string, TypeGenerator> _typesByString = new Dictionary<string, TypeGenerator>();
    //    private Dictionary<Key, TypeGenerator> _typesByKey = new Dictionary<Key, TypeGenerator>();
    //    private Queue<TypeGenerator> _dirtyTypes = new Queue<TypeGenerator>();
    //    private CoreMapper _mapper;
    //    private object typeDef;

    //    public TypeDefinitionMapper(CoreMapper mapper)
    //    {
    //        _mapper = mapper;
    //    }

    //    public void MarkTypeAsDirty(TypeGenerator typeGen) => _dirtyTypes.Enqueue(typeGen);
      
    //    public TypeGenerator Map(TypeDefinition mapType)
    //    {
    //        var key = mapType.GetKey();
    //        if(_typesByKey.TryGetValue(key, out TypeGenerator value))
    //        {
    //            return value.GetGeneratorForTypeDef(mapType);
    //        }
                       
    //        var newGen = new TypeGenerator(mapType, _mapper);
    //        _typesByKey.Add(key, newGen);
    //        newGen.WriteParameters(mapType);
    //        return newGen;
    //    }

    //    internal int ProcessItems(int count)
    //    {
    //        int i = 0;
    //        for (; i < count && ItemsRemainingToProcess > 0; i++)
    //        {
    //            var items = _dirtyTypes.Dequeue();
    //            items.Finish();
    //        }
    //        return i;
    //    }
    //}
}
