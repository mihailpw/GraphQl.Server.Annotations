using System;
using GraphQl.Server.Annotations.TypeResolvers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace GraphQl.Server.Annotations.Common
{
    internal static class GlobalContext
    {
        private static IServiceProvider _provider;


        public static IGraphTypeRegistry TypeRegistry => _provider.GetService<IGraphTypeRegistry>();
        public static IGlobalGraphTypeResolver TypeResolver => _provider.GetService<IGlobalGraphTypeResolver>();
        public static IGraphPartsFactory PartsFactory => _provider.GetService<IGraphPartsFactory>();

        public static IServiceProvider ServiceProvider { get; private set; }
        public static IConfig Config { get; private set; }


        public static void Populate(IServiceProvider serviceProvider)
        {
            _provider = serviceProvider;

            ServiceProvider = new RequestServicesProvider(serviceProvider.GetService<IHttpContextAccessor>());
            Config = new Config();
        }
    }
}