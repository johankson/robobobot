using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Robobobot.Core;
using Robobobot.Server.BackgroundServices;
using Robobobot.Server.Models;
using Robobobot.Server.Services;
namespace Robobobot.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BattleController : ControllerBase
{
    private readonly BattleService battleService;
    private readonly IFpsController fpsController;

    public BattleController(BattleService battleService, IFpsController fpsController)
    {
        this.battleService = battleService;
        this.fpsController = fpsController;
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

    /// <summary>
    /// Gets the playing fields visual representation seen from your tank.
    /// </summary>
    /// <param name="playerHeaders">The headers.</param>
    /// <returns></returns>
    [HttpGet]
    [Route("get-visual")]
    public async Task<IActionResult> GetVisual([FromQuery] PlayerHeaders playerHeaders)
    {
        try
        {
            if (!battleService.RequestActionLock(playerHeaders.Token))
            {
                return new BadRequestObjectResult("You already have a pending action");
            }

            // The build in delay
            var battle = battleService.GetBattleByPlayerToken(playerHeaders.Token);
            if (battle is null)
            {
                return new BadRequestObjectResult("Could not find a battle for the given token...");
            }
            
            // This should be fetched by config for that specific command and settings
            await Task.Delay(3000);
        
            // Concept code
            return new OkObjectResult(new GetVisualResponse(battle.RenderPlayerVisual(playerHeaders.Token)));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        finally
        {
            battleService.ReleaseActionLock(playerHeaders.Token);
        }
    }
}

public record GetVisualResponse(string Battlefield);

public record PlayerHeaders
{
    /// <summary>
    /// The player token for a given battle and player. This is a required header.
    /// </summary>
    [FromHeader]
    [Required]
    public string Token { get; set; } = string.Empty;
}