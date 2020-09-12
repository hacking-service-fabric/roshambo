using Microsoft.AspNetCore.Http;
using Roshambo.Twilio.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Roshambo.Twilio
{
    public class RequestDataProvider: IRequestDataProvider
    {
        private static readonly Random Random = new Random();
        private readonly int _randomNumber = Random.Next();

        private readonly IHttpContextAccessor _httpContextAccessor;

        public RequestDataProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Task<RequestData> GetRequestData()
        {
            return Task.FromResult(new RequestData
            {
                Body = new Dictionary<string, string>
                {
                    ["X"] = _httpContextAccessor.HttpContext.Request.Headers["X-Twilio-Signature"],
                    ["Y"] = _randomNumber.ToString()
                }
            });
        }
    }
}
