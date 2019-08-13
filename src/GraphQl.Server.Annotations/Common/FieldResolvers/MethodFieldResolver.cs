using System;
using System.Reflection;
using System.Threading.Tasks;
using GraphQL.Resolvers;
using GraphQl.Server.Annotations.Common.FieldResolvers.Core;
using GraphQl.Server.Annotations.Common.Helpers;
using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;

namespace GraphQl.Server.Annotations.Common.FieldResolvers
{
    internal class MethodFieldResolver : IFieldResolver
    {
        private readonly Type _serviceType;
        private readonly MethodInfo _serviceMethod;
        private readonly IServiceProvider _serviceProvider;

        private readonly ArgumentsBuilder _argumentsBuilder;


        public MethodFieldResolver(Type serviceType, MethodInfo serviceMethod, IServiceProvider serviceProvider)
        {
            _serviceType = serviceType;
            _serviceMethod = serviceMethod;
            _serviceProvider = serviceProvider;

            _argumentsBuilder = new ArgumentsBuilder(serviceMethod);
        }


        public object Resolve(ResolveFieldContext context)
        {
            var arguments = _argumentsBuilder.Build(context);

            var target = _serviceType.IsInstanceOfType(context.Source)
                ? context.Source
                : _serviceProvider.GetRequiredService(_serviceType);

            if (target == null)
                throw new InvalidOperationException($"Could not resolve an instance of {_serviceType.Name} to execute {(context.ParentType != null ? $"{context.ParentType.Name}." : null)}{context.FieldName}");

            var result = _serviceMethod.Invoke(target, arguments);

            return ProcessingUtils.ProcessSyncOrAsync(result, SkipTypes);
        }


        private static object SkipTypes(object value)
        {
            switch (value)
            {
                case IId idValue:
                    return idValue.ValueObject;
                case INonNull nonNullValue:
                    return nonNullValue.ValueObject;
                default:
                    return value;
            }
        }
    }
}