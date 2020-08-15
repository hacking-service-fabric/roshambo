using System.Threading.Tasks;

namespace Roshambo.Common
{
    public interface IGameService
    {
        Task<TurnWinner> JudgeTurnAsync(GameOption playerTurn, GameOption computerTurn);
    }
}
