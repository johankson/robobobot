using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;
using Robobobot.Server.Models;
using Robobobot.Server.Services;
namespace Robobobot.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BattleController : ControllerBase
{
    private readonly BattleService battleService;
    
    public BattleController(BattleService battleService)
    {
        this.battleService = battleService;

    }
    
    [HttpPost]
    [Route("join")]
    public IActionResult Join([FromBody] JoinRequest joinRequest)
    {
        return new BadRequestObjectResult("The battle mode is not implemented yet. Please use the sandbox for now.");
    }
    
    /// <summary>
    /// Joins a new simulated battle for you to try practice your bot on.
    /// </summary>
    /// <param name="joinRequest">The request for creating a sandbox battle.</param>
    /// <returns>A JoinResponse with a battle Id and a Player Token.</returns>
    /// <remarks>The token to use is the player token. The Battle Id is representing the entire battle.</remarks>
    [HttpPost]
    [Route("join-sandbox")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult JoinSandbox([FromBody] JoinSandboxRequest joinRequest)
    {
        var (battle, player)= battleService.CreateSandboxBattle(joinRequest.Name, joinRequest.NumberOfBots);
        return new OkObjectResult(new JoinResponse(battle.BattleToken, player.Token));
    }

    [HttpGet]
    [Route("view-battle")]
    public IActionResult ViewBattle(string battleId)
    {
        // this should be protected by a viewer ID so clients don't use this info
        var battle = battleService.Get(battleId);
        if (battle is null)
        {
            return new NotFoundResult();
        }

        var renderer = new BattleRenderer(battle);
    
        return new OkObjectResult(renderer.RenderAsText());
    }
}