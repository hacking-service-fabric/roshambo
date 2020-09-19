using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Roshambo.Twilio
{
    public class TwilioValidationMiddleware: IMiddleware
    {
        private readonly IRequestDataProvider _requestDataProvider;

        public TwilioValidationMiddleware(IRequestDataProvider requestDataProvider)
        {
            _requestDataProvider = requestDataProvider;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var data = await _requestDataProvider.GetRequestDataAsync();
            ServiceEventSource.Current.Message("Validator got {0}, {1}",
                data.Url, data.Signature);

            if (true)
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
