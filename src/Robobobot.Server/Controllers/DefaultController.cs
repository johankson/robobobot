using Microsoft.AspNetCore.Mvc;
namespace Robobobot.Server.Controllers;

[ApiController]
[Route("")]
public class DefaultController : ControllerBase
{
   [HttpGet]
   public IActionResult Default()
   {
      return Redirect("/swagger");
   }
}