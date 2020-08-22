using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using Roshambo.Common;

namespace Roshambo.Twilio
{
    public class TwilioMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly Func<ITranslationService> _translationServiceFactory;

        public TwilioMiddleware(RequestDelegate next, Func<ITranslationService> translationServiceFactory)
        {
            _next = next;
            _translationServiceFactory = translationServiceFactory;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                var translationService = _translationServiceFactory.Invoke();

                var playerMove = GameOption.Rock;
                var computerMove = GameOption.Scissor;
                var winner = MoveWinner.Human;

                var text = await translationService.GetTextMessageBodyAsync(
                    playerMove, computerMove, winner);
                await WriteResponseAsync(context.Response, text);
            }
            catch (Exception e)
            {
                ServiceEventSource.Current.RuntimeException(e);
            }
        }

        private async Task WriteResponseAsync(HttpResponse response, string text)
        {
            response.StatusCode = 200;
            response.ContentType = "text/xml";
            await response.WriteAsync(@$"<?xml version=""1.0"" encoding=""UTF-8""?>
<Response>
    <Message>
        <Body>{text}</Body>
    </Message>
</Response>");
        }
    }
}
