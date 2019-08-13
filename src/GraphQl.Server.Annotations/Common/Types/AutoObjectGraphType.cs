﻿using System;
using System.Linq;
using GraphQL;
using GraphQl.Server.Annotations.Common.FieldResolvers;
using GraphQl.Server.Annotations.Common.Helpers;
using GraphQl.Server.Annotations.Providers;
using GraphQL.Types;

namespace GraphQl.Server.Annotations.Common.Types
{
    internal sealed class AutoObjectGraphType<T> : ObjectGraphType
    {
        public AutoObjectGraphType()
        {
            var serviceProvider = GlobalContext.ServiceProvider;
            var partsFactory = GlobalContext.PartsFactory;
            var config = GlobalContext.Config;

            var type = typeof(T);
            if (!type.IsGraphQlMember())
            {
                throw new InvalidOperationException($"Type {type.Name} should be marked with {nameof(GraphQLAttribute)}.");
            }

            type.FindInAttributes<IGraphTypeInfoProvider>()?.Provide(this, type, serviceProvider);

            foreach (var propertyInfo in GraphQlUtils.GetRegisteredProperties(type))
            {
                var fieldType = partsFactory.CreateFieldType(propertyInfo, new PropertyFieldResolver(propertyInfo, config));
                propertyInfo.FindInAttributes<IFieldTypeInfoProvider>()?.Provide(fieldType, propertyInfo, serviceProvider);
                AddField(fieldType);
            }

            foreach (var methodInfo in GraphQlUtils.GetRegisteredMethods(typeof(T)))
            {
                var fieldType = partsFactory.CreateFieldType(methodInfo, new MethodFieldResolver(type, methodInfo, serviceProvider));
                methodInfo.FindInAttributes<IFieldTypeInfoProvider>()?.Provide(fieldType, methodInfo, serviceProvider);
                AddField(fieldType);
            }

            if (!Fields.Any())
            {
                throw new InvalidOperationException($"Type {type.Name} has not supported fields.");
            }

            var requireConfigureIsTypeOf = false;
            foreach (var interfaceType in GraphQlUtils.GetRegisteredInterfaces(type))
            {
                requireConfigureIsTypeOf = true;
                Interface(partsFactory.CreateInterfaceType(interfaceType));
            }

            if (requireConfigureIsTypeOf)
            {
                IsTypeOf = partsFactory.CreateIsTypeOfFunc(type);
            }
        }
    }
}