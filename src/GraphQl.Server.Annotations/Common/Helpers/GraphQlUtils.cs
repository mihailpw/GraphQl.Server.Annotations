using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GraphQl.Server.Annotations.Common.Types;
using GraphQL.Types;

namespace GraphQl.Server.Annotations.Common.Helpers
{
    internal static class GraphQlUtils
    {
        public const BindingFlags DefaultBindingFlags = BindingFlags.Instance | BindingFlags.Public;


        public static IEnumerable<PropertyInfo> GetRegisteredProperties(Type type)
        {
            bool CheckIfPropertySupported(PropertyInfo propertyInfo)
            {
                var isSupported = propertyInfo.IsGraphQlMember();
                if (!isSupported)
                {
                    return false;
                }

                var propertyType = propertyInfo.PropertyType;
                if (!IsEnabledForRegister(propertyType))
                {
                    // ReSharper disable once PossibleNullReferenceException
                    throw new NotSupportedException($"The property type of {propertyInfo.DeclaringType.Name}.{propertyInfo.Name} is not registered (type name: {propertyType.Name}).");
                }

                return true;
            }

            return type.GetProperties(DefaultBindingFlags).Where(CheckIfPropertySupported);
        }

        public static IEnumerable<MethodInfo> GetRegisteredMethods(Type type)
        {
            bool CheckIfMethodSupported(MethodInfo methodInfo)
            {
                var isSupported = !methodInfo.IsSpecialName
                    && methodInfo.IsGraphQlMember()
                    && methodInfo.DeclaringType != typeof(object);
                if (!isSupported)
                {
                    return false;
                }

                var returnType = methodInfo.ReturnType;
                if (!IsEnabledForRegister(returnType))
                {
                    // ReSharper disable once PossibleNullReferenceException
                    throw new NotSupportedException($"The return type of {methodInfo.DeclaringType.Name}.{methodInfo.Name}() is not registered (type name: {returnType.Name}).");
                }

                return true;
            }

            return type.GetMethods(DefaultBindingFlags).Where(CheckIfMethodSupported);
        }

        public static IEnumerable<Type> GetRegisteredInterfaces(Type type)
        {
            return type.GetInterfaces().Where(t => t.IsGraphQlMember() && IsEnabledForRegister(t));
        }

        public static IEnumerable<ParameterInfo> GetAvailableParameters(MethodInfo methodInfo)
        {
            return methodInfo.GetParameters().Where(p => p.IsGraphQlMember());
        }

        public static bool IsEnabledForRegister(Type type)
        {
            var realType = TypeUtils.GetRealType(type);
            var isEnabledForRegister = GlobalContext.TypeRegistry.IsRegistered(realType);
            return isEnabledForRegister;
        }
    }
}