using Microsoft.AspNetCore.Mvc;
namespace Robobobot.Server.Controllers;

[ApiController]
[Route("")]
public class DefaultController : ControllerBase
{
   /// <summary>
   /// Redirects any lost soul to the swagger UI.
   /// </summary>
   /// <returns>A 302 not found with a redirect.</returns>
   [HttpGet]
   public IActionResult Default()
   {
      return Redirect("/swagger");
   }
}