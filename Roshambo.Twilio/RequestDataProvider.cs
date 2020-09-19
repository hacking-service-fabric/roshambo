using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.WebUtilities;
using Roshambo.Twilio.Models;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Roshambo.Twilio
{
    public class RequestDataProvider: IRequestDataProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RequestDataProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<RequestData> GetRequestDataAsync()
        {
            var request = _httpContextAccessor.HttpContext.Request;
            using var bodyStream = new StreamReader(request.Body);

            var body = await bodyStream.ReadToEndAsync();

            var parameters = QueryHelpers.ParseQuery(body)
                .ToDictionary(
                    k => k.Key,
                    v => v.Value.First());

            return new RequestData
            {
                Url = request.GetDisplayUrl(),
                Signature = request.Headers["X-Twilio-Signature"],
                Parameters = parameters
            };
        }
    }
}
