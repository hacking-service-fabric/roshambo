using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using Roshambo.GettingStarted.Interfaces;
using Roshambo.PlayerActor.Interfaces;
using Roshambo.Shared;

namespace Roshambo.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        [HttpPost, Route("Play")]
        public async Task<IActionResult> Play(GameOptions turn)
        {
            var player = ActorProxy.Create<IPlayerActor>(new ActorId("testing"),
                new Uri("fabric:/Roshambo.App/PlayerActorService"));

            var count = await player.GetCountAsync(CancellationToken.None);

            await player.SetCountAsync((int) turn, CancellationToken.None);
            return Ok(new
            {
                YouPlayed = turn.ToString(),
                ActorSaid = ((GameOptions)count).ToString()
            });
        }

        [HttpPost, Route("Try")]
        public async Task<IActionResult> Try()
        {
            var service = ServiceProxy.Create<IGettingStartedService>(
                new Uri("fabric:/Roshambo.App/Roshambo.GettingStarted"));

            await service.DoSomething();

            return Ok();
        }
    }
}
