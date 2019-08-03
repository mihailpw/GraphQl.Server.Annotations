using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
            return methodInfo.GetParameters().Where(p => !TypeUtils.ResolveFieldContext.IsInType(p.ParameterType));
        }

        public static Type GetGraphQlTypeFor(Type type)
        {
            if (TypeUtils.Task.IsInType(type))
                return GetGraphQlTypeFor(TypeUtils.Task.UnwrapType(type));

            if (TypeUtils.Nullable.IsInType(type))
                return GetGraphQlTypeFor(TypeUtils.Nullable.UnwrapType(type));

            if (TypeUtils.Id.IsInType(type))
                return typeof(IdGraphType);

            if (TypeUtils.NonNull.IsInType(type))
            {
                var elementType = TypeUtils.NonNull.UnwrapType(type);
                var elementGraphQlType = GetGraphQlTypeFor(elementType);
                return typeof(NonNullGraphType<>).MakeGenericType(elementGraphQlType);
            }

            if (TypeUtils.Enumerable.IsInType(type))
            {
                var elementType = TypeUtils.Enumerable.UnwrapType(type);
                var elementGraphQlType = GetGraphQlTypeFor(elementType);
                return typeof(ListGraphType<>).MakeGenericType(elementGraphQlType);
            }

            return GlobalContext.TypeRegistry.Resolve(type);
        }

        public static bool IsEnabledForRegister(Type type)
        {
            var realType = TypeUtils.GetRealType(type);
            var isEnabledForRegister = GlobalContext.TypeRegistry.IsRegistered(realType);
            return isEnabledForRegister;
        }

        public static IEnumerable<object> BuildArguments(
            MethodInfo methodInfo,
            ResolveFieldContext context)
        {
            foreach (var parameterInfo in methodInfo.GetParameters())
            {
                var returnType = parameterInfo.ParameterType;
                if (TypeUtils.ResolveFieldContext.IsInType(returnType))
                {
                    if (returnType.IsGenericType)
                    {
                        var contextSourceType = returnType.GenericTypeArguments[0];
                        if (contextSourceType.IsInstanceOfType(context.Source))
                        {
                            yield return Activator.CreateInstance(typeof(ResolveFieldContext<>).MakeGenericType(contextSourceType), context);
                        }
                        else
                        {
                            throw new InvalidOperationException($"Provided context with {contextSourceType.Name} type can not be processed. Context type can be only with {context.Source.GetType().Name} type.");
                        }
                    }
                    else
                    {
                        yield return context;
                    }
                }
                else
                {
                    var value = context.GetArgument(returnType, parameterInfo.Name);
                    yield return value;
                }
            }

        }
    }
}