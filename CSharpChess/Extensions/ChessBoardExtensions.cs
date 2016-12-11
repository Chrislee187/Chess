using System.Collections.Generic;
using CSharpChess.TheBoard;

namespace CSharpChess.Extensions
{
    public static class ChessBoardExtensions
    {
        public static IEnumerable<IEnumerable<BoardPiece>> Ranks(this ChessBoard board)
        {
            foreach (var rank in Chess.Board.Ranks)
            {
                var list = new List<BoardPiece>();
                foreach (var chessFile in Chess.Board.Files)
                {
                    list.Add(board[BoardLocation.At(chessFile, rank)]);
                }
                yield return list;
            }
        }
        public static IEnumerable<IEnumerable<BoardPiece>> Files(this ChessBoard board)
        {
            foreach (var chessFile in Chess.Board.Files)
            {
                var list = new List<BoardPiece>();
                foreach (var rank in Chess.Board.Ranks)
                {
                    list.Add(board[BoardLocation.At(chessFile, rank)]);
                }
                yield return list;
            }
        }
    }
}