using board.engine.Board;

namespace board.engine.Movement
{
    public interface IPathGenerator
    {
        Paths PathsFrom(BoardLocation location, int forPlayer);
    }
}