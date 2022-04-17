using Microsoft.AspNetCore.Mvc;
using Robobobot.Core;
using Robobobot.Core.Models;
using Robobobot.Server.BackgroundServices;
namespace Robobobot.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AdminController : ControllerBase
{
    private readonly IFpsController fpsController;
    private readonly BattleService battleService;

    public AdminController(IFpsController fpsController, BattleService battleService)
    {
        this.fpsController = fpsController;
        this.battleService = battleService;
    }

    [HttpGet]
    [Route("pause-server")]
    public IActionResult PauseServer()
    {
        fpsController.Pause();
        battleService.ServerLog.Log($"Pausing server awaiting manual start or new battle initiation");
        return Ok();
    }
    
    [HttpGet]
    [Route("resume-server")]
    public IActionResult ResumeServer()
    {
        fpsController.Resume();
        battleService.ServerLog.Log($"Resuming server with game loop running at {fpsController.Fps} fps.");
        return Ok();
    }
    
    [HttpGet]
    [Route("stats")]
    public IActionResult Stats()
    {
        var stats = new ServerStats(fpsController.State.ToString(), fpsController.Fps);
        return new OkObjectResult(stats);
    }
    
    private record ServerStats(string State, int Fps);
    
    [HttpGet]
    [Route("set-fps/{fps}")]
    public IActionResult SetFps(int fps)
    {
        fpsController.Fps = fps;
        battleService.ServerLog.Log($"Setting fps to {fps}");
        return Ok();
    }

    /// <summary>
    /// View the server log
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("view-log")]
    [Produces("text/plain")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public IActionResult ViewLog()
    {
        var log = string.Join(Environment.NewLine, battleService.ServerLog.GetLast(1000));
        return new OkObjectResult(log);
    }

    [HttpGet]
    [Route("move-player")]
    public IActionResult MovePlayer(string playerToken, int x, int y)
    {
        battleService.GetPlayerByToken(playerToken)!.Location = Location.Create(x, y);
        return new OkResult();
    }
}

