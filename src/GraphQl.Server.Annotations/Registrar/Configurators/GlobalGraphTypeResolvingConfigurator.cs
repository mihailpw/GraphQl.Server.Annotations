using System;
using System.Collections.Generic;
using GraphQl.Server.Annotations.Common;
using GraphQl.Server.Annotations.Common.Helpers;
using GraphQl.Server.Annotations.TypeResolvers;
using GraphQl.Server.Annotations.TypeResolvers.Core;
using GraphQl.Server.Annotations.TypeResolvers.Resolvers;

namespace GraphQl.Server.Annotations.Registrar.Configurators
{
    internal class GlobalGraphTypeResolvingConfigurator : IGlobalGraphTypeResolvingConfigurator
    {
        private readonly GlobalGraphTypeResolverStorage _globalGraphTypeResolverStorage;


        public GlobalGraphTypeResolvingConfigurator(
            GlobalGraphTypeResolverStorage globalGraphTypeResolverStorage,
            IGraphTypeRegistry graphTypeRegistry)
        {
            _globalGraphTypeResolverStorage = globalGraphTypeResolverStorage;

            _globalGraphTypeResolverStorage.TypePreparer = new DefaultTypePreparer();
            _globalGraphTypeResolverStorage.GraphTypeResolvers.Add(new RegistryGraphTypeResolver(graphTypeRegistry));
            _globalGraphTypeResolverStorage.GraphTypeResolvers.Add(new IdGraphTypeResolver());
            _globalGraphTypeResolverStorage.GraphTypeResolvers.Add(new NonNullGraphTypeResolver(globalGraphTypeResolverStorage));
            _globalGraphTypeResolverStorage.GraphTypeResolvers.Add(new ListGraphTypeResolver(globalGraphTypeResolverStorage));
            _globalGraphTypeResolverStorage.GraphTypeResolvers.Add(new PageGraphTypeResolver());
            _globalGraphTypeResolverStorage.GraphTypeResolvers.Add(new PartialGraphTypeResolver());
            _globalGraphTypeResolverStorage.GraphTypeResolvers.Add(new AutoEnumGraphTypeResolver(graphTypeRegistry));
        }


        public IEnumerable<IGraphTypeResolver> TypeResolvers => _globalGraphTypeResolverStorage.GraphTypeResolvers;

        public IGlobalGraphTypeResolvingConfigurator WithTypePreparer(ITypePreparer typePreparer)
        {
            typePreparer.VerifyNotNull(nameof(typePreparer));
            _globalGraphTypeResolverStorage.TypePreparer = typePreparer;
            return this;
        }

        public IGlobalGraphTypeResolvingConfigurator ClearGraphTypeResolvers()
        {
            _globalGraphTypeResolverStorage.GraphTypeResolvers.Clear();
            return this;
        }

        public IGlobalGraphTypeResolvingConfigurator AddGraphTypeResolver(IGraphTypeResolver graphTypeResolver)
        {
            graphTypeResolver.VerifyNotNull(nameof(graphTypeResolver));
            _globalGraphTypeResolverStorage.GraphTypeResolvers.Add(graphTypeResolver);
            return this;
        }

        public IGlobalGraphTypeResolvingConfigurator AddGraphTypeResolver(Func<IServiceProvider, IGraphTypeResolver> graphTypeResolverFactory)
        {
            throw new NotImplementedException();
        }
    }
}