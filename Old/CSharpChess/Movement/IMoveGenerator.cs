using System.Collections.Generic;
using CSharpChess.Extensions;

namespace CSharpChess.Movement
{
    public interface IMoveGenerator
    {
        IEnumerable<Move> All(CSharpChess.Board board, BoardLocation at);
    }
}