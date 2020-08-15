using System.Fabric;
using Microsoft.Extensions.DependencyInjection;

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
    }
}
