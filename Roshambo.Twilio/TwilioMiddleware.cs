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
        private readonly Func<string, IPlayerSession> _playerSessionProvider;

        public TwilioMiddleware(RequestDelegate next,
            Func<ITranslationService> translationServiceFactory,
            Func<string, IPlayerSession> playerSessionProvider)
        {
            _next = next;
            _translationServiceFactory = translationServiceFactory;
            _playerSessionProvider = playerSessionProvider;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                var translationService = _translationServiceFactory.Invoke();
                var playerSession = _playerSessionProvider.Invoke("test");

                var playerMove = GameOption.Rock;
                var computerMove = await playerSession.GetComputerMoveAsync();
                var winner = MoveWinner.Human;

                var text = await translationService.GetTextMessageBodyAsync(
                    playerMove, computerMove, winner);
                await WriteResponseAsync(context.Response, text);
            }
            catch (Exception e)
            {
                ServiceEventSource.Current.RuntimeException(e);
                await WriteErrorResponseAsync(context.Response);
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

        private Task WriteErrorResponseAsync(HttpResponse response)
        {
            response.StatusCode = 500;
            return Task.CompletedTask;
        }
    }
}
