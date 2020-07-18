using System;
using System.Collections.Generic;
using System.Fabric;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Roshambo.GettingStarted.Interfaces;

namespace Roshambo.GettingStarted
{
    using Microsoft.ServiceFabric.Services.Remoting.Runtime;

    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    internal sealed class GettingStarted : StatelessService, IGettingStartedService
    {
        public GettingStarted(StatelessServiceContext context)
            : base(context)
        { }

        /// <summary>
        /// Optional override to create listeners (e.g., TCP, HTTP) for this service replica to handle client or user requests.
        /// </summary>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return this.CreateServiceRemotingInstanceListeners();
        }

        /// <summary>
        /// This is the main entry point for your service instance.
        /// </summary>
        /// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service instance.</param>
        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            var iterations = 0;
            var game = new GameEngine();

            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var result = game.PlayRound();
                ServiceEventSource.Current.ServiceMessage(this.Context, "{0} against {1} => {2}",
                    result.PlayerOption, result.ComputerOption, result.Result switch
                    {
                        WinOptions.Won => "Computer 1",
                        WinOptions.Lost => "Computer 2",
                        WinOptions.Tied => "Tie",
                        _ => "Inconceivable!"
                    });

                ServiceEventSource.Current.ServiceMessage(this.Context, "Working-{0}", ++iterations);

                await Task.Delay(TimeSpan.FromMinutes(1), cancellationToken);
            }
        }

        public Task<GameResult> Play(GameOptions playerOption)
        {
            var game = new GameEngine();
            return Task.FromResult(game.PlayRound(playerOption));
        }
    }
}
