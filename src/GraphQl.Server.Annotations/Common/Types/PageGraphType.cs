using System.Linq;
using GraphQl.Server.Annotations.Common.Helpers;
using GraphQl.Server.Annotations.TypeResolvers;
using GraphQL.Types;

namespace GraphQl.Server.Annotations.Common.Types
{
    internal class PageInfoGraphType : ObjectGraphType<PageInfo>
    {
        public PageInfoGraphType()
        {
            Name = "PageInfo";

            Field(m => m.PageIndex).Description("Page index");
            Field(m => m.PageSize).Description("Page size");
            Field(m => m.TotalCount).Description("Total items count");
            Field(m => m.TotalPages).Description("Total pages count");
            Field(m => m.IndexFrom).Description("Index from").DefaultValue();
            Field(m => m.HasPreviousPage).Description("Has previous page");
            Field(m => m.HasNextPage).Description("Has next page");
        }
    }

    internal class PageGraphType<T> : ObjectGraphType<Page<T>>
    {
        private readonly IGraphTypeRegistry _typeRegistry = GlobalContext.TypeRegistry;
        private readonly IGlobalGraphTypeResolver _typeResolver = GlobalContext.TypeResolver;


        public PageGraphType()
        {
            var type = typeof(T);
            var objectType = _typeRegistry.ResolveAdditional(type).FirstOrDefault(t => t.IsGraphQlMember()) ?? type;
            var graphQlType = _typeResolver.ResolveGraphType(type);

            Name = $"{objectType.GetNameOrDefault(objectType.Name)}Page";

            Field(m => m.PageInfo, type: typeof(PageInfoGraphType)).Description("Page index");
            Field(m => m.Items, type: typeof(ListGraphType<>).MakeGenericType(graphQlType)).Description("Items");
        }
    }
}