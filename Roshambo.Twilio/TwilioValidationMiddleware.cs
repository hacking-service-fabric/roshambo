using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Roshambo.Twilio
{
    public class TwilioValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IRequestDataProvider _requestDataProvider;

        public TwilioValidationMiddleware(RequestDelegate next,
            IRequestDataProvider requestDataProvider)
        {
            _next = next;
            _requestDataProvider = requestDataProvider;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var data = await _requestDataProvider.GetRequestData();
            ServiceEventSource.Current.Message("Validator got {0}, {1}",
                data.Body["Y"], data.Body["X"]);

            if (true == false)
            {
                await _next(context);
            }
            else
            {
                context.Response.StatusCode = 400; // BadRequest
            }
        }
    }
}
