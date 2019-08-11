using System;
using GraphQl.Server.Annotations.Common;

namespace GraphQl.Server.Annotations.TypeResolvers.Core
{
    internal class RegistryGraphTypeResolver : GraphTypeResolverBase
    {
        private readonly IGraphTypeRegistry _graphTypeRegistry;


        public RegistryGraphTypeResolver(IGraphTypeRegistry graphTypeRegistry)
        {
            _graphTypeRegistry = graphTypeRegistry;
        }


        protected override bool IsSuitable(Type type)
        {
            return _graphTypeRegistry.IsRegistered(type);
        }

        protected override Type ResolveGraphType(Type type)
        {
            return _graphTypeRegistry.Resolve(type);
        }
    }
}