using System;
using GraphQl.Server.Annotations.Common;
using GraphQl.Server.Annotations.TypeResolvers;
using Microsoft.Extensions.DependencyInjection;

namespace GraphQl.Server.Annotations.Registrar.Configurators
{
    internal class Configurator : IConfigurator
    {
        private readonly IServiceCollection _services;
        private readonly IGraphTypeRegistry _typeRegistry;
        private readonly IGlobalGraphTypeResolvingConfigurator _typeResolvingConfigurator;


        public Configurator(IServiceCollection services, IGraphTypeRegistry typeRegistry, IGlobalGraphTypeResolvingConfigurator typeResolvingConfigurator)
        {
            _services = services;
            _typeRegistry = typeRegistry;
            _typeResolvingConfigurator = typeResolvingConfigurator;
        }


        public IConfigurator RegisterObject<T>(Action<IMappingConfigurator> configureAction = null) where T : class
        {
            _services.AddTransient<T>();
            var registeredType = _typeRegistry.RegisterObject(typeof(T));
            if (configureAction != null)
            {
                var configurator = new MappingConfigurator(registeredType, _typeRegistry);
                configureAction(configurator);
            }
            return this;
        }

        public IConfigurator RegisterInterface<T>(Action<IMappingConfigurator> configureAction = null) where T : class
        {
            var registeredType = _typeRegistry.RegisterInterface(typeof(T));
            if (configureAction != null)
            {
                var configurator = new MappingConfigurator(registeredType, _typeRegistry);
                configureAction(configurator);
            }
            return this;
        }

        public IConfigurator RegisterInputObject<T>() where T : class
        {
            _typeRegistry.RegisterInputObject(typeof(T));
            return this;
        }

        public IConfigurator ConfigureGraphTypeResolver(Action<IGlobalGraphTypeResolvingConfigurator> configureAction)
        {
            configureAction(_typeResolvingConfigurator);
            return this;
        }
    }
}