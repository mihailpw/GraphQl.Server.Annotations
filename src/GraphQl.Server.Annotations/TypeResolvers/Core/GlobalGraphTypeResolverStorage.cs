using System;
using System.Collections.Generic;

namespace GraphQl.Server.Annotations.TypeResolvers.Core
{
    internal class GlobalGraphTypeResolverStorage : IGlobalGraphTypeResolver
    {
        public IList<IGraphTypeResolver> GraphTypeResolvers { get; }
        public ITypePreparer TypePreparer { get; set; }


        public GlobalGraphTypeResolverStorage()
        {
            GraphTypeResolvers = new List<IGraphTypeResolver>();
        }


        public Type ResolveGraphType(Type type)
        {
            var preparedType = TypePreparer.PrepareType(type);

            foreach (var graphTypeResolver in GraphTypeResolvers)
            {
                if (graphTypeResolver.TryResolveType(preparedType, out var graphType))
                {
                    return graphType;
                }
            }

            throw new ArgumentException($"Can not resolve graph type for {preparedType.Name}.", nameof(type));
        }
    }
}