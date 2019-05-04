using chess.engine.Board;

namespace chess.engine.Movement
{
    public interface IPathValidator
    {
        Path ValidatePath(Path possiblePath, IBoardState boardState);
    }
}