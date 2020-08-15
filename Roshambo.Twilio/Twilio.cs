using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ServiceFabric.Services.Communication.AspNetCore;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Roshambo.Common;
using System;
using System.Collections.Generic;
using System.Fabric;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;

namespace Roshambo.Twilio
{
    /// <summary>
    /// The FabricRuntime creates an instance of this class for each service type instance. 
    /// </summary>
    internal sealed class Twilio : StatelessService, ITranslationService
    {
        public Twilio(StatelessServiceContext context)
            : base(context)
        { }

        /// <summary>
        /// Optional override to create listeners (like tcp, http) for this service instance.
        /// </summary>
        /// <returns>The collection of listeners.</returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return Enumerable.Union(
                this.CreateServiceRemotingInstanceListeners(),
                new[]
                {
                    new ServiceInstanceListener(serviceContext =>
                        new KestrelCommunicationListener(serviceContext, "ServiceEndpoint", (url, listener) =>
                        {
                            ServiceEventSource.Current.ServiceMessage(serviceContext, $"Starting Kestrel on {url}");

                            return new WebHostBuilder()
                                        .UseKestrel()
                                        .ConfigureServices(
                                            services => services
                                                .AddSingleton<StatelessServiceContext>(serviceContext))
                                        .UseContentRoot(Directory.GetCurrentDirectory())
                                        .UseStartup<Startup>()
                                        .UseServiceFabricIntegration(listener, ServiceFabricIntegrationOptions.None)
                                        .UseUrls(url)
                                        .Build();
                        }))
                });
        }

        public Task<GameOption> GetUserInputAsync(string input)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetTextMessageBody(GameOption playerTurn, GameOption computerTurn, TurnWinner winner)
        {
            throw new NotImplementedException();
        }
    }
}
