using System;
using GraphQl.Server.Annotations.Common.Helpers;
using GraphQL.Types;

namespace GraphQl.Server.Annotations.TypeResolvers.Resolvers
{
    public class IdGraphTypeResolver : GraphTypeResolverBase
    {
        protected override bool IsSuitable(Type type)
        {
            return TypeUtils.Id.IsInType(type);
        }

        protected override Type ResolveGraphType(Type type)
        {
            return typeof(IdGraphType);
        }
    }
}