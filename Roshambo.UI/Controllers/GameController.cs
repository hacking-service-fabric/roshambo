using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using Roshambo.GettingStarted.Interfaces;
using Roshambo.PlayerActor.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Roshambo.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        [HttpPost, Route("Play")]
        public async Task<IActionResult> Play(GameOptions turn)
        {
            var service = ServiceProxy.Create<IGettingStartedService>(
                new Uri("fabric:/Roshambo.App/Roshambo.GettingStarted"));
            var gameResult = await service.Play(turn);

            var player = ActorProxy.Create<IPlayerActor>(new ActorId("testing"),
                new Uri("fabric:/Roshambo.App/PlayerActorService"));
            var streak = await player.RefreshStreakAsync(gameResult.Result, CancellationToken.None);

            return Ok(new
            {
                Player = gameResult.PlayerOption.ToString(),
                Computer = gameResult.ComputerOption.ToString(),
                PlayerResult = gameResult.Result.ToString(),
                Streak = streak
            });
        }
    }
}
