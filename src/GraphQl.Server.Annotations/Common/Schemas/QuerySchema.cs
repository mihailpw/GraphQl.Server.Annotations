using GraphQl.Server.Annotations.Common.Helpers;

namespace GraphQl.Server.Annotations.Common.Schemas
{
    internal sealed class QuerySchema<TQuery> : SchemaBase
    {
        public QuerySchema(IGraphQlTypeRegistry typeRegistry)
            : base(typeRegistry)
        {
            Query = ActivatorHelper.CreateGraphQlObject(typeof(TQuery));

            PopulateAdditionalTypes();
        }
    }
}