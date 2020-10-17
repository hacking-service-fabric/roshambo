using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;
using Microsoft.ServiceFabric.Actors.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using Roshambo.Common;
using Roshambo.Common.Models;

namespace Roshambo.PlayerSessionActor
{
    /// <remarks>
    /// This class represents an actor.
    /// Every ActorID maps to an instance of this class.
    /// The StatePersistence attribute determines persistence and replication of actor state:
    ///  - Persisted: State is written to disk and replicated.
    ///  - Volatile: State is kept in memory only and replicated.
    ///  - None: State is kept in memory only and not replicated.
    /// </remarks>
    [StatePersistence(StatePersistence.Persisted)]
    public class PlayerSessionActor : Actor, IPlayerSession
    {
        private const string NextComputerMoveStateName = "nextComputerMove";

        private readonly Func<IGameOptionService> _gameOptionServiceFactory;

        /// <summary>
        /// Initializes a new instance of PlayerSessionActor
        /// </summary>
        /// <param name="actorService">The Microsoft.ServiceFabric.Actors.Runtime.ActorService that will host this actor instance.</param>
        /// <param name="actorId">The Microsoft.ServiceFabric.Actors.ActorId for this actor instance.</param>
        public PlayerSessionActor(ActorService actorService, ActorId actorId) 
            : base(actorService, actorId)
        {
            _gameOptionServiceFactory = ServiceExtensions.GetGameOptionServiceFactory();
        }

        /// <summary>
        /// This method is called whenever an actor is activated.
        /// An actor is activated the first time any of its methods are invoked.
        /// </summary>
        protected override async Task OnActivateAsync()
        {
            ActorEventSource.Current.ActorMessage(this, "Actor activated.");

            var hasNextComputerMove = await StateManager.ContainsStateAsync(NextComputerMoveStateName);
            if (!hasNextComputerMove)
            {
                await UpdateNextComputerMoveAsync();
            }
        }

        public async Task<GameOption> GetComputerMoveAsync()
        {
            return await StateManager.GetStateAsync<GameOption>(NextComputerMoveStateName);
        }

        public async Task<PlayerTurnResult> StoreTurnOutcomeAsync(TurnWinner turnWinner)
        {
            await UpdateNextComputerMoveAsync();

            return new PlayerTurnResult
            {
                StreakReset = false,
                CurrentStreak = 7,
                PreviousStreak = 6,
                NextMoveReady = true
            };
            // TODO: Implement
        }

        private async Task UpdateNextComputerMoveAsync()
        {
            var gameOptionService = _gameOptionServiceFactory.Invoke();
            var nextComputerMove = await gameOptionService.GetRandomOptionAsync();

            await StateManager.SetStateAsync(NextComputerMoveStateName, nextComputerMove);
        }
    }
}
