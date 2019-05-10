using System;
using System.Linq;
using board.engine;
using board.engine.Board;
using board.engine.Movement;
using chess.engine.Chess.Entities;
using chess.engine.Chess.Pieces;
using chess.engine.Extensions;
using chess.engine.Game;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;


namespace chess.engine.Chess
{
    public class ChessGame
    {
        private readonly BoardEngine<ChessPieceEntity> _engine;

        public Colours CurrentPlayer { get; private set; }
        private Colours NextPlayer() => CurrentPlayer == Colours.White ? Colours.Black : Colours.White;

        public GameState GameState { get; private set; }
        public bool InProgress => GameState == GameState.InProgress;

        public LocatedItem<ChessPieceEntity>[,] Board => _engine.Board;

        public IBoardState<ChessPieceEntity> BoardState => _engine.BoardState;

        private readonly ILogger<ChessGame> _logger;
        private readonly IBoardEntityFactory<ChessPieceEntity> _entityFactory;
        private readonly IChessGameStateService _gameStateService;

        public ChessGame(
            ILogger<ChessGame> logger,
            IBoardEngineProvider<ChessPieceEntity> boardEngineProvider,
            IBoardEntityFactory<ChessPieceEntity> entityFactory,
            IChessGameStateService gameStateService
        )
            : this(logger, boardEngineProvider, entityFactory, gameStateService, new ChessBoardSetup(entityFactory))
        {
        }

        public ChessGame(
            ILogger<ChessGame> logger,
            IBoardEngineProvider<ChessPieceEntity> boardEngineProvider,
            IBoardEntityFactory<ChessPieceEntity> entityFactory,
            IChessGameStateService gameStateService,
            IBoardSetup<ChessPieceEntity> setup,
            Colours whoseTurn = Colours.White)
        {

            _logger = logger;
            _logger.LogDebug("Initialising new chess game");

            _engine = boardEngineProvider.Provide(setup);
            _entityFactory = entityFactory;
            _gameStateService = gameStateService;
            CurrentPlayer = whoseTurn;
            GameState = _gameStateService.CurrentGameState(BoardState, CurrentPlayer);
        }

        public string Move(string input)
        {
            _logger.LogDebug($"Attempting move {input}");
            // TODO: Unit test this?
            var validated = ValidateInput(input);

            if (!String.IsNullOrEmpty(validated.errorMessage))
            {
                var from = input.Substring(0, 2).ToBoardLocation();
                var items = BoardState.GetItem(from);
                var moves = items.Paths.FlattenMoves().Select(m => m.ToString()).ToList();
                var debug = $"{this.ToText()}\nValid move list for piece at {from};\n" + String.Join(", ", moves);

                return validated.errorMessage + $"\n\nDEBUG INFO\n{debug}";
            }

            _engine.Move(validated.move);

            CurrentPlayer = NextPlayer();
            GameState = _gameStateService.CurrentGameState(BoardState, CurrentPlayer);

            return GameState == GameState.Check || GameState == GameState.Checkmate
                ? GameState.ToString()
                : "";
        }

        private (BoardMove move, string errorMessage) ValidateInput(string input)
        {
            var from = input.Substring(0, 2).ToBoardLocation();
            var to = input.Substring(2, 2).ToBoardLocation();
            ChessPieceEntity promotionPiece = null;
            var piece = _engine.PieceAt(from);
            var pieceColour = piece.Item.Player;

            if (input.Length == 6)
            {
                var extra = input.Substring(4, 2).ToList();

                if (extra[0] == '+')
                {
                    var extraData = new ChessPieceEntityFactory.ChessPieceEntityFactoryTypeData
                    {
                        Owner = pieceColour,
                        PieceName = PieceNameMapper.FromChar(extra[1])
                    };
                    promotionPiece = _entityFactory.Create(extraData);
                }
                else
                {
                    return (null, $"'{extra}' is not a valid promotion");
                }

            }

            if (pieceColour != CurrentPlayer)
            {
                return (null, $"It is not {pieceColour}'s turn.");
            }

            var validMove = piece.Paths.FindValidMove(from, to, promotionPiece);

            if (validMove == null)
            {
                return (null, $"{input} is not a valid move!");
            }

            if (_engine.PieceAt(validMove.To)?.Item.Piece == ChessPieceName.King)
            {
                return (null, $"Cannot take the king");
            }

            return (validMove, String.Empty);
        }



        #region Meta Info

        public static int DirectionModifierFor(Colours player) => player == Colours.White ? +1 : -1;
        public static int EndRankFor(Colours colour) => colour == Colours.White ? 8 : 1;

        #endregion
    }

    public enum GameState
    {
        InProgress,
        Check,
        Checkmate
    }
}