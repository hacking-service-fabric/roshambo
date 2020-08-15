using System.Threading.Tasks;

namespace Roshambo.Common
{
    public interface IGameOptionService
    {
        Task<GameOption> GetRandomOptionAsync();
    }
}
