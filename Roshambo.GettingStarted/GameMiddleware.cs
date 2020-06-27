using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Roshambo.Shared;

namespace Roshambo.GettingStarted
{
    class GameMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly GameEngine _gameEngine;

        public GameMiddleware(RequestDelegate next, GameEngine gameEngine)
        {
            _next = next;
            _gameEngine = gameEngine;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Query.TryGetValue("play", out var optionPlayedRaw) &&
                Enum.TryParse(typeof(GameOptions), optionPlayedRaw, true, out var optionPlayed))
            {
                var result = _gameEngine.PlayRound((GameOptions) optionPlayed);
                await context.Response.WriteAsync(JsonConvert.SerializeObject(new
                {
                    computerPlayed = result.Computer1,
                    humanWon = result.Winner == "Computer1",
                    tied = result.Winner == "Tie"
                }));
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
        }
    }
}
