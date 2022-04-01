using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Robobobot.Core;
using Robobobot.Core.Actions;
using Robobobot.Core.Models;
using Robobobot.Server.BackgroundServices;

namespace Robobobot.Server.Controllers;

[ApiController]
[Produces("application/json")]
[Route("api/[controller]")]
public class BattleController : ControllerBase
{
    private readonly BattleService battleService;
    private readonly IFpsController fpsController;
    private readonly ILogger<BattleController> logger;

    public BattleController(BattleService battleService, IFpsController fpsController, ILogger<BattleController> logger)
    {
        this.battleService = battleService;
        this.fpsController = fpsController;
        this.logger = logger;
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
    [HttpPost()]
    [Route("join-sandbox")]
    [ProducesResponseType(typeof(JoinResponse), StatusCodes.Status200OK)]
    public IActionResult JoinSandbox([FromBody] JoinSandboxRequest joinRequest)
    {
        var (battle, player)= battleService.CreateSandboxBattle(joinRequest.Name, joinRequest.NumberOfBots, joinRequest.BattleFieldOptions);
        return new OkObjectResult(new JoinResponse(battle.BattleToken, player.Token));
    }
    
    [HttpGet]
    [Route("view-battle-raw")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public IActionResult ViewBattleRaw(string battleId)
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
        if(fpsController.State == FpsControllerState.Paused)
            fpsController.Resume();
        
        try
        {
            if (!battleService.RequestActionLock(playerHeaders.Token))
            {
                return new BadRequestObjectResult("You already have a pending action");
            }

            // The get the battle and player
            var battle = battleService.GetBattleByPlayerToken(playerHeaders.Token);
            if (battle is null) return new BadRequestObjectResult("Could not find a battle for the given token...");

            var player = battleService.GetPlayerByToken(playerHeaders.Token);
            if(player is null)  return new BadRequestObjectResult("Could not find a player for the given token...");
            
            logger.LogInformation("Enqueueing GetVisual Action");
            var action = await battle.EnqueueAndWait(new GetVisualAction(player));
            if (action.Result == null)
            {
                return new BadRequestResult();
            }
            
            logger.LogInformation("Completed GetVisual Action, waiting the specified time");
            await Task.Delay(action.Result.ExecutionDuration);

            logger.LogInformation("Returning result to client");
            return new OkObjectResult(action.Result);

            return new BadRequestResult();
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