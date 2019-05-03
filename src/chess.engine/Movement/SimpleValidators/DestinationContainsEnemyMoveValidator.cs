using System.Linq;
using chess.engine.Board;

namespace chess.engine.Movement.SimpleValidators
{
    public class DestinationContainsEnemyMoveValidator : IMoveValidator
    {
        public bool ValidateMove(ChessMove move, IBoardState boardState)
        {
            var sourcePiece = boardState.GetItems(move.From).SingleOrDefault();
            Guard.NotNull(sourcePiece);
            if (!boardState.IsEmpty(move.To))
            {
                var destinationPiece = boardState.GetItems(move.To).Single();
                return sourcePiece.Item.Player != destinationPiece.Item.Player;
            }

            return false;
        }
    }
}