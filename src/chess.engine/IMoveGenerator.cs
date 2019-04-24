using System.Collections.Generic;

namespace chess.engine
{
    public interface IMoveGenerator
    {
        IEnumerable<Move> MovesFrom(BoardLocation location, Colours playerToMove);
        IEnumerable<Move> MovesFrom(string location, Colours playerToMove);
    }
}