using System;
using GraphQL.Types;

namespace GraphQl.Server.Annotations.Providers
{
    public interface IGraphTypeInfoProvider
    {
        void Provide(GraphType graphType, Type type, IServiceProvider serviceProvider);
    }
}