using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Roshambo.Shared;

namespace Roshambo.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        [HttpPost, Route("Play")]
        public IActionResult Play(GameOptions turn)
        {
            return Ok(new
            {
                YouPlayed = turn.ToString()
            });
        }
    }
}
