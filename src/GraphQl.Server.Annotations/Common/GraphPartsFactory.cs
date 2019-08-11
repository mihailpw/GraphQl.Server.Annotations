using System;
using System.Linq;
using System.Reflection;
using GraphQL.Resolvers;
using GraphQl.Server.Annotations.Common.Helpers;
using GraphQl.Server.Annotations.Providers;
using GraphQl.Server.Annotations.TypeResolvers;
using GraphQL.Types;

namespace GraphQl.Server.Annotations.Common
{
    internal class GraphPartsFactory : IGraphPartsFactory
    {
        private readonly IGraphTypeRegistry _typeRegistry;
        private readonly IGlobalGraphTypeResolver _globalGraphTypeResolver;


        public GraphPartsFactory(IGraphTypeRegistry typeRegistry, IGlobalGraphTypeResolver globalGraphTypeResolver)
        {
            _typeRegistry = typeRegistry;
            _globalGraphTypeResolver = globalGraphTypeResolver;
        }


        public FieldType CreateFieldType(PropertyInfo propertyInfo, IFieldResolver fieldResolver = null)
        {
            return new FieldType
            {
                Name = propertyInfo.Name,
                Type = _globalGraphTypeResolver.ResolveGraphType(propertyInfo.PropertyType),
                Arguments = null,
                Resolver = fieldResolver,
            };
        }

        public FieldType CreateFieldType(MethodInfo methodInfo, IFieldResolver fieldResolver = null)
        {
            var queryArguments = GraphQlUtils.GetAvailableParameters(methodInfo).Select(CreateQueryArgument);

            return new FieldType
            {
                Name = methodInfo.Name,
                Type = _globalGraphTypeResolver.ResolveGraphType(methodInfo.ReturnType),
                Arguments = new QueryArguments(queryArguments),
                Resolver = fieldResolver,
            };
        }

        public Type CreateInterfaceType(Type interfaceType)
        {
            return _globalGraphTypeResolver.ResolveGraphType(interfaceType);
        }

        public Func<object, bool> CreateIsTypeOfFunc(Type type)
        {
            var additionalTypes = _typeRegistry.ResolveAdditional(type).ToList();

            bool IsTypeOfFunc(object target)
            {
                var targetType = target.GetType();
                if (type == targetType)
                {
                    return true;
                }

                foreach (var additionalType in additionalTypes)
                {
                    if (additionalType == targetType)
                    {
                        return true;
                    }
                }

                return false;
            }

            return IsTypeOfFunc;
        }


        private QueryArgument CreateQueryArgument(ParameterInfo parameterInfo)
        {
            var queryArgument = new QueryArgument(_globalGraphTypeResolver.ResolveGraphType(parameterInfo.ParameterType))
            {
                Name = parameterInfo.Name,
                DefaultValue = parameterInfo.HasDefaultValue ? parameterInfo.DefaultValue : null
            };

            parameterInfo.FindInAttributes<IQueryArgumentInfoProvider>()?.Provide(queryArgument, parameterInfo, GlobalContext.ServiceProvider);

            return queryArgument;
        }
    }
}