using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;

namespace Roshambo.Common
{
    public interface IPlayerSession: IActor
    {
        Task<GameOption> GetComputerTurnAsync();
        Task SaveNextComputerTurnAsync(GameOption turn);

        Task<int> StoreTurnOutcomeAndGetStreakAsync(TurnWinner turnWinner);
    }
}
