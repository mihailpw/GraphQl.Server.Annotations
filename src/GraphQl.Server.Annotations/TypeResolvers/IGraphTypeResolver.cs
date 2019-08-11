using System;

namespace GraphQl.Server.Annotations.TypeResolvers
{
    public interface IGraphTypeResolver
    {
        bool TryResolveType(Type type, out Type graphType);
    }
}