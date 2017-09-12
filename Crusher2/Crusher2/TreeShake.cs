using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;

namespace Crusher2
{
    public class TreeShake
    {
        private List<TypeDefinition> _types;
        private List<TypeDefinition> _interfaces;
        private List<TypeDefinition> _abstractType;
        private List<MethodDefinition> _methods;
        private List<EventDefinition> _events;
        private Dictionary<Key, object> _objectMap;

        public TreeShake(Dictionary<Key, object> allObjects)
        {
            _objectMap = allObjects;
            _events = allObjects.Where(kv => kv.Key.Token.TokenType == TokenType.Event)
                .Select(kv => kv.Value).Cast<EventDefinition>().ToList();
            _methods = allObjects.Where(kv => kv.Key.Token.TokenType == TokenType.Method)
                .Select(kv => kv.Value).Cast<MethodDefinition>().ToList();
            _types = allObjects.Where(kv => kv.Key.Token.TokenType == TokenType.TypeDef)
                .Select(kv => kv.Value).Cast<TypeDefinition>().ToList();
            _abstractType = _types.Where(t => t.IsAbstract && (!t.IsInterface)).ToList();
        }

        public (List<Key> deleted, List<Key> remapped) PruneInterfaces()
        {
            _interfaces = _types.Where(t => t.IsInterface).ToList();
            var list = new Dictionary<Key, (TypeDefinition def, int count)>();
            foreach(var i in _interfaces)
            {
                list.Add(i.GetKey(),(i, _types
                    .Count(t => t.Interfaces
                        .Any(i1 => i1.InterfaceType.Resolve().GetKey() == i.GetKey()))));
            }
            var deletedList = list.Where(l => l.Value.count == 0).Select(l => l.Key).ToList();
            var remappedList = list.Where(l => l.Value.count == 1).Select(l => l.Key).ToList();

            foreach(var target in deletedList)
            {
                _objectMap.Remove(target);
            }

            foreach(var target in remappedList)
            {
                var inter = (TypeDefinition) _objectMap[target];
                var mappedType = _types.First(t => t.Interfaces.Any(i => i.InterfaceType.Resolve().GetKey() == inter.GetKey()));
                _objectMap[target] = mappedType;
            }

            return (deletedList, remappedList);
        }

        public (List<Key> deleted, List<Key> remapped) PruneAbstractTypes()
        {
            var keys = new List<(Key key, int count)>();
            foreach(var ab in _abstractType)
            {
                keys.Add((ab.GetKey(), _types.Count(t => t.BaseType != null && 
                    t.BaseType.Resolve().GetKey() == ab.GetKey())));
            }
            var deleted = keys.Where(k => k.count == 0).Select(k => k.key).ToList();
            var remapped = keys.Where(k => k.count == 1).Select(k => k.key).ToList();

            foreach(var del in deleted)
            {
                _objectMap.Remove(del);
            }

            foreach(var map in remapped)
            {
                var remap = _types.Single(t => t.BaseType != null &&
                    t.BaseType.Resolve().GetKey() == map);
                _objectMap[map] = remap;
            }

            return (deleted, remapped);
        }

        
    }
}
