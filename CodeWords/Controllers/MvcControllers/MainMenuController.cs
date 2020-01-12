using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CodeWords.Controllers
{
    [Route("")]
    public class MainMenuController : Controller
    {
        private readonly ILogger<MainMenuController> _logger;

        public MainMenuController(ILogger<MainMenuController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }        
    }
}
