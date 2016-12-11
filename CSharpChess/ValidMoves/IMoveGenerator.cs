using System.Collections.Generic;
using CSharpChess.TheBoard;

namespace CSharpChess.ValidMoves
{
    public interface IMoveGenerator
    {
        IEnumerable<ChessMove> All(ChessBoard board, BoardLocation at);
        IEnumerable<ChessMove> Moves(ChessBoard board, BoardLocation at);
        IEnumerable<ChessMove> Takes(ChessBoard board, BoardLocation at);
        IEnumerable<ChessMove> Covers(ChessBoard board, BoardLocation at);
        void RecalcAll();
    }
}