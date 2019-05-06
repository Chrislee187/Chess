using chess.engine.Game;

namespace chess.engine.Movement
{
    public interface IPathGenerator
    {
        Paths PathsFrom(BoardLocation location, Colours forPlayer);
        Paths PathsFrom(string location, Colours forPlayer);
    }
}