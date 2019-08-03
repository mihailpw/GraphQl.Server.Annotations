using System;
using GraphQL;
using GraphQl.Server.Annotations.Common.Helpers;
using GraphQl.Server.Annotations.Providers;
using GraphQL.Types;

namespace GraphQl.Server.Annotations.Common.Types
{
    internal sealed class AutoEnumerationGraphType<T> : EnumerationGraphType<T> where T : Enum
    {
        public AutoEnumerationGraphType()
        {
            var serviceProvider = GlobalContext.ServiceProvider;

            var type = typeof(T);

            type.FindInAttributes<IGraphTypeInfoProvider>()?.Provide(this, type, serviceProvider);
        }


        protected override string ChangeEnumCase(string val)
        {
            return val.ToCamelCase();
        }
    }
}