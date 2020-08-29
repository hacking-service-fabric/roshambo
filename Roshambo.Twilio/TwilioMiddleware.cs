using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using Roshambo.Common;
using Roshambo.Common.Models;

namespace Roshambo.Twilio
{
    public class TwilioMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly Func<ITranslationService> _translationServiceFactory;
        private readonly Func<string, IPlayerSession> _playerSessionProvider;
        private readonly Func<IGameService> _gameServiceFactory;

        public TwilioMiddleware(RequestDelegate next,
            Func<ITranslationService> translationServiceFactory,
            Func<string, IPlayerSession> playerSessionProvider,
            Func<IGameService> gameServiceFactory)
        {
            _next = next;
            _translationServiceFactory = translationServiceFactory;
            _playerSessionProvider = playerSessionProvider;
            _gameServiceFactory = gameServiceFactory;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // TODO: Validate AccountSid

                var translationService = _translationServiceFactory.Invoke();
                var playerSession = _playerSessionProvider.Invoke("test"); // TODO: From
                var gameService = _gameServiceFactory.Invoke();

                var playerMove = await translationService.GetUserInputAsync("bla"); // TODO: Body
                var computerMove = await playerSession.GetComputerMoveAsync();
                var winner = await gameService.JudgeTurnAsync(playerMove, computerMove);
                var playerTurnResult = await playerSession.StoreTurnOutcomeAsync(winner);

                var text = await translationService.GetTextMessageBodyAsync(
                    playerMove, computerMove, winner, playerTurnResult);
                await WriteResponseAsync(context.Response, text);
            }
            catch (Exception e)
            {
                ServiceEventSource.Current.RuntimeException(
                    e.Message,
                    e.StackTrace);
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
