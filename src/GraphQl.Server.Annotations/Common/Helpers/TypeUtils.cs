using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL.Types;

namespace GraphQl.Server.Annotations.Common.Helpers
{
    internal static class TypeUtils
    {
        public static class Enumerable
        {
            public static bool IsInType(Type type)
            {
                return type != typeof(string) && typeof(IEnumerable).IsAssignableFrom(type);
            }

            public static Type UnwrapType(Type type)
            {
                var processingType = type;
                while (true)
                {
                    if (processingType == typeof(string))
                    {
                        return processingType;
                    }

                    var interfaces = processingType.GetInterfaces().Append(processingType).ToArray();
                    if (interfaces.Any(i => i.IsGenericTypeDefinition(typeof(IEnumerable<>))))
                    {
                        processingType = processingType.GenericTypeArguments[0];
                    }
                    else if (interfaces.Any(i => i == typeof(IEnumerable)))
                    {
                        return typeof(object);
                    }
                    else
                    {
                        return processingType;
                    }
                }
            }
        }

        public static class Task
        {
            public static bool IsInType(Type type)
            {
                return type == typeof(Task) || type.IsGenericTypeDefinition(typeof(Task<>));
            }

            public static Type UnwrapType(Type type)
            {
                var processingType = type;
                while (true)
                {
                    if (processingType.IsGenericTypeDefinition(typeof(Task<>)))
                    {
                        processingType = processingType.GenericTypeArguments[0];
                    }
                    else if (processingType == typeof(Task))
                    {
                        return typeof(void);
                    }
                    else
                    {
                        return processingType;
                    }
                }
            }
        }

        public static class Nullable
        {
            public static bool IsInType(Type type)
            {
                return type.IsGenericTypeDefinition(typeof(Nullable<>));
            }

            public static Type UnwrapType(Type type)
            {
                return type.IsGenericTypeDefinition(typeof(Nullable<>))
                    ? type.GenericTypeArguments[0]
                    : type;
            }
        }

        public static class Id
        {
            public static bool IsInType(Type type)
            {
                return type.IsGenericTypeDefinition(typeof(Id<>));
            }

            public static Type UnwrapType(Type type)
            {
                return type.IsGenericTypeDefinition(typeof(Id<>))
                    ? type.GenericTypeArguments[0]
                    : type;
            }
        }

        public static class NonNull
        {
            public static bool IsInType(Type type)
            {
                return type.IsGenericTypeDefinition(typeof(NonNull<>));
            }

            public static Type UnwrapType(Type type)
            {
                return type.IsGenericTypeDefinition(typeof(NonNull<>))
                    ? type.GenericTypeArguments[0]
                    : type;
            }
        }

        public static class DirectPage
        {
            public static bool IsInType(Type type)
            {
                return type.IsGenericTypeDefinition(typeof(DirectPage<>));
            }

            public static Type UnwrapType(Type type)
            {
                return type.IsGenericTypeDefinition(typeof(DirectPage<>))
                    ? type.GenericTypeArguments[0]
                    : type;
            }
        }

        public static class ResolveFieldContext
        {
            public static bool IsInType(Type type)
            {
                return type == typeof(GraphQL.Types.ResolveFieldContext) || type.IsGenericTypeDefinition(typeof(ResolveFieldContext<>));
            }

            public static Type UnwrapType(Type type)
            {
                return type.IsGenericTypeDefinition(typeof(ResolveFieldContext<>))
                    ? type.GenericTypeArguments[0]
                    : type;
            }
        }


        public static Type UnwrapCustomType(Type type)
        {
            var resultType = type;
            while (true)
            {
                if (Id.IsInType(resultType))
                    resultType = Id.UnwrapType(resultType);
                else if (NonNull.IsInType(resultType))
                    resultType = NonNull.UnwrapType(resultType);
                else
                    return resultType;
            }
        }

        public static Type GetRealType(Type type)
        {
            var resultType = type;
            while (true)
            {
                if (Nullable.IsInType(resultType))
                    resultType = Nullable.UnwrapType(resultType);
                else if (Enumerable.IsInType(resultType))
                    resultType = Enumerable.UnwrapType(resultType);
                else if (Task.IsInType(resultType))
                    resultType = Task.UnwrapType(resultType);
                else if (Id.IsInType(resultType))
                    resultType = Id.UnwrapType(resultType);
                else if (NonNull.IsInType(resultType))
                    resultType = NonNull.UnwrapType(resultType);
                else if (DirectPage.IsInType(resultType))
                    resultType = DirectPage.UnwrapType(resultType);
                else
                    return resultType;
            }
        }
    }
}