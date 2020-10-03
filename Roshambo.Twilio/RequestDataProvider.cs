using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Hosting;
using Roshambo.Twilio.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Roshambo.Twilio
{
    public class RequestDataProvider: IRequestDataProvider
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RequestDataProvider(IWebHostEnvironment environment, IHttpContextAccessor httpContextAccessor)
        {
            _environment = environment;
            _httpContextAccessor = httpContextAccessor;
        }

        private RequestData _requestData;

        public async Task<RequestData> GetRequestDataAsync()
            => _requestData ??= await _getRequestDataAsync();

        private async Task<RequestData> _getRequestDataAsync()
        {
            var result = new RequestData();

            var context = _httpContextAccessor.HttpContext;
            var request = context.Request;

            result.Url = request.GetDisplayUrl();
            if (_environment.IsDevelopment())
            {
                result.Url = new UriBuilder(result.Url)
                {
                    Scheme = request.Headers["X-Forwarded-Proto"]
                }.ToString();
            }

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
