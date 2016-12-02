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
            var moveTos = new List<BoardLocation>();
            var pieceColour = board[at].Piece.Colour;

            var verticalDirectioModifier = pieceColour == Chess.Colours.White ? +1 : -1;
            const int leftDirectionModifier = -1;
            const int rightDirectionModifier = 1;

            Action<BoardLocation> addToList = (takeLocation) =>
            {
                if (CanTakeAt(board, takeLocation, pieceColour))
                    moveTos.Add(takeLocation);
            };
            CheckForNormalTake(at, rightDirectionModifier, verticalDirectioModifier, addToList);
            CheckForNormalTake(at, leftDirectionModifier, verticalDirectioModifier, addToList);

            // TODO: en passant, BoardPieces need a move history so we can check the previous move was a two-step
            return moveTos.Select(m => new ChessMove(at, m));
        }
        private static void CheckForNormalTake(BoardLocation from, int horizontalDirectionModifier, int verticalDirectionModifier, Action<BoardLocation> addToList)
        {
            var notOnHorizontalEdge = horizontalDirectionModifier > 0 
                ? from.File < Chess.ChessFile.H 
                : from.File > Chess.ChessFile.A;

            if (notOnHorizontalEdge)
            {
                var newFile = (int)from.File + horizontalDirectionModifier;
                var newRank = from.Rank + verticalDirectionModifier;
                addToList(BoardLocation.At(newFile, newRank));
            }
        }

        private static bool CanTakeAt(ChessBoard board, BoardLocation takeLocation, Chess.Colours colourOfTakingPiece) 
            => !board.IsEmptyAt(takeLocation) && board[takeLocation].Piece.Colour != colourOfTakingPiece;

        private static IEnumerable<ChessMove> ForwardMoves(ChessBoard board, BoardLocation at)
        {
            var chessPiece = board[at].Piece;
            if(chessPiece.Colour == Chess.Colours.None) return new List<ChessMove>();

            var direction = board[at].Piece.Colour == Chess.Colours.White ? +1 : -1;

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

        private static bool Blocked(ChessBoard board, ChessMove chessMove)
        {
            var chessPiece = board[chessMove.To].Piece;
            return !chessPiece.Equals(ChessPiece.NullPiece);
        }
    }
}