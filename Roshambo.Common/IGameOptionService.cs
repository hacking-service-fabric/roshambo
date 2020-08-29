using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Remoting;
using Roshambo.Common.Models;

namespace Roshambo.Common
{
    public interface IGameOptionService: IService
    {
        Task<GameOption> GetRandomOptionAsync();
    }
}
