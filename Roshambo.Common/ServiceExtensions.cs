using System;
using System.Fabric;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ServiceFabric.Services.Remoting.Client;

namespace Roshambo.Common
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddStatelessService(
            this IServiceCollection services,
            StatelessServiceContext statelessService)
        {
            return services.AddSingleton(statelessService);
        }

        public static IServiceCollection AddStatefulService(
            this IServiceCollection services,
            StatefulServiceContext statefulService)
        {
            return services.AddSingleton(statefulService);
        }

        public static IServiceCollection AddTranslationService(
            this IServiceCollection services)
        {
            return services.AddSingleton<Func<ITranslationService>>(
                () => ServiceProxy.Create<ITranslationService>(
                    new Uri("fabric:/Roshambo.App/Roshambo.Twilio")));
        }
    }
}
