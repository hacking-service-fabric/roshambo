using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Twilio.Security;

namespace Roshambo.Twilio
{
    public class TwilioValidationMiddleware: IMiddleware
    {
        private readonly IRequestDataProvider _requestDataProvider;
        private readonly RequestValidator _requestValidator;

        public TwilioValidationMiddleware(IRequestDataProvider requestDataProvider,
            RequestValidator requestValidator)
        {
            _requestDataProvider = requestDataProvider;
            _requestValidator = requestValidator;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var data = await _requestDataProvider.GetRequestDataAsync();

            if (data.IsLocal || _requestValidator.Validate(data.Url, data.Parameters, data.Signature))
            {
                await next(context);
            }
            else
            {
                context.Response.StatusCode = 400; // BadRequest
            }
        }
    }
}
