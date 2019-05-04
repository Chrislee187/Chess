using chess.engine.Board;

namespace chess.engine.Movement.Validators
{
    public class DestinationIsEmptyValidator : IMoveValidator
    {
        public bool ValidateMove(BoardMove move, IBoardState boardState)
            => boardState.IsEmpty(move.To);

    }
}