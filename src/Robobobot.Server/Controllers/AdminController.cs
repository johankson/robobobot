using Microsoft.AspNetCore.Mvc;
using Robobobot.Server.BackgroundServices;
namespace Robobobot.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AdminController : ControllerBase
{
    private readonly IFpsController fpsController;

    public AdminController(IFpsController fpsController)
    {
        this.fpsController = fpsController;
    }

    [HttpGet]
    [Route("pause-server")]
    public IActionResult PauseServer()
    {
        fpsController.Pause();
        return Ok();
    }
    
    [HttpGet]
    [Route("resume-server")]
    public IActionResult ResumeServer()
    {
        fpsController.Resume();
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
        return Ok();
    }
}

