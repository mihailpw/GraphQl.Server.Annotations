using System;
using System.Collections.Generic;

namespace GraphQl.Server.Annotations.TypeResolvers
{
    public class CompositeGraphTypeResolver : IGraphTypeResolver
    {
        private readonly IEnumerable<IGraphTypeResolver> _typeResolvers;


        public CompositeGraphTypeResolver(IEnumerable<IGraphTypeResolver> typeResolvers)
        {
            _typeResolvers = typeResolvers;
        }


        public bool TryResolveType(Type type, out Type graphType)
        {
            foreach (var graphTypeResolver in _typeResolvers)
            {
                if (graphTypeResolver.TryResolveType(type, out graphType))
                {
                    return true;
                }
            }

            graphType = null;
            return false;
        }
    }
}