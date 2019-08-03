using GraphQl.Server.Annotations.Common.Helpers;

namespace GraphQl.Server.Annotations.Common.Schemas
{
    internal sealed class QueryMutationSchema<TQuery, TMutation> : SchemaBase
    {
        public QueryMutationSchema(IGraphQlTypeRegistry typeRegistry)
            : base(typeRegistry)
        {
            Query = ActivatorHelper.CreateGraphQlObject(typeof(TQuery));
            Mutation = ActivatorHelper.CreateGraphQlObject(typeof(TMutation));

            PopulateAdditionalTypes();
        }
    }
}