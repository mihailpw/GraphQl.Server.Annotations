using System;
using GraphQl.Server.Annotations.Common.Helpers;
using GraphQL.Types;

namespace GraphQl.Server.Annotations.TypeResolvers.Resolvers
{
    public class NonNullGraphTypeResolver : GraphTypeResolverBase
    {
        private readonly IGlobalGraphTypeResolver _globalGraphTypeResolver;


        public NonNullGraphTypeResolver(IGlobalGraphTypeResolver globalGraphTypeResolver)
        {
            _globalGraphTypeResolver = globalGraphTypeResolver;
        }


        protected override bool IsSuitable(Type type)
        {
            return TypeUtils.NonNull.IsInType(type);
        }

        protected override Type ResolveGraphType(Type type)
        {
            var elementType = TypeUtils.NonNull.UnwrapType(type);
            var elementGraphQlType = _globalGraphTypeResolver.ResolveGraphType(elementType);
            return typeof(NonNullGraphType<>).MakeGenericType(elementGraphQlType);
        }
    }
}