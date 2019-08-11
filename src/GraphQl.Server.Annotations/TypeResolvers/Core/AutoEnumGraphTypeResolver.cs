using System;
using GraphQl.Server.Annotations.Common;
using GraphQl.Server.Annotations.Common.Types;

namespace GraphQl.Server.Annotations.TypeResolvers.Core
{
    internal class AutoEnumGraphTypeResolver : GraphTypeResolverBase
    {
        private readonly IGraphTypeRegistry _graphTypeRegistry;


        public AutoEnumGraphTypeResolver(IGraphTypeRegistry graphTypeRegistry)
        {
            _graphTypeRegistry = graphTypeRegistry;
        }


        protected override bool IsSuitable(Type type)
        {
            return type.IsEnum;
        }

        protected override Type ResolveGraphType(Type type)
        {
            if (!_graphTypeRegistry.TryResolve(type, out var graphType))
            {
                graphType = typeof(AutoEnumerationGraphType<>).MakeGenericType(type);
                _graphTypeRegistry.DirectRegister(type, graphType);
            }

            return graphType;
        }
    }
}