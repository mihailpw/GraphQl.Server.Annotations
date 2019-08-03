using System;
using System.Reflection;
using GraphQL.Types;

namespace GraphQl.Server.Annotations.Providers
{
    public interface IQueryArgumentInfoProvider
    {
        void Provide(QueryArgument queryArgument, ParameterInfo parameterInfo, IServiceProvider serviceProvider);
    }
}