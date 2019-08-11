using System;
using System.Collections.Generic;
using GraphQl.Server.Annotations.TypeResolvers;

namespace GraphQl.Server.Annotations.Registrar.Configurators
{
    public interface IGlobalGraphTypeResolvingConfigurator
    {
        IEnumerable<IGraphTypeResolver> TypeResolvers { get; }

        IGlobalGraphTypeResolvingConfigurator WithTypePreparer(ITypePreparer typePreparer);
        IGlobalGraphTypeResolvingConfigurator ClearGraphTypeResolvers();
        IGlobalGraphTypeResolvingConfigurator AddGraphTypeResolver(IGraphTypeResolver graphTypeResolver);
        IGlobalGraphTypeResolvingConfigurator AddGraphTypeResolver(Func<IServiceProvider, IGraphTypeResolver> graphTypeResolverFactory);
    }
}