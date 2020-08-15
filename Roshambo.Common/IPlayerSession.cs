using System.Threading.Tasks;

namespace Roshambo.Common
{
    public interface IPlayerSession
    {
        Task<GameOption> GetComputerTurnAsync();
        Task SaveNextComputerTurnAsync(GameOption turn);

        Task<int> StoreTurnOutcomeAndGetStreakAsync(TurnWinner turnWinner);
    }
}
