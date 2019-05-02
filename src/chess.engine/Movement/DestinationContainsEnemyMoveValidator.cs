using chess.engine.Board;

namespace chess.engine.Movement
{
    public class DestinationContainsEnemyMoveValidator : IMoveValidator
    {
        public bool ValidateMove(ChessMove move, BoardState boardState)
        {
            var sourcePiece = boardState.Entities[move.From];
            Guard.NotNull(sourcePiece);

            if (boardState.Entities.TryGetValue(move.To, out var destinationPiece))
            {
                return sourcePiece.Player != destinationPiece.Player;
            }

            return false;
        }
    }
}