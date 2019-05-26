using chess.engine.Game;
using chess.webapi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace chess.webapi.Controllers
{
    [Route("api/pgn")]
    [ApiController]
    public class PgnApiController : Controller
    {
        private ILogger<ChessGameApiController> _logger;


        public PgnApiController(
            ILogger<ChessGameApiController> logger,
            IChessService chessService
            )
        {
            _logger = logger;

        }

        // GET

        public ActionResult<string> Index()
        {
            return View();
        }
    }
}