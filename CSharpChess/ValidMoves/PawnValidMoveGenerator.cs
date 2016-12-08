using System;
using System.Collections.Generic;
using System.Linq;
using CSharpChess.TheBoard;

namespace CSharpChess.ValidMoves
{
    public class PawnValidMoveGenerator : ValidMoveGeneratorBase
    {
        public PawnValidMoveGenerator() : base(Chess.PieceNames.Pawn)
        {}

        protected override IEnumerable<ChessMove> Takes(ChessBoard board, BoardLocation at)
        {
            var moves = new List<ChessMove>();
            var pieceColour = board[at].Piece.Colour;
            
            moves.AddRange(ValidCaptures(board, at)
                .Select(l => new ChessMove(at, l, PromotedTo(l, pieceColour, MoveType.Take)))
                );

            moves.AddRange(ValidEnPassants(board, at)
                .Select(l => new ChessMove(at, l, PromotedTo(l, pieceColour, MoveType.TakeEnPassant)))
                );

            return moves;
        }
        protected override IEnumerable<ChessMove> Moves(ChessBoard board, BoardLocation at)
        {
            var chessPiece = board[at].Piece;
            var direction = Chess.Pieces.VerticalDirectionModifierFor(chessPiece);
            var boardLocation = new BoardLocation(at.File, at.Rank + direction);
            var newMove = new ChessMove(at, boardLocation, PromotedTo(boardLocation, chessPiece.Colour, MoveType.Move));

            var validMoves = new List<ChessMove>();
            if (board.IsEmptyAt(newMove.To))
            {
                validMoves.Add(newMove);
                if (board[at].Location.Rank == Chess.StartingPawnRankFor(chessPiece.Colour) )
                {
                    var location = new BoardLocation(at.File, at.Rank + (direction * 2));
                    newMove = new ChessMove(at, location, PromotedTo(location, chessPiece.Colour, MoveType.Move));

                    if (board.IsEmptyAt(newMove.To))
                        validMoves.Add(newMove);
                }
            }

            return validMoves;
        }
        protected override IEnumerable<ChessMove> Covers(ChessBoard board, BoardLocation at)
        {
            var chessPiece = board[at].Piece;
            var direction = Chess.Pieces.VerticalDirectionModifierFor(chessPiece);
            var boardLocation = new BoardLocation(at.File, at.Rank + direction);
            var newMove = new ChessMove(at, boardLocation, PromotedTo(boardLocation, chessPiece.Colour, MoveType.Move));

            var validMoves = new List<ChessMove>();
            if (!board.IsEmptyAt(newMove.To) && board[newMove.To].Piece.Colour == chessPiece.Colour)
            {
                validMoves.Add(newMove);
                if (board[at].Location.Rank == Chess.StartingPawnRankFor(chessPiece.Colour))
                {
                    var location = new BoardLocation(at.File, at.Rank + (direction * 2));
                    newMove = new ChessMove(at, location, PromotedTo(location, chessPiece.Colour, MoveType.Move));

                    if (!board.IsEmptyAt(newMove.To) && board[newMove.To].Piece.Colour == chessPiece.Colour)
                        validMoves.Add(newMove);
                }
            }

            return validMoves;
        }
        protected override IEnumerable<BoardLocation> Threats(ChessBoard board, BoardLocation at)
        {
            var threats = new List<BoardLocation>();
            threats.AddRange(EnPassantLocations(board, at));
            threats.AddRange(CaptureLocations(board, at));
            return threats;
        }

        private static IEnumerable<BoardLocation> ValidCaptures(ChessBoard board, BoardLocation at) 
            => CaptureLocations(board, at).Where(p => Chess.CanTakeAt(board, p, board[at].Piece.Colour));

        private static IEnumerable<BoardLocation> CaptureLocations(ChessBoard board, BoardLocation at)
            => CalcCaptureLocations(board, at, CalcNormalTakePosition);

        private static IEnumerable<BoardLocation> ValidEnPassants(ChessBoard board, BoardLocation at) 
            => EnPassantLocations(board, at).Where(p => Chess.Rules.Pawns.CanEnPassant(board, at, p));

        private static IEnumerable<BoardLocation> EnPassantLocations(ChessBoard board, BoardLocation at) 
            => CalcCaptureLocations(board, at, CalcEnPassantPosition);

        private static IEnumerable<BoardLocation> CalcCaptureLocations(ChessBoard board, BoardLocation at, Func<ChessBoard, BoardLocation, int, BoardLocation> positionCalculator)
        {
            var directions = new[] { Chess.Board.LeftDirectionModifier, Chess.Board.RightDirectionModifier };

            var positions = new List<BoardLocation>();
            foreach (var direction in directions)
            {
                BoardLocation loc;
                if ((loc = positionCalculator(board, at, direction)) != null)
                {
                    positions.Add(loc);
                }
            }
            return positions;
        }

        private static BoardLocation CalcNormalTakePosition(ChessBoard board, BoardLocation at, int horizontal)
        {
            var vertical = Chess.Pieces.VerticalDirectionModifierFor(board[at].Piece);

            if (NotOnEdge(at, horizontal))
            {
                return BoardLocation.At((int)at.File + horizontal, at.Rank + vertical);
            }

            return null;
        }

        private static BoardLocation CalcEnPassantPosition(ChessBoard board, BoardLocation at, int horizontal)
        {
            var vertical = Chess.Pieces.VerticalDirectionModifierFor(board[at].Piece);

            var enpassantRank = Chess.Pieces.EnpassantFromRankFor(board[at].Piece.Colour);

            if (at.Rank == enpassantRank && NotOnEdge(at, horizontal))
            {
                return BoardLocation.At((int) at.File + horizontal, at.Rank + vertical);
            }

            return null;
        }

        private static MoveType PromotedTo(BoardLocation location, Chess.Colours colour, MoveType dflt)
        {
            return location.Rank == Chess.PromotionRankFor(colour)
                ? MoveType.Promotion
                : dflt;
        }

        private static bool NotOnEdge(BoardLocation at, int horizontal)
        {
            var notOnHorizontalEdge = horizontal > 0
                ? at.File < Chess.ChessFile.H
                : at.File > Chess.ChessFile.A;
            return notOnHorizontalEdge;
        }
    }
}