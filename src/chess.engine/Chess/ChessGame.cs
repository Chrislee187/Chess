using System.Linq;
using chess.engine.Board;
using chess.engine.Game;
using chess.engine.Movement;

namespace chess.engine.Chess
{
    public class ChessGame
    {
        private readonly ChessBoardEngine _engine;
        public Colours CurrentPlayer { get; private set; } = Colours.White;
        public bool InProgress = true;

        public BoardPiece[,] Board => _engine.Board;

        public ChessGame() : this(new ChessBoardSetup())
        { }

        public ChessGame(IGameSetup setup)
        {
            _engine = new ChessBoardEngine(setup, new ChessMoveValidator(new MoveValidationFactory()), new ChessRefreshAllPaths());
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

            return "";
        }

        private Colours NextPlayer()
        {
            return CurrentPlayer == Colours.White ? Colours.Black : Colours.White;
        }

        private (ChessMove move, string errorMessage) ValidateInput(string input)
        {
            var from = BoardLocation.At(input.Substring(0, 2));
            var to = BoardLocation.At(input.Substring(2, 2));

            var piece = _engine.PieceAt(from);
            var pieceColour = piece?.Entity.Player;
            if (pieceColour.HasValue && pieceColour.Value != CurrentPlayer)
            {
                return (null, $"It is not {pieceColour.Value}'s turn.");
            }

            var validMove = piece?
                .Paths.SelectMany(path => path)
                .SingleOrDefault(move => move.To.Equals(to));

            if (validMove == null)
            {
                return (null, $"{input} is not a valid move!");
            }

            return (validMove, string.Empty);
        }


    }
}