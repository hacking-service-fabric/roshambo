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
            var result = new RequestData();

            var context = _httpContextAccessor.HttpContext;
            var request = context.Request;

            result.Url = request.GetDisplayUrl();
            result.IsLocal = request.Host.Host.ToLower() == "localhost";
            result.Signature = request.Headers["X-Twilio-Signature"];

            using var bodyStream = new StreamReader(request.Body);
            var body = await bodyStream.ReadToEndAsync();

            result.Parameters = QueryHelpers.ParseQuery(body)
                .ToDictionary(
                    k => k.Key,
                    v => v.Value.First());

            return result;
        }
    }
}
