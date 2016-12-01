using System.Collections.Generic;
using CSharpChess.TheBoard;

namespace CSharpChess.ValidMoves
{
    public class PawnValidMoveGenerator
    {
        public IEnumerable<ChessMove> For(ChessBoard board, BoardLocation at)
        {
            var possibleMoves = new List<ChessMove>();

            possibleMoves.AddRange(ForwardMoves(board, at));

            possibleMoves.AddRange(EnPassantMoves(board, at));

            return possibleMoves;
        }

        private IEnumerable<ChessMove> ForwardMoves(ChessBoard board, BoardLocation at)
        {
            if(board[at].Piece.Colour == Chess.Colours.None) return new List<ChessMove>();

            var direction = GetDirection(board[at]);


            var newMove = new ChessMove(at, new BoardLocation(at.File, at.Rank + direction));
            var validMoves = new List<ChessMove>();
            if (!Blocked(board, newMove))
            {
                validMoves.Add(newMove);

                newMove = new ChessMove(at, new BoardLocation(at.File, at.Rank + (direction * 2)));
                if (!Blocked(board, newMove))
                    validMoves.Add(newMove);
            }

            return validMoves;
        }

        private static int GetDirection(BoardPiece boardPiece)
        {
            return boardPiece.Piece.Colour == Chess.Colours.White ? +1 : -1;
        }

        private IEnumerable<ChessMove> EnPassantMoves(ChessBoard board, BoardLocation from)
        {
            return new List<ChessMove>();
        }

        private bool Blocked(ChessBoard board, ChessMove chessMove)
        {
            return !board[chessMove.To].Piece.Equals(ChessPiece.NullPiece);
        }
    }
}