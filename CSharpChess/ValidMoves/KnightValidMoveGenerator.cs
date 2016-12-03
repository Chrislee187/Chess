using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using CSharpChess.TheBoard;

namespace CSharpChess.ValidMoves
{
    public class KnightValidMoveGenerator
    {
        public IEnumerable<ChessMove> For(ChessBoard board, string location)
        {
            return For(board, (BoardLocation) location);
        }

        public IEnumerable<ChessMove> For(ChessBoard board, BoardLocation at)
        {
            var possibleMoves = new List<ChessMove>();

            possibleMoves.AddRange(Moves(board, at));

            possibleMoves.AddRange(Takes(board, at));

            return possibleMoves;
        }

        private IEnumerable<ChessMove> Moves(ChessBoard board, BoardLocation at)
        {
            var chessPiece = board[at].Piece;
            if(chessPiece.Colour == Chess.Colours.None) return new List<ChessMove>();

            var possibleMoves = Chess.Rules.Knights.MovesFrom(at);

            Func<BoardLocation, bool> locationIsValidToMoveTo = board.IsEmptyAt;

            return possibleMoves
                .Where(locationIsValidToMoveTo)
                .Select(m => new ChessMove(at, m, MoveType.Move));
        }

        private IEnumerable<ChessMove> Takes(ChessBoard board, BoardLocation at)
        {
            var chessPiece = board[at].Piece;
            if (chessPiece.Colour == Chess.Colours.None) return new List<ChessMove>();

            var possibleMoves = Chess.Rules.Knights.MovesFrom(at);

            Chess.Colours enemyColour = Chess.EnemyColour(chessPiece.Colour);

            Func<BoardLocation, bool> locationIsValidToTake = m => board.IsNotEmptyAt(m) && board[m].Piece.Colour == enemyColour;

            return possibleMoves
                .Where(locationIsValidToTake)
                .Select(m => new ChessMove(at, m, MoveType.Take));
        }

        private MoveType Promotable(BoardLocation location, Chess.Colours colour, MoveType dflt)
        {
            var promotionRank = Chess.PromotionRankFor(colour);
            return location.Rank == promotionRank ? MoveType.Promotion : dflt;
        }
        private IEnumerable<BoardLocation> NormalTakes(ChessBoard board, BoardLocation at, int horizontal)
        {
            var vertical = Chess.Pieces.Direction(board[at].Piece);

            var pieceColour = board[at].Piece.Colour;
            var notOnHorizontalEdge = horizontal > 0 
                ? at.File < Chess.ChessFile.H 
                : at.File > Chess.ChessFile.A;

            var moveTos = new List<BoardLocation>();

            if (!notOnHorizontalEdge) return moveTos;

            var newFile = (int)at.File + horizontal;
            var newRank = at.Rank + vertical;
            var takeLocation = BoardLocation.At(newFile, newRank);

            if (CanTakeAt(board, takeLocation, pieceColour))
            {
                moveTos.Add(takeLocation);
            }

            return moveTos;
        }

        private IEnumerable<BoardLocation> EnPassantTakes(ChessBoard board, BoardLocation at, int horizontal)
        {
            var vertical = Chess.Pieces.Direction(board[at].Piece);

            var moveTos = new List<BoardLocation>();

            var enpassantFromRank = Chess.Pieces.EnpassantFromRankFor(board[at].Piece.Colour);

            if (at.Rank == enpassantFromRank)
            {
                var notOnHorizontalEdge = horizontal > 0
                    ? at.File < Chess.ChessFile.H
                    : at.File > Chess.ChessFile.A;

                if (notOnHorizontalEdge)
                {
                    var newFile = (int)at.File + horizontal;
                    var enPassantLocation = new BoardLocation((Chess.ChessFile)newFile, at.Rank + vertical);

                    if (board.CanEnPassant(at, enPassantLocation))
                    {
                        moveTos.Add(enPassantLocation);
                    }
                }
            }
            
            return moveTos;
        }

        private static bool CanTakeAt(ChessBoard board, BoardLocation takeLocation, Chess.Colours colourOfTakingPiece) 
            => !board.IsEmptyAt(takeLocation) && board[takeLocation].Piece.Colour != colourOfTakingPiece;


        private static bool Blocked(ChessBoard board, ChessMove chessMove)
        {
            return !board[chessMove.To].Piece.Equals(ChessPiece.NullPiece);
        }
    }
}