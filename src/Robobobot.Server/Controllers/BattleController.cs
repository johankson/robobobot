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
        var (battle, player)= battleService.CreateSandboxBattle(joinRequest.Name, joinRequest.BattleFieldOptions, joinRequest.SandboxOptions);
       
        if (fpsController.State != FpsControllerState.Running)
        {
            battleService.ServerLog.Log($"Server not running, starting it at {fpsController.Fps} fps");
            fpsController.Resume();
        }
        
        return new OkObjectResult(new JoinResponse(battle.BattleToken, player.Token, player.Name));
    }
    
    [HttpGet]
    [Route("view-battle-raw")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [Produces("text/plain")]
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
    
    [HttpGet]
    [Route("view-battle")]
    [Produces<BattleView>]
    public IActionResult ViewBattle(string battleId)
    {
        // this should be protected by a viewer ID so clients don't use this info
        var battle = battleService.Get(battleId);
        if (battle is null)
        {
            return new NotFoundResult();
        }

        var result = new BattleView()
        {
            BattleField = new BattleRenderer(battle).RenderBattleField(renderPlayers: true),
            Width = battle.BattleField.Width,
            Height = battle.BattleField.Height,
            Players = battle.Players.ToList(),
        };
        
        return new OkObjectResult(result);
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
        return await ExecuteAction(new GetVisualAction(playerHeaders.Token), playerHeaders.Token);
    }
    
    /// <summary>
    /// Fires the gun!
    /// </summary>
    /// <param name="playerHeaders">The headers.</param>
    /// <returns></returns>
    [HttpGet]
    [Route("fire")]
    public async Task<IActionResult> Fire([FromQuery] PlayerHeaders playerHeaders)
    {
        return await ExecuteAction(new FireAction(playerHeaders.Token), playerHeaders.Token);
    }


    [HttpGet]
    [Route("move/{direction}")]
    public async Task<IActionResult> Move([FromQuery] PlayerHeaders playerHeaders, MoveDirection direction)
    {
        return await ExecuteAction(new MoveAction(playerHeaders.Token, direction), playerHeaders.Token);
    }
    
    [HttpGet]
    [Route("aim/{deltaangle}")]
    public async Task<IActionResult> Aim([FromQuery] PlayerHeaders playerHeaders, int deltaangle)
    {
        return await ExecuteAction(new AimAction(playerHeaders.Token, deltaangle), playerHeaders.Token);
    }
    
    /// <summary>
    /// Gets the readings from the instruments in your tank.
    /// </summary>
    /// <param name="playerHeaders">The headers.</param>
    /// <returns></returns>
    [HttpGet]
    [Route("get-readings")]
    public async Task<IActionResult> GetReadings([FromQuery] PlayerHeaders playerHeaders)
    {
        return await ExecuteAction(new GetReadingsAction(playerHeaders.Token), playerHeaders.Token);
    }

    private async Task<IActionResult> ExecuteAction<T>(T action, string playerToken) where T : ActionBase
    {
        if (action == null) throw new ArgumentNullException(nameof(action));
        if (playerToken == null) throw new ArgumentNullException(nameof(playerToken));
        if (fpsController.State == FpsControllerState.Paused) fpsController.Resume();

        if (!battleService.RequestActionLock(playerToken))
        {
            return new BadRequestObjectResult("You already have a pending action. Please try again later!");
        }
        
        try
        {
            // The get the battle and player
            var (battle, player) = battleService.GetBattleAndPlayerByPlayerToken(playerToken);
            if (battle is null) return new BadRequestObjectResult("Could not find a battle for the given token...");
            if (player is null)  return new BadRequestObjectResult("Could not find a player for the given token...");
            
            battleService.ServerLog.Log($"Enqueueing '{action.GetType().Name}' Action for player '{playerToken}'");
            var result = await battle.EnqueueAndWait(action);
            if (result.Result == null)
            {
                return new BadRequestResult();
            }
            
            battleService.ServerLog.Log($"Completed '{action.GetType().Name}' Action for player '{playerToken}', waiting the specified time of {result.Result.ExecutionDuration} ms");
            await Task.Delay(result.Result.ExecutionDuration);

            battleService.ServerLog.Log($"Returning result for '{action.GetType().Name}' to player '{playerToken}'");
            return new OkObjectResult(result.Result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        finally
        {
            battleService.ReleaseActionLock(playerToken);
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

public class BattleView
{
    public string BattleField { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public List<Player> Players { get; set; }
}