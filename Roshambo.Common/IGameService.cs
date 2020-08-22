using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Remoting;

namespace Roshambo.Common
{
    public interface IGameService: IService
    {
        Task<MoveWinner> JudgeMoveAsync(GameOption playerMove, GameOption computerMove);
    }
}
