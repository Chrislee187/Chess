using System.Linq;
using chess.engine.Board;
using chess.engine.Movement;

namespace chess.engine.Game
{
    public class ChessGame
    {
        private readonly ChessBoardEngine _engine = new ChessBoardEngine().InitBoard();
        public Colours CurrentPlayer { get; private set; } = Colours.White;
        public bool InProgress = true;


        public BoardPiece[,] Board => _engine.Board;

        public string Move(string input)
        {
            var from = BoardLocation.At(input.Substring(0, 2));
            var to = BoardLocation.At(input.Substring(2, 2));

            var validated = ValidateInput(input, from, to);

            if (!string.IsNullOrEmpty(validated.errorMessage))
            {
                return validated.errorMessage;
            }

            // execute board action for move type
            _engine.Move(validated.move);

            CurrentPlayer = CurrentPlayer == Colours.White ? Colours.Black : Colours.White;

            return "";
        }

        private (ChessMove move, string errorMessage) ValidateInput(string input, BoardLocation @from, BoardLocation to)
        {
            var pieceColour = _engine.PieceAt(@from)?.Entity.Player;
            if (pieceColour.HasValue && pieceColour.Value != CurrentPlayer)
            {
                return (null, $"It is not {pieceColour.Value}'s turn.");
            }

            var validMove = _engine.PieceAt(@from)?
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