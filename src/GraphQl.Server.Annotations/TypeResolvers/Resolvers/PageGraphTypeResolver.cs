using System;
using GraphQl.Server.Annotations.Common.Helpers;
using GraphQl.Server.Annotations.Common.Types;

namespace GraphQl.Server.Annotations.TypeResolvers.Resolvers
{
    public class PageGraphTypeResolver : GraphTypeResolverBase
    {
        protected override bool IsSuitable(Type type)
        {
            return TypeUtils.DirectPage.IsInType(type);
        }

        protected override Type ResolveGraphType(Type type)
        {
            var elementType = TypeUtils.DirectPage.UnwrapType(type);
            return typeof(PageGraphType<>).MakeGenericType(elementType);
        }
    }
}