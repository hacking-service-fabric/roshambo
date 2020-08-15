using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Remoting;

namespace Roshambo.Common
{
    public interface ITranslationService: IService
    {
        Task<GameOption> GetUserInputAsync(string input);
        Task<string> GetTextMessageBody(GameOption playerTurn, GameOption computerTurn, TurnWinner winner);
    }
}
