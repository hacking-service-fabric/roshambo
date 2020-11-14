using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Roshambo.Common;
using Roshambo.Common.Models;

namespace Roshambo.GameServices
{
    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    internal sealed class GameServices : StatelessService, IGameOptionService, IGameService
    {
        private static readonly Random _randomGenerator = new Random();

        public GameServices(StatelessServiceContext context)
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
            // TODO: Replace the following sample code with your own logic 
            //       or remove this RunAsync override if it's not needed in your service.

            long iterations = 0;

            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();

                ServiceEventSource.Current.ServiceMessage(this.Context, "Working-{0}", ++iterations);

                await Task.Delay(TimeSpan.FromMinutes(1), cancellationToken);
            }
        }

        public Task<GameOption> GetRandomOptionAsync()
        {
            var randomNumber = _randomGenerator.Next(0, 3);
            return Task.FromResult((GameOption)randomNumber);
        }

        public Task<TurnWinner> JudgeTurnAsync(GameOption playerMove, GameOption computerMove)
        {
            if (playerMove == computerMove)
                return Task.FromResult(TurnWinner.Tie);

            var playerWon = (playerMove == GameOption.Rock && computerMove == GameOption.Scissor)
               || (playerMove == GameOption.Paper && computerMove == GameOption.Rock)
               || (playerMove == GameOption.Scissor && computerMove == GameOption.Paper);

            return Task.FromResult(playerWon ? TurnWinner.Human : TurnWinner.Computer);
        }
    }
}
