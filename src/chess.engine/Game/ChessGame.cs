using System.Diagnostics;
using board.engine;
using board.engine.Board;
using board.engine.Movement;
using chess.engine.Entities;
using chess.engine.SAN;
using Microsoft.Extensions.Logging;

namespace chess.engine.Game
{
    public class ChessGame
    {
        private readonly ILogger<ChessGame> _logger;
        private readonly BoardEngine<ChessPieceEntity> _engine;
        private readonly ICheckDetectionService _checkDetectionService;

        private SanMoveFinder _sanMoveFinder;

        public static bool OutOfBounds(int value) => value < 1 || value > 8;
        public Colours CurrentPlayer { get; private set; }
        public GameCheckState CheckState { get; private set; }
        public bool InProgress => CheckState == GameCheckState.None;
        public LocatedItem<ChessPieceEntity>[,] Board => _engine.Board;

        public IBoardState<ChessPieceEntity> BoardState => _engine.BoardState;

        public ChessGame(
            ILogger<ChessGame> logger,
            IBoardEngineProvider<ChessPieceEntity> boardEngineProvider,
            IBoardEntityFactory<ChessPieceEntity> entityFactory,
            ICheckDetectionService checkDetectionService
        )
            : this(logger, boardEngineProvider, checkDetectionService, new ChessBoardSetup(entityFactory))
        {
        }

        public ChessGame(
            ILogger<ChessGame> logger,
            IBoardEngineProvider<ChessPieceEntity> boardEngineProvider,
            ICheckDetectionService checkDetectionService,
            IBoardSetup<ChessPieceEntity> setup,
            Colours whoseTurn = Colours.White)
        {

            _logger = logger;
            _logger?.LogInformation("Initialising new chess game");

            _engine = boardEngineProvider.Provide(setup);

            _checkDetectionService = checkDetectionService;
            CurrentPlayer = whoseTurn;
            CheckState = _checkDetectionService.Check(BoardState);
        }

        public string Move(string input)
        {
            _logger?.LogDebug($"Attempting move {input}");

            if (!StandardAlgebraicNotation.TryParse(input, out var san))
            {
                // TODO: More detailed error
                return $"Error: invalid move {input}, are you using upper-case for Files?";
            }


            _sanMoveFinder = new SanMoveFinder(_engine.BoardState);

            var move = _sanMoveFinder.Find(san, CurrentPlayer);

            if (move == null)
            {
                return $"Error: No matching move found: {input}";
            }

            return PlayValidMove(move);

        }

        private string PlayValidMove(BoardMove move)
        {
            var preMove = _engine.BoardState.ToTextBoard();
            _engine.Move(move);

            if (preMove == _engine.BoardState.ToTextBoard())
            {
                Debugger.Break();
            }

            CurrentPlayer = NextPlayer();
            CheckState = _checkDetectionService.Check(BoardState);

            return CheckState != GameCheckState.None
                ? CheckState.ToString()
                : "";

        }

        private Colours NextPlayer() => CurrentPlayer == Colours.White ? Colours.Black : Colours.White;

        #region Meta Info

        public static int DirectionModifierFor(Colours player) => player == Colours.White ? +1 : -1;
        public static int EndRankFor(Colours colour) => colour == Colours.White ? 8 : 1;

        #endregion
    }

    public enum PlayerState
    {
        None,
        Check,
        Checkmate
    }
}