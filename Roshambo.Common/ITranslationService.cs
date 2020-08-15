using System.Threading.Tasks;

namespace Roshambo.Common
{
    public interface ITranslationService
    {
        Task<GameOption> GetUserInputAsync(string input);
        Task<string> GetTextMessageBody(GameOption playerTurn, GameOption computerTurn, TurnWinner winner);
    }
}
