using System.Collections.Generic;
using Chess.Common.Extensions;

namespace Chess.Common.Movement
{
    public interface IMoveGenerator
    {
        IEnumerable<Move> All(Common.Board board, BoardLocation at);
    }
}