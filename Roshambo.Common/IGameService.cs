using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Remoting;
using Roshambo.Common.Models;

namespace Roshambo.Common
{
    public interface IGameService: IService
    {
        Task<TurnWinner> JudgeTurnAsync(GameOption playerMove, GameOption computerMove);
    }
}
