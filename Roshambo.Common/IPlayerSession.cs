using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using Roshambo.Common.Models;

namespace Roshambo.Common
{
    public interface IPlayerSession: IActor
    {
        Task<GameOption> GetComputerMoveAsync();
        Task SaveNextComputerMoveAsync(GameOption move);

        Task<PlayerTurnResult> StoreTurnOutcomeAsync(TurnWinner turnWinner);
    }
}
