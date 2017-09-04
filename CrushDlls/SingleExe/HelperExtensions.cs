using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;

namespace SingleExe
{
    public static class HelperExtensions
    {
        public static IEnumerable<TypeDefinition> ModuleFilter(this IEnumerable<TypeDefinition> types)
        {
            return types.Where(t => t.Name != "<Module>");
        }

        public static string SafeName(this TypeDefinition t)
        {
            var name = t.Namespace == "" ? $"{t.Module.Name}.{t.FullName}" : t.FullName;
            return name;
        }

        public static string SafeName(this TypeReference t)
        {
            var name = t.Namespace == "" ? $"{t.Module.Name}.{t.FullName}" : t.FullName;
            return name;
        }
    }
}
