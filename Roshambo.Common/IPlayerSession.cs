using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;

namespace Roshambo.Common
{
    public interface IPlayerSession: IActor
    {
        Task<GameOption> GetComputerMoveAsync();
        Task SaveNextComputerMoveAsync(GameOption move);

        Task<int> StoreMoveOutcomeAndGetStreakAsync(MoveWinner moveWinner);
    }
}
