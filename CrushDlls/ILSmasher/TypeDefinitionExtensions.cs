using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Mono.Cecil;

namespace ILSmasher
{
    /// <summary>
    ///     Extensions for <see cref="TypeDefinition" />
    /// </summary>
    public static class TypeDefinitionExtensions
    {
        /// <summary>
        ///     Gets a field by its name.
        /// </summary>
        /// <param name="self">Reference to type definition that owns the method/member.</param>
        /// <param name="memberName">Name of the field.</param>
        /// <returns>Field definiton of the said field. Null, if none found.</returns>
        public static FieldDefinition GetField(this TypeDefinition self, string memberName)
        {
            return self.Fields.FirstOrDefault(f => f.Name == memberName);
        }

        /// <summary>
        ///     Gets the method by its name. If more overloads exist, only the first one defined is chosen.
        /// </summary>
        /// <param name="self">Reference to type definition that owns the method/member.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <returns>Method definition for the given method name. If no methods with such names are found, returns null.</returns>
        public static MethodDefinition GetMethod(this TypeDefinition self, string methodName)
        {
            return self.Methods.FirstOrDefault(m => m.Name == methodName);
        }

        /// <summary>
        ///     Gets the method by its name. If more overloads exist, only the one that has the same specified parameters is
        ///     chosen.
        ///     To easily obtain parameter types, refer to <see cref="ParamHelper" /> class.
        /// </summary>
        /// <param name="self">Reference to type definition that owns the method/member.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="paramTypes">Parameter types in the order they are declared in the method.</param>
        /// <returns>
        ///     Method definition for the given method name and overload. If no methods with such names and parameters are
        ///     found, returns null.
        /// </returns>
        public static MethodDefinition GetMethod(this TypeDefinition self,
                                                 string methodName,
                                                 params TypeReference[] paramTypes)
        {
            return GetMethod(self, methodName, paramTypes.Select(p => p.FullName).ToArray());
        }

        /// <summary>
        ///     Gets the method by its name. If more overloads exist, only the one that has the same specified parameters is
        ///     chosen.
        /// </summary>
        /// <param name="self">Reference to type definition that owns the method/member.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="types">Parameter types in the order they are declared in the method.</param>
        /// <returns>
        ///     Method definition for the given method name and overload. If no methods with such names and parameters are
        ///     found, returns null.
        /// </returns>
        public static MethodDefinition GetMethod(this TypeDefinition self, string methodName, params Type[] types)
        {
            return GetMethod(self, methodName, types.Select(t => ParamHelper.FromType(t).FullName).ToArray());
        }

        /// <summary>
        ///     Gets the method by its name. If more overloads exist, only the one that has the same specified parameters is
        ///     chosen.
        /// </summary>
        /// <param name="self">Reference to type definition that owns the method/member.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="types">
        ///     Full name of the parameter types in the order they are declared in the method. The full name is
        ///     specified by <see cref="Type" /> specification.
        /// </param>
        /// <returns>
        ///     Method definition for the given method name and overload. If no methods with such names and parameters are
        ///     found, returns null.
        /// </returns>
        public static MethodDefinition GetMethod(this TypeDefinition self, string methodName, params string[] types)
        {
            return
                    self.Methods.FirstOrDefault(
                        m =>
                            m.Name == methodName &&
                            types.SequenceEqual(m.Parameters.Select(p => p.ParameterType.FullName),
                                                StringComparer.InvariantCulture));
        }

        /// <summary>
        ///     Gets the all the method overloads with the given name.
        /// </summary>
        /// <param name="self">Reference to type definition that owns the method/member.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <returns>
        ///     An array of all the method overloads with the specified name.
        /// </returns>
        public static MethodDefinition[] GetMethods(this TypeDefinition self, string methodName)
        {
            return self.Methods.Where(m => m.Name == methodName).ToArray();
        }

        /// <summary>
        ///     Finds the methods with the given name that have at least the provided parameters. The number of parameters need not
        ///     match, which is why
        ///     the methods returned may have more parameters than passed to this method.
        /// </summary>
        /// <param name="self">Reference to type definition that owns the method/member.</param>
        /// <param name="methodName">Name of the method to match.</param>
        /// <param name="types">Parameter types in the order they should be declared in the method.</param>
        /// <returns>An array of methods that have the specified name and *at least* the given parameters.</returns>
        public static MethodDefinition[] MatchMethod(this TypeDefinition self, string methodName, params Type[] types)
        {
            return MatchMethod(self, methodName, types.Select(t => ParamHelper.FromType(t).FullName).ToArray());
        }

        /// <summary>
        ///     Finds the methods with the given name that have at least the provided parameters. The number of parameters need not
        ///     match, which is why
        ///     the methods returned may have more parameters than passed to this method.
        /// </summary>
        /// <param name="self">Reference to type definition that owns the method/member.</param>
        /// <param name="methodName">Name of the method to match.</param>
        /// <param name="paramTypes">Parameter types in the order they should be declared in the method.</param>
        /// <returns>An array of methods that have the specified name and *at least* the given parameters.</returns>
        public static MethodDefinition[] MatchMethod(this TypeDefinition self,
                                                     string methodName,
                                                     params TypeReference[] paramTypes)
        {
            return MatchMethod(self, methodName, paramTypes.Select(p => p.FullName).ToArray());
        }

        /// <summary>
        ///     Finds the methods with the given name that have at least the provided parameters. The number of parameters need not
        ///     match, which is why
        ///     the methods returned may have more parameters than passed to this method.
        /// </summary>
        /// <param name="self">Reference to type definition that owns the method/member.</param>
        /// <param name="methodName">Name of the method to match.</param>
        /// <param name="paramTypes">Parameter types in the order they should be declared in the method.</param>
        /// <returns>An array of methods that have the specified name and *at least* the given parameters.</returns>
        public static MethodDefinition[] MatchMethod(this TypeDefinition self,
                                                     string methodName,
                                                     params string[] paramTypes)
        {
            return
                    self.Methods.Where(
                            m =>
                                m.Name == methodName &&
                                paramTypes.Length <= m.Parameters.Count &&
                                paramTypes.SequenceEqual(
                                    m.Parameters.Take(paramTypes.Length).Select(p => p.ParameterType.FullName),
                                    StringComparer.InvariantCulture))
                        .ToArray();
        }
    }

    internal class TypeComparer : IEqualityComparer<TypeReference>
    {
        private static TypeComparer _instance;
        public static TypeComparer Instance => _instance ?? (_instance = new TypeComparer());

        public bool Equals(TypeReference x, TypeReference y)
        {
          
            if (
                !(x.Name == y.Name &&
                  x.Namespace == y.Namespace &&
                  x.IsArray == y.IsArray &&
                  x.IsGenericInstance == y.IsGenericInstance &&
                  x.IsByReference == y.IsByReference))
                return false;
            if (!x.IsGenericInstance)
                return true;
            GenericInstanceType gx = (GenericInstanceType)x;
            GenericInstanceType gy = (GenericInstanceType)y;
                      
            return gx.GenericArguments.Count == gy.GenericArguments.Count &&
                   !gx.GenericArguments.Where((t, i) => !Equals(t, gy.GenericArguments[i])).Any();
        }

        public int GetHashCode(TypeReference obj)
        {
            return obj.GetHashCode();
        }
    }
}