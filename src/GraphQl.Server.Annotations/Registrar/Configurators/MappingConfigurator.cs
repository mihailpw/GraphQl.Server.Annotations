using System;
using GraphQl.Server.Annotations.Common;

namespace GraphQl.Server.Annotations.Registrar.Configurators
{
    internal class MappingConfigurator : IMappingConfigurator
    {
        private readonly Type _graphQlType;
        private readonly IGraphTypeRegistry _typeRegistry;


        public MappingConfigurator(
            Type graphQlType,
            IGraphTypeRegistry typeRegistry)
        {
            _graphQlType = graphQlType;
            _typeRegistry = typeRegistry;
        }


        public IMappingConfigurator Map<T>()
        {
            _typeRegistry.DirectRegister(typeof(T), _graphQlType);
            return this;
        }
    }
}