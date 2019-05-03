using chess.engine.Board;

namespace chess.engine.Movement
{
    public class DestinationIsEmptyValidator : IMoveValidator
    {
        public bool ValidateMove(ChessMove move, BoardState boardState)
            => boardState.IsEmpty(move.To);

    }
}