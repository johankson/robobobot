using Microsoft.AspNetCore.Mvc;
namespace Robobobot.Server.Controllers.Web;

public class Default : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}