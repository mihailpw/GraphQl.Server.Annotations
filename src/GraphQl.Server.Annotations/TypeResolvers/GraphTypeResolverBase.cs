using System;
using GraphQL;

namespace GraphQl.Server.Annotations.TypeResolvers
{
    public abstract class GraphTypeResolverBase : IGraphTypeResolver
    {
        public bool TryResolveType(Type type, out Type graphType)
        {
            if (IsSuitable(type))
            {
                var resolvedGraphType = ResolveGraphType(type);
                if (!resolvedGraphType.IsGraphType())
                {
                    throw new InvalidOperationException($"Type {resolvedGraphType.Name} is not graph type.");
                }

                graphType = resolvedGraphType;
                return true;
            }

            graphType = null;
            return false;
        }


        protected abstract bool IsSuitable(Type type);
        protected abstract Type ResolveGraphType(Type type);
    }
}