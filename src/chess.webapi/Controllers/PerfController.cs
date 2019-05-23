using chess.webapi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace chess.webapi.Controllers
{
    [Route("[controller]")]
    public class PerfController : Controller
    {
        private ILogger<PerfController> _logger;
        private readonly IPerfService _perfService;

        public PerfController(
            ILogger<PerfController> logger,
            IPerfService perfService
        )
        {
            _logger = logger;
            _perfService = perfService;
        }

        [HttpGet]
        // GET
        public IActionResult Index()
        {
            return View("Index");
        }

        [HttpGet("playwikigame/{iterations}")]
        // GET /
        public IActionResult Index(int iterations)
        {

            return Json(_perfService.PlayWikiGame(iterations));
        }
    }
}