using System;

namespace GraphQl.Server.Annotations.TypeResolvers
{
    public interface IGlobalGraphTypeResolver
    {
        Type ResolveGraphType(Type type);
    }
}