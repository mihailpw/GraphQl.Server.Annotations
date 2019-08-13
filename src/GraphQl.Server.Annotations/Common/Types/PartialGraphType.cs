using System.Linq;
using GraphQl.Server.Annotations.Common.Helpers;
using GraphQl.Server.Annotations.TypeResolvers;
using GraphQL.Types;

namespace GraphQl.Server.Annotations.Common.Types
{
    internal class PartialInfoGraphType : ObjectGraphType<PartialInfo>
    {
        public PartialInfoGraphType()
        {
            Name = "PartialInfo";

            Field(m => m.StartIndex).Description("Start index");
            Field(m => m.PartialSize).Description("Partial size");
            Field(m => m.TotalCount).Description("Total items count");
            Field(m => m.HasMore).Description("Has more items");
        }
    }

    internal class PartialGraphType<T> : ObjectGraphType
    {
        private readonly IGraphTypeRegistry _typeRegistry = GlobalContext.TypeRegistry;
        private readonly IGlobalGraphTypeResolver _typeResolver = GlobalContext.TypeResolver;


        public PartialGraphType()
        {
            var type = typeof(T);
            var objectType = _typeRegistry.ResolveAdditional(type).FirstOrDefault(t => t.IsGraphQlMember()) ?? type;
            var graphQlType = _typeResolver.ResolveGraphType(type);

            Name = $"{objectType.GetNameOrDefault(defaultName: objectType.Name)}Partial";

            Field(
                type: typeof(NonNullGraphType<PartialInfoGraphType>),
                name: nameof(Partial<object>.PartialInfo),
                description: "Partial info");
            Field(
                type: typeof(ListGraphType<>).MakeGenericType(graphQlType),
                name: nameof(Partial<object>.Items),
                description: "Partial items");
        }
    }
}