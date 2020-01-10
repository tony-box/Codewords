using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CodeWords.Models;

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
