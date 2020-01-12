using Microsoft.AspNetCore.Mvc;

namespace CodeWords.Controllers.MvcControllers
{
    public class GameController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult WordKey()
        {
            return View("WordKey");
        }
    }
}