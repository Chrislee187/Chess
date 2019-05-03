using chess.engine.Board;

namespace chess.engine.Movement.SimpleValidators
{
    public class DestinationIsEmptyOrContainsEnemyValidator : IMoveValidator
    {
        public bool ValidateMove(ChessMove move, IBoardState boardState)
            => new DestinationIsEmptyValidator().ValidateMove(move, boardState) 
               || new DestinationContainsEnemyMoveValidator().ValidateMove(move, boardState);

    }
}