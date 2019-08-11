using GraphQl.Server.Annotations.Common.Helpers;

namespace GraphQl.Server.Annotations.Common.Schemas
{
    internal sealed class QuerySubscriptionSchema<TQuery, TSubscription> : SchemaBase
    {
        public QuerySubscriptionSchema(IGraphTypeRegistry typeRegistry)
            : base(typeRegistry)
        {
            Query = ActivatorHelper.CreateGraphQlObject(typeof(TQuery));
            Subscription = ActivatorHelper.CreateGraphQlObject(typeof(TSubscription));

            PopulateAdditionalTypes();
        }
    }
}