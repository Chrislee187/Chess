using System.Linq;
using chess.engine.Board;

namespace chess.engine.Movement.SimpleValidators
{
    public class DestinationContainsEnemyMoveValidator : IMoveValidator
    {
        public bool ValidateMove(ChessMove move, BoardState boardState)
        {
            var sourcePiece = boardState.Get(move.From).SingleOrDefault();
            Guard.NotNull(sourcePiece);
            if (!boardState.IsEmpty(move.To))
            {
                var destinationPiece = boardState.Get(move.To).Single();
                return sourcePiece.Item.Player != destinationPiece.Item.Player;
            }

            return false;
        }
    }
}