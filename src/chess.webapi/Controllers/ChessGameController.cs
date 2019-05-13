using chess.engine.Game;
using chess.webapi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace chess.webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChessGameController : Controller
    {
        private readonly IChessService _chessService;
        private ILogger<ChessGameController> _logger;

        public ChessGameController(
            ILogger<ChessGameController> logger,
            IChessService chessService
            )
        {
            _logger = logger;
            _chessService = chessService;
        }

        // GET

        public ActionResult<string> Index()
        {
            var chessGameResult = _chessService.GetNewBoard();
            return Json(chessGameResult);
        }

        [HttpGet("{board}/{move}")]
        public ActionResult<string> Move(string board, string move)
        {
            return Json(_chessService.PlayMove(board, move));
        }

        [HttpGet("{board}")]
        [HttpGet("{board}/moves")]
        public ActionResult<string> GetMoves(string board)
        {
            return Json(_chessService.GetMoves(board));
        }

        [HttpGet("{board}/moves/white")]
        public ActionResult<string> GetWhiteMoves(string board)
        {
            return Json(GetMovesForPlayer(board, Colours.White));
        }
        [HttpGet("{board}/moves/black")]
        public ActionResult<string> GetBlackMoves(string board)
        {
            return Json(GetMovesForPlayer(board, Colours.Black));
        }
        private ActionResult<string> GetMovesForPlayer(string board, Colours forPlayer)
        {
            return Json(_chessService.GetMovesForPlayer(board, forPlayer));
        }

        [HttpGet("{board}/moves/{location}")]
        public ActionResult<string> GetMoves(string board, string location)
        {
            return Json(_chessService.GetMovesForLocation(board, location));
        }


    }
}