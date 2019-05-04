using chess.engine.Board;

namespace chess.engine.Movement
{
    public interface IMoveValidator
    {
        bool ValidateMove(BoardMove move, IBoardState boardState);
    }
}