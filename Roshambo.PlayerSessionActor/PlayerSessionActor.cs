using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;
using Roshambo.Common;
using Roshambo.Common.Models;
using System;
using System.Threading.Tasks;

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
        private const string PreviousWinnerStateName = "previousWinner";
        private const string PreviousStreakStateName = "previousStreak";

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
            var result = new PlayerTurnResult();

            await UpdateNextComputerMoveAsync();
            result.NextMoveReady = true;

            if (turnWinner == TurnWinner.Tie)
            {
                var previousStreak = await StateManager.TryGetStateAsync<int>(PreviousStreakStateName);
                if (previousStreak.HasValue)
                {
                    result.CurrentStreak = result.PreviousStreak = previousStreak.Value;
                }

                return result;
            }

            var previousWinner = await StateManager.TryGetStateAsync<TurnWinner>(
                PreviousWinnerStateName);

            if (previousWinner.HasValue)
            {
                result.PreviousStreak = await StateManager.GetStateAsync<int>(
                    PreviousStreakStateName);
                result.StreakReset = previousWinner.Value != turnWinner;
            }
            else
            {
                result.StreakReset = true;
            }

            if (result.StreakReset)
            {
                result.CurrentStreak = 1;
            }
            else
            {
                result.CurrentStreak = result.PreviousStreak + 1;
            }

            await StateManager.SetStateAsync(PreviousStreakStateName, result.CurrentStreak);
            await StateManager.SetStateAsync(PreviousWinnerStateName, turnWinner);
            return result;
        }

        private async Task UpdateNextComputerMoveAsync()
        {
            var gameOptionService = _gameOptionServiceFactory.Invoke();
            var nextComputerMove = await gameOptionService.GetRandomOptionAsync();

            await StateManager.SetStateAsync(NextComputerMoveStateName, nextComputerMove);
        }
    }
}
