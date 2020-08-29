using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Remoting;
using Roshambo.Common.Models;

namespace Roshambo.Common
{
    public interface ITranslationService: IService
    {
        Task<GameOption> GetUserInputAsync(string input);
        Task<string> GetTextMessageBodyAsync(GameOption playerMove, GameOption computerMove, TurnWinner winner);
    }
}
