using Microsoft.ServiceFabric.Services.Remoting;
using System.Threading.Tasks;

namespace Roshambo.GettingStarted.Interfaces
{
    public interface IGettingStartedService: IService
    {
        Task<GameResult> Play(GameOptions playerOption);
    }
}
