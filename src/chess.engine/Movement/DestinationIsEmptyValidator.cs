using chess.engine.Board;

namespace chess.engine.Movement
{
    public class DestinationIsEmptyValidator : IMoveValidator
    {
        public bool ValidateMove(ChessMove move, BoardState boardState)
            => !boardState.Entities.ContainsKey(move.To) || boardState.Entities[move.To] == null;

    }
}