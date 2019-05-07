using System.Linq;
using chess.engine.Board;
using chess.engine.Chess.Pieces;
using chess.engine.Entities;
using chess.engine.Game;
using chess.engine.Movement;

namespace chess.engine.Chess
{
    public enum GameState
    {
        InProgress, Check, Checkmate
    }

    public class ChessGame
    {
        private readonly BoardEngine<ChessPieceEntity> _engine;

        public Colours CurrentPlayer { get; private set; }
        private Colours NextPlayer() => CurrentPlayer == Colours.White ? Colours.Black : Colours.White;

        public GameState GameState { get; private set; }
        public bool InProgress => GameState == GameState.InProgress;

        public LocatedItem<ChessPieceEntity>[,] Board => _engine.Board;

        public IBoardState<ChessPieceEntity> BoardState => _engine.BoardState;

        public ChessGame() : this(new ChessBoardSetup())
        { }

        public ChessGame(IGameSetup<ChessPieceEntity> setup, Colours whoseTurn = Colours.White)
        {
            _engine = new BoardEngine<ChessPieceEntity>(setup, 
                new ChessPathsValidator(new ChessPathValidator(new MoveValidationFactory<ChessPieceEntity>())),
                new ChessRefreshAllPaths());

            CurrentPlayer = whoseTurn;
        }
        
        public string Move(string input)
        {
            var validated = ValidateInput(input);

            if (!string.IsNullOrEmpty(validated.errorMessage))
            {
                var from = BoardLocation.At(input.Substring(0, 2));
                var items = BoardState.GetItem(from);
                var moves = items.Paths.FlattenMoves().Select(m => m.ToString()).ToList();
                var debug = $"{this.ToText()}\nValid move list for piece at {from};\n" + string.Join(", ", moves);

                return validated.errorMessage + $"\n\nDEBUG INFO\n{debug}";
            }

            _engine.Move(validated.move);

            CurrentPlayer = NextPlayer();
            GameState = BoardState.CurrentGameState(CurrentPlayer, CurrentPlayer.Enemy());

            return GameState == GameState.Check || GameState == GameState.Checkmate 
                ? GameState.ToString() 
                : "";
        }

        private (BoardMove move, string errorMessage) ValidateInput(string input)
        {
//            if (input == "f1b5") Debugger.Break();

            var from = BoardLocation.At(input.Substring(0, 2));
            var to = BoardLocation.At(input.Substring(2, 2));
            ChessPieceName? promotionPiece = null;
            if (input.Length == 6)
            {
                var extra = input.Substring(4, 2).ToList();

                if (extra[0] == '+')
                {
                    promotionPiece = PieceNameMapper.FromChar(extra[1]);
                }
                else
                {
                    return (null, $"'{extra}' is not a valid promotion");
                }

            }
            var piece = _engine.PieceAt(from);
            var pieceColour = piece.Item.Owner;
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
            return (validMove, string.Empty);
        }
        
        #region Meta Info
        public static int EndRankFor(Colours colour) => colour == Colours.White ? 8 : 1;
        public static int EndRankFor(int colour) => EndRankFor((Colours)colour);
        #endregion
    }
}