using Microsoft.AspNetCore.Mvc;
using Interfaces;

namespace Foresythe.Controllers
{
    [Route("api/v1/Home")]
    public class HomeController : Controller
    {
        private readonly ILoggerService _logger;

        public HomeController(ILoggerService logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        
    }
}
