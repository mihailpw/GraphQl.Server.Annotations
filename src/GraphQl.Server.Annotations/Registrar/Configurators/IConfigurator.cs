using System;

namespace GraphQl.Server.Annotations.Registrar.Configurators
{
    public interface IConfigurator
    {
        IConfigurator RegisterObject<T>(Action<IMappingConfigurator> configureAction = null) where T : class;
        IConfigurator RegisterInterface<T>(Action<IMappingConfigurator> configureAction = null) where T : class;
        IConfigurator RegisterInputObject<T>() where T : class;

        IConfigurator ConfigureGraphTypeResolver(Action<IGlobalGraphTypeResolvingConfigurator> configureAction);
    }
}