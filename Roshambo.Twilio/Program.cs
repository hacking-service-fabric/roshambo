using Microsoft.ServiceFabric.Services.Runtime;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Roshambo.Common;
using Roshambo.Twilio.Implementations;

namespace Roshambo.Twilio
{
    internal static class Program
    {
        /// <summary>
        /// This is the entry point of the service host process.
        /// </summary>
        private static async Task Main()
        {
            var services = new ServiceCollection();

            services
                .AddSingleton<IResponseSentenceProvider,
                    ResponseResultSentenceProvider>()
                .AddSingleton<IResponseSentenceProvider,
                    ResponseStreakSentenceProvider>()
                .AddSingleton<IResponseSentenceProvider,
                    ResponseNextMoveSentenceProvider>();

            services.AddSingleton<Twilio>();

            try
            {
                // The ServiceManifest.XML file defines one or more service type names.
                // Registering a service maps a service type name to a .NET type.
                // When Service Fabric creates an instance of this service type,
                // an instance of the class is created in this host process.

                await ServiceRuntime.RegisterServiceAsync("Roshambo.TwilioType",
                    context =>
                    {
                        services.AddStatelessService(context);
                        return services.BuildServiceProvider()
                            .GetRequiredService<Twilio>();
                    });

                ServiceEventSource.Current.ServiceTypeRegistered(Process.GetCurrentProcess().Id, typeof(Twilio).Name);

                // Prevents this host process from terminating so services keeps running. 
                Thread.Sleep(Timeout.Infinite);
            }
            catch (Exception e)
            {
                ServiceEventSource.Current.ServiceHostInitializationFailed(e.ToString());
                throw;
            }
        }
    }
}
