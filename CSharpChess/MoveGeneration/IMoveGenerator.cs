using System.Collections.Generic;
using CSharpChess.TheBoard;

namespace CSharpChess.MoveGeneration
{
    public interface IMoveGenerator
    {
        IEnumerable<ChessMove> All(ChessBoard board, BoardLocation at);
    }
}