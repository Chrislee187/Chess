using System;
using System.Collections.Generic;
using System.Linq;
using CSharpChess.TheBoard;

namespace CSharpChess.ValidMoves
{
    public class PawnValidMoveGenerator
    {
        public IEnumerable<ChessMove> For(ChessBoard board, BoardLocation at)
        {
            var possibleMoves = new List<ChessMove>();

            possibleMoves.AddRange(ForwardMoves(board, at));

            possibleMoves.AddRange(TakeMoves(board, at));

            return possibleMoves;
        }

        private IEnumerable<ChessMove> TakeMoves(ChessBoard board, BoardLocation at)
        {
            // Get piece NW & NE (SW/SW for black) of location
            // if it's an enemy piece add to list
            var direction = GetDirectionModifer(board[at]);
            var moveTos = new List<BoardLocation>();
            var pieceColour = board[at].Piece.Colour;

            const int left = -1;
            const int right = 1;

            Action<BoardLocation> AddToList = (takeLocation) =>
            {
                if (CanTakeAt(board, takeLocation, pieceColour))
                    moveTos.Add(takeLocation);
            };

            if (at.File > Chess.ChessFile.A)
            {
                var takeLocation = BoardLocation.At((int) at.File + left,at.Rank + (1 * direction));
                AddToList(takeLocation);
            }

            if (at.File < Chess.ChessFile.H)
            {
                var takeLocation = BoardLocation.At((int)at.File + right, at.Rank + (1 * direction));
                AddToList(takeLocation);
            }

            // TODO: en passant
            return moveTos.Select(m => new ChessMove(at, m));
        }

        private static bool CanTakeAt(ChessBoard board, BoardLocation takeLocation, Chess.Colours colour)
        {
            return !board.IsEmptyAt(takeLocation) && board[takeLocation].Piece.Colour != colour;
        }

        private static IEnumerable<ChessMove> ForwardMoves(ChessBoard board, BoardLocation at)
        {
            if(board[at].Piece.Colour == Chess.Colours.None) return new List<ChessMove>();

            var direction = GetDirectionModifer(board[at]);

            // TODO: Create a function that takes an order line of locations
            // and returns the index of the first blocker?
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

        private static int GetDirectionModifer(BoardPiece boardPiece)
        {
            return boardPiece.Piece.Colour == Chess.Colours.White ? +1 : -1;
        }

        private static IEnumerable<ChessMove> EnPassantMoves(ChessBoard board, BoardLocation from)
        {
            return new List<ChessMove>();
        }

        private static bool Blocked(ChessBoard board, ChessMove chessMove)
        {
            var chessPiece = board[chessMove.To].Piece;
            return !chessPiece.Equals(ChessPiece.NullPiece);
        }
    }
}