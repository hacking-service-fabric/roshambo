using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Remoting;

namespace Roshambo.Common
{
    public interface IGameOptionService: IService
    {
        Task<GameOption> GetRandomOptionAsync();
    }
}
