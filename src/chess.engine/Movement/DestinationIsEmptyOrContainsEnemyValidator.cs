using chess.engine.Board;

namespace chess.engine.Movement
{
    public class DestinationIsEmptyOrContainsEnemyValidator : IMoveValidator
    {
        public bool ValidateMove(ChessMove move, BoardState boardState)
            => new DestinationIsEmptyValidator().ValidateMove(move, boardState) 
               || new DestinationContainsEnemyMoveValidator().ValidateMove(move, boardState);

    }
}