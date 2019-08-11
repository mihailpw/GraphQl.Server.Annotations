using System;
using GraphQl.Server.Annotations.Common.Helpers;
using GraphQL.Types;

namespace GraphQl.Server.Annotations.TypeResolvers.Resolvers
{
    public class ListGraphTypeResolver : GraphTypeResolverBase
    {
        private readonly IGlobalGraphTypeResolver _globalGraphTypeResolver;


        public ListGraphTypeResolver(IGlobalGraphTypeResolver globalGraphTypeResolver)
        {
            _globalGraphTypeResolver = globalGraphTypeResolver;
        }


        protected override bool IsSuitable(Type type)
        {
            return TypeUtils.Enumerable.IsInType(type);
        }

        protected override Type ResolveGraphType(Type type)
        {
            var elementType = TypeUtils.Enumerable.UnwrapType(type);
            var elementGraphQlType = _globalGraphTypeResolver.ResolveGraphType(elementType);
            return typeof(ListGraphType<>).MakeGenericType(elementGraphQlType);
        }
    }
}