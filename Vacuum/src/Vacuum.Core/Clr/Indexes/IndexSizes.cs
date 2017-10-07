using System;
using System.Collections.Generic;
using System.Text;
using Vacuum.Core.Clr.Flags;

namespace Vacuum.Core.Clr.Indexes
{
    public class IndexSizes
    {
        public static Dictionary<Type, bool> GetLargeSizes(Func<TableFlag, int> countFunction, HeapOffsetSizeFlags heaps)
        {
            var dictionary = new Dictionary<Type, bool>()
            {
                { typeof(TypeOrMethodDefIndex), UsesLargeIndexes(countFunction,1, TableFlag.Method, TableFlag.TypeDef) },
                { typeof(MethodDefOrRefIndex), UsesLargeIndexes(countFunction, 1, TableFlag.Method, TableFlag.MemberRef) },
                { typeof(ImplementationIndex), UsesLargeIndexes(countFunction, 2, TableFlag.File, TableFlag.AssemblyRef, TableFlag.ExportedType) },
                { typeof(MemberForwardedIndex), UsesLargeIndexes(countFunction, 1, TableFlag.Field, TableFlag.Method) },
                { typeof(EventIndex), UsesLargeIndexes(countFunction,0, TableFlag.Event) },
                { typeof(PropertyIndex), UsesLargeIndexes(countFunction,0, TableFlag.Property) },
                { typeof(HasSemanticsIndex), UsesLargeIndexes(countFunction, 1, TableFlag.Event, TableFlag.Property) },
                { typeof(HasDeclSecurityIndex), UsesLargeIndexes(countFunction, 2, TableFlag.TypeDef, TableFlag.Method, TableFlag.Assembly) },
                { typeof(StringIndex), (heaps & HeapOffsetSizeFlags.String) != 0 },
                { typeof(BlobIndex), (heaps & HeapOffsetSizeFlags.Blob) != 0 },
                { typeof(GuidIndex), (heaps & HeapOffsetSizeFlags.GUID) != 0 },
                { typeof(HasFieldMarshalIndex), UsesLargeIndexes(countFunction, 1, TableFlag.Field, TableFlag.Param) },
                { typeof(CustomAttributeTypeIndex), UsesLargeIndexes(countFunction, 3, TableFlag.Method, TableFlag.MemberRef) },
                { typeof(HasCustomAttributeIndex), UsesLargeIndexes(countFunction, 5, TableFlag.Method, TableFlag.TypeDef, TableFlag.TypeRef,
                  TableFlag.Field, TableFlag.Param, TableFlag.InterfaceImpl, TableFlag.MemberRef, TableFlag.Module,
                  TableFlag.DeclSecurity, TableFlag.Property, TableFlag.Event, TableFlag.StandAloneSig, TableFlag.ModuleRef,
                  TableFlag.TypeSpec, TableFlag.Assembly, TableFlag.AssemblyRef, TableFlag.File, TableFlag.ExportedType,
                  TableFlag.ManifestResource) },
                { typeof(HasConstantIndex), UsesLargeIndexes(countFunction, 2, TableFlag.Field, TableFlag.Param, TableFlag.Property) },
                { typeof(MemberRefParentIndex), UsesLargeIndexes(countFunction, 3, TableFlag.TypeDef, TableFlag.TypeRef, TableFlag.ModuleRef,
                  TableFlag.Module, TableFlag.TypeSpec) },
                { typeof(FieldIndex), UsesLargeIndexes(countFunction,0, TableFlag.Field) },
                { typeof(TypeDefIndex), UsesLargeIndexes(countFunction,0, TableFlag.TypeDef) },
                { typeof(TypeDefOrRefIndex), UsesLargeIndexes(countFunction, 2, TableFlag.TypeRef, TableFlag.TypeDef, TableFlag.TypeSpec) },
                { typeof(MethodIndex), UsesLargeIndexes(countFunction,0, TableFlag.Method) },
                { typeof(ParamIndex), UsesLargeIndexes(countFunction,0, TableFlag.Param) },
                { typeof(ResolutionScopeIndex),  UsesLargeIndexes(countFunction, 4, TableFlag.AssemblyRef, TableFlag.Module, TableFlag.ModuleRef, TableFlag.TypeRef)},
            };
            return dictionary;
        }

        private static bool UsesLargeIndexes(Func<TableFlag, int> sizeFunc, int bits, params TableFlag[] flags)
        {
            var max = 0;
            for (var i = 0; i < flags.Length; i++)
            {
                max = Math.Max(max, sizeFunc(flags[i]));
            }
            max <<= bits;
            return max >= ushort.MaxValue;
        }
    }
}
