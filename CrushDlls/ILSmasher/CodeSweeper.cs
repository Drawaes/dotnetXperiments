using System;
using System.Collections.Generic;
using System.Text;
using Mono.Cecil;

namespace ILSmasher
{
    public static class CodeSweeper
    {
        private static Queue<object> _itemsToWalk = new Queue<object>();

        public static void AddEntryMethod()
        {
            _itemsToWalk.Enqueue(ModuleLoader.OldEntryModule.EntryPoint);
        }

        public static void WalkItems(int maxItems)
        {
            var counter = 0;
            while(_itemsToWalk.Count > 0 && counter < maxItems)
            {
                var item = _itemsToWalk.Dequeue();
                counter++;
                Console.WriteLine($"Processed {counter} items");
                Console.WriteLine($"Taken item still {_itemsToWalk.Count} to go");

                switch(item)
                {
                    case MethodDefinition methodDef:
                        WalkMethodDefinition(methodDef);
                        break;
                    case TypeReference typeRef when typeRef.IsArray:
                        _itemsToWalk.Enqueue(typeRef.GetElementType());
                        break;
                    case TypeReference typeRef:
                        TypeMapper.TouchType(typeRef);
                        break;
                    case ParameterDefinition paramDef:
                        WalkParameterDefinition(paramDef);
                        break;
                    case FieldDefinition fieldDef:
                        WalkFieldDefinition(fieldDef);
                        break;
                    case MethodReference methodRef:
                        WalkMethodReference(methodRef);
                        break;
                    case FieldReference fieldReference:
                        WalkFieldReference(fieldReference);
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        private static void WalkFieldReference(FieldReference field)
        {
            _itemsToWalk.Enqueue(field.DeclaringType);
            _itemsToWalk.Enqueue(field.FieldType);
        }

        private static void WalkMethodReference(MethodReference methodRef)
        {
            _itemsToWalk.Enqueue(MethodMapper.GetMethodDefinition(methodRef));
        }

        private static void WalkFieldDefinition(FieldDefinition fieldDef)
        {
            _itemsToWalk.Enqueue(fieldDef.DeclaringType);
            _itemsToWalk.Enqueue(fieldDef.FieldType);
        }

        private static void WalkParameterDefinition(ParameterDefinition paramDef)
        {
            _itemsToWalk.Enqueue(paramDef.ParameterType);
        }

        private static void WalkMethodDefinition(MethodDefinition methodDef)
        {
            MethodMapper.AddMethod(methodDef);
            _itemsToWalk.Enqueue(methodDef.DeclaringType);
            _itemsToWalk.Enqueue(methodDef.MethodReturnType.ReturnType);

            foreach (var gp in methodDef.GenericParameters)
            {
                _itemsToWalk.Enqueue(gp);
            }
            
            foreach(var p in methodDef.Parameters)
            {
                _itemsToWalk.Enqueue(p);
            }

            if(methodDef.HasBody)
            {
                foreach(var v in methodDef.Body.Variables)
                {
                    _itemsToWalk.Enqueue(v.VariableType);
                }
                foreach(var i in methodDef.Body.Instructions)
                {
                    if(i.Operand is IMetadataTokenProvider prov)
                    {
                        _itemsToWalk.Enqueue(prov);
                    }
                }
            }
        }
    }
}
