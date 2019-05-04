using System.Linq;
using chess.engine.Actions;
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
        public static int EndRankFor(Colours colour) => colour == Colours.White ? 8 : 1;
        public static int EndRankFor(int colour) => EndRankFor((Colours) colour);
        private readonly ChessBoardEngine<ChessPieceEntity> _engine;
        public Colours CurrentPlayer { get; private set; } = Colours.White;
        private Colours NextPlayer() => CurrentPlayer == Colours.White ? Colours.Black : Colours.White;

        public bool InProgress = true;

        public BoardPiece[,] Board => _engine.Board;
        public IBoardState<ChessPieceEntity> BoardState => _engine.BoardState;

        public GameState GameState { get; private set; }
        public ChessGame() : this(new ChessBoardSetup())
        { }

        public ChessGame(IGameSetup<ChessPieceEntity> setup)
        {
            _engine = new ChessBoardEngine<ChessPieceEntity>(setup, 
                new ChessPathsValidator(new ChessPathValidator(new MoveValidationFactory<ChessPieceEntity>()), new BoardActionFactory<ChessPieceEntity>()),
                new ChessRefreshAllPaths());
        }
        
        public string Move(string input)
        {
            var validated = ValidateInput(input);

            if (!string.IsNullOrEmpty(validated.errorMessage))
            {
                return validated.errorMessage;
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

            var validMove = piece.Paths.FindValidMove(to, promotionPiece);

            if (validMove == null)
            {
                return (null, $"{input} is not a valid move!");
            }

            return (validMove, string.Empty);
        }


    }
}