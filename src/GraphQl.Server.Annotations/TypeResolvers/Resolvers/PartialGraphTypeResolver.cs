using System;
using GraphQl.Server.Annotations.Common.Helpers;
using GraphQl.Server.Annotations.Common.Types;

namespace GraphQl.Server.Annotations.TypeResolvers.Resolvers
{
    public class PartialGraphTypeResolver : GraphTypeResolverBase
    {
        protected override bool IsSuitable(Type type)
        {
            return TypeUtils.Partial.IsInType(type);
        }

        protected override Type ResolveGraphType(Type type)
        {
            var elementType = TypeUtils.Partial.UnwrapType(type);
            return typeof(PartialGraphType<>).MakeGenericType(elementType);
        }
    }
}