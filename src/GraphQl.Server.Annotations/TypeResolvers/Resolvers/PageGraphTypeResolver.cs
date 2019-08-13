﻿using System;
using GraphQl.Server.Annotations.Common.Helpers;
using GraphQl.Server.Annotations.Common.Types;

namespace GraphQl.Server.Annotations.TypeResolvers.Resolvers
{
    public class PageGraphTypeResolver : GraphTypeResolverBase
    {
        protected override bool IsSuitable(Type type)
        {
            return TypeUtils.Page.IsInType(type);
        }

        protected override Type ResolveGraphType(Type type)
        {
            var elementType = TypeUtils.Page.UnwrapType(type);
            return typeof(PageGraphType<>).MakeGenericType(elementType);
        }
    }
}