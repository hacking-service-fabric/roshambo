using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.ServiceFabric.Services.Communication.AspNetCore;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Roshambo.Common;
using Roshambo.Common.Models;
using System.Collections.Generic;
using System.Fabric;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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
                        new KestrelCommunicationListener(serviceContext, "PublicEndpoint", (url, listener) =>
                        {
                            ServiceEventSource.Current.ServiceMessage(serviceContext, $"Starting Kestrel on {url}");

                            return new WebHostBuilder()
                                        .UseKestrel()
                                        .UseConfiguration(new ConfigurationBuilder()
                                            .AddEnvironmentVariables("ROSHAMBO:")
                                            .Build())
                                        .ConfigureServices(
                                            services => services.AddStatelessService(serviceContext))
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
            return Task.FromResult(GameOption.Rock);
            // TODO: Implement
        }

        public Task<string> GetTextMessageBodyAsync(
            GameOption playerMove, GameOption computerMove,
            TurnWinner winner, PlayerTurnResult playerTurnResult)
        {
            return Task.FromResult(winner switch
            {
                TurnWinner.Human => $"{playerMove} beat {computerMove}.",
                TurnWinner.Computer => $"{playerMove} lost to {computerMove}.",
                TurnWinner.Tie => "Tie!",
                _ => $"I didn't recognize the input {winner} :("
            });
        }
    }
}
