using chess.engine.Game;
using chess.webapi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace chess.webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChessController : Controller
    {
        private readonly IChessService _chessService;
        private ILogger<ChessController> _logger;

        public ChessController(
            ILogger<ChessController> logger,
            IChessService chessService
            )
        {
            _logger = logger;
            _chessService = chessService;
        }

        // GET
        public ActionResult<string> Index()
        {
            return _chessService.GetNewBoard();
        }

        [HttpGet("{board}/{move}")]
        public ActionResult<string> Move(string board, string move)
        {
            return _chessService.PlayMove(board, move);
        }
        [HttpGet("{board}")]
        [HttpGet("{board}/moves")]
        public ActionResult<string> GetMoves(string board)
        {
            return _chessService.GetMoves(board);
        }

        [HttpGet("{board}/moves/white")]
        public ActionResult<string> GetWhiteMoves(string board)
        {
            return GetMovesForPlayer(board, Colours.White);
        }
        [HttpGet("{board}/moves/black")]
        public ActionResult<string> GetBlackMoves(string board)
        {
            return GetMovesForPlayer(board, Colours.Black);
        }
        private ActionResult<string> GetMovesForPlayer(string board, Colours forPlayer)
        {
            return _chessService.GetMovesForPlayer(board, forPlayer);
        }

        [HttpGet("{board}/moves/{location}")]
        public ActionResult<string> GetMoves(string board, string location)
        {
            return _chessService.GetMovesForLocation(board, location);
        }


    }
}