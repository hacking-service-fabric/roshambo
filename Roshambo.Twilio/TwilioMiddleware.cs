using Microsoft.AspNetCore.Http;
using Roshambo.Common;
using System;
using System.Threading.Tasks;

namespace Roshambo.Twilio
{
    public class TwilioMiddleware: IMiddleware
    {
        private readonly Func<ITranslationService> _translationServiceFactory;
        private readonly Func<string, IPlayerSession> _playerSessionProvider;
        private readonly Func<IGameService> _gameServiceFactory;
        private readonly IRequestDataProvider _requestDataProvider;

        public TwilioMiddleware(
            Func<ITranslationService> translationServiceFactory,
            Func<string, IPlayerSession> playerSessionProvider,
            Func<IGameService> gameServiceFactory,
            IRequestDataProvider requestDataProvider)
        {
            _translationServiceFactory = translationServiceFactory;
            _playerSessionProvider = playerSessionProvider;
            _gameServiceFactory = gameServiceFactory;
            _requestDataProvider = requestDataProvider;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var data = await _requestDataProvider.GetRequestDataAsync();
            ServiceEventSource.Current.Message("Endpoint got {0}, {1}",
                data.Url, data.Signature);

            try
            {
                var translationService = _translationServiceFactory.Invoke();
                var playerSession = _playerSessionProvider.Invoke(
                    data.Parameters["From"]);
                var gameService = _gameServiceFactory.Invoke();

                var playerMove = await translationService.GetUserInputAsync(
                    data.Parameters["Body"]);
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
