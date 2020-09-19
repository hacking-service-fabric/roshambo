using Roshambo.Twilio.Models;
using System.Threading.Tasks;

namespace Roshambo.Twilio
{
    public interface IRequestDataProvider
    {
        Task<RequestData> GetRequestDataAsync();
    }
}
