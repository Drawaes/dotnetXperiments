using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;

namespace ILSmasher
{
    public static class GenericMapper
    {
        public static TypeReference GetParameterType(this MethodReference method, int parameterIndex)
        {

            if (method.DeclaringType is GenericInstanceType genericInstance)
                return InflateGenericType(genericInstance, method.Parameters[parameterIndex].ParameterType);

            return method.Parameters[parameterIndex].ParameterType;
        }

        private static GenericInstanceType MakeGenericType(GenericInstanceType genericInstanceProvider, GenericInstanceType type)
        {
            var result = new GenericInstanceType(type.ElementType);

            for (var i = 0; i < type.GenericArguments.Count; ++i)
            {
                result.GenericArguments.Add(InflateGenericType(genericInstanceProvider, type.GenericArguments[i]));
            }

            return result;
        }

        public static TypeReference InflateGenericType(GenericInstanceType genericInstanceProvider, TypeReference typeToInflate)
        {
            if (typeToInflate is ArrayType arrayType)
            {
                var inflatedElementType = InflateGenericType(genericInstanceProvider, arrayType.ElementType);

                if (inflatedElementType != arrayType.ElementType)
                    return new ArrayType(inflatedElementType, arrayType.Rank);

                return arrayType;
            }

            if (typeToInflate is GenericInstanceType genericInst)
                return MakeGenericType(genericInstanceProvider, genericInst);

            if (typeToInflate is GenericParameter genericParameter)
            {
                if (genericParameter.Owner is MethodReference)
                    return genericParameter;

                var elementType = genericInstanceProvider.ElementType.Resolve();
                var parameter = elementType.GenericParameters.Single(p => p == genericParameter);
                return genericInstanceProvider.GenericArguments[parameter.Position];
            }

            if (typeToInflate is FunctionPointerType functionPointerType)
            {
                var result = new FunctionPointerType
                {
                    ReturnType = InflateGenericType(genericInstanceProvider, functionPointerType.ReturnType)
                };

                for (int i = 0; i < functionPointerType.Parameters.Count; i++)
                {
                    var inflatedParameterType = InflateGenericType(genericInstanceProvider, functionPointerType.Parameters[i].ParameterType);
                    result.Parameters.Add(new ParameterDefinition(inflatedParameterType));
                }

                return result;
            }

            if (typeToInflate is IModifierType modifierType)
            {
                var modifier = InflateGenericType(genericInstanceProvider, modifierType.ModifierType);
                var elementType = InflateGenericType(genericInstanceProvider, modifierType.ElementType);

                if (modifierType is OptionalModifierType)
                {
                    return new OptionalModifierType(modifier, elementType);
                }

                return new RequiredModifierType(modifier, elementType);
            }

            if (typeToInflate is PinnedType pinnedType)
            {
                var elementType = InflateGenericType(genericInstanceProvider, pinnedType.ElementType);

                if (elementType != pinnedType.ElementType)
                    return new PinnedType(elementType);

                return pinnedType;
            }

            if (typeToInflate is PointerType pointerType)
            {
                var elementType = InflateGenericType(genericInstanceProvider, pointerType.ElementType);

                if (elementType != pointerType.ElementType)
                    return new PointerType(elementType);

                return pointerType;
            }

            if (typeToInflate is ByReferenceType byReferenceType)
            {
                var elementType = InflateGenericType(genericInstanceProvider, byReferenceType.ElementType);

                if (elementType != byReferenceType.ElementType)
                    return new ByReferenceType(elementType);

                return byReferenceType;
            }

            if (typeToInflate is SentinelType sentinelType)
            {
                var elementType = InflateGenericType(genericInstanceProvider, sentinelType.ElementType);

                if (elementType != sentinelType.ElementType)
                    return new SentinelType(elementType);

                return sentinelType;
            }

            return typeToInflate;
        }

    }
}
