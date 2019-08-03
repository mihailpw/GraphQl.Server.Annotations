using System;
using System.Linq;
using GraphQL;
using GraphQl.Server.Annotations.Common.Helpers;
using GraphQl.Server.Annotations.Providers;
using GraphQL.Types;

namespace GraphQl.Server.Annotations.Common.Types
{
    internal sealed class AutoInputObjectGraphType<T> : InputObjectGraphType
    {
        public AutoInputObjectGraphType()
        {
            var serviceProvider = GlobalContext.ServiceProvider;
            var partsFactory = GlobalContext.PartsFactory;

            var type = typeof(T);
            if (!type.IsGraphQlMember())
            {
                throw new InvalidOperationException($"Type {type.Name} should be marked with {nameof(GraphQLAttribute)}.");
            }

            type.FindInAttributes<IGraphTypeInfoProvider>()?.Provide(this, type, serviceProvider);

            foreach (var propertyInfo in GraphQlUtils.GetRegisteredProperties(typeof(T)))
            {
                var fieldType = partsFactory.CreateFieldType(propertyInfo);
                propertyInfo.FindInAttributes<IFieldTypeInfoProvider>()?.Provide(fieldType, propertyInfo, serviceProvider);
                AddField(fieldType);
            }

            if (!Fields.Any())
            {
                throw new InvalidOperationException($"Type {type.Name} has not supported fields.");
            }
        }
    }
}