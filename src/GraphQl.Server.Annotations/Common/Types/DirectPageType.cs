using System.Linq;
using GraphQl.Server.Annotations.Common.Helpers;
using GraphQL.Types;

namespace GraphQl.Server.Annotations.Common.Types
{
    internal class DirectPageType<T> : ObjectGraphType<DirectPage<T>>
    {
        private readonly IGraphQlTypeRegistry _typeRegistry = GlobalContext.TypeRegistry;


        public DirectPageType()
        {
            var type = typeof(T);
            var objectType = _typeRegistry.ResolveAdditional(type).FirstOrDefault(t => t.IsGraphQlMember()) ?? type;
            var graphQlType = GraphQlUtils.GetGraphQlTypeFor(type);

            Name = $"PageOf{objectType.GetNameOrDefault(objectType.Name)}";

            Field(m => m.PageIndex, type: typeof(IdGraphType)).Description("Page index");
            Field(m => m.PageSize).Description("Page size");
            Field(m => m.TotalCount).Description("Total items count");
            Field(m => m.TotalPages).Description("Total pages count");
            Field(m => m.IndexFrom).Description("Index from").DefaultValue();
            Field(m => m.Items, type: typeof(ListGraphType<>).MakeGenericType(graphQlType)).Description("Items");
            Field(m => m.HasPreviousPage).Description("Has previous page");
            Field(m => m.HasNextPage).Description("Has next page");
        }
    }
}