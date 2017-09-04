using System;
using System.Collections.Generic;
using System.Linq;
using CSharpChess.System;
using CSharpChess.System.Extensions;
using CSharpChess.TheBoard;
using static CSharpChess.Chess;
using static CSharpChess.Rules.Pawns;

namespace CSharpChess.MoveGeneration
{
    public class PawnMoveGenerator : MoveGeneratorBase
    {
        protected override IEnumerable<ChessMove> ValidMoves(ChessBoard board, BoardLocation at) 
            => CalcMoves(board, at, (b, l, c) => b.IsEmptyAt(l));

        protected override IEnumerable<ChessMove> ValidCovers(ChessBoard board, BoardLocation at) 
            => CalcMoves(board, at, (b, l, c) => b.IsCoveringAt(l, c));

        protected override IEnumerable<ChessMove> ValidTakes(ChessBoard board, BoardLocation at) 
            => CalcTakes(board, at);

        private static IEnumerable<ChessMove> CalcTakes(ChessBoard board, BoardLocation at)
        {
            var moves = new List<ChessMove>();
            var pieceColour = board[at].Piece.Colour;

            moves.AddRange(CalcNormalTakes(board, at)
                .Select(l => new ChessMove(at, l, PromotedTo(l, pieceColour, MoveType.Take)))
            );

            moves.AddRange(CalcEnPassants(board, at)
                .Select(l => new ChessMove(at, l, PromotedTo(l, pieceColour, MoveType.TakeEnPassant)))
            );

            return moves;
        }

        private static IEnumerable<ChessMove> CalcMoves(ChessBoard board, BoardLocation at, Func<ChessBoard, BoardLocation, Colours, bool> destinationCheck)
        {
            var chessPiece = board[at].Piece;
            var direction = System.Board.ForwardDirectionModifierFor(chessPiece);
            var boardLocation = new BoardLocation(at.File, at.Rank + direction);
            var newMove = new ChessMove(at, boardLocation, PromotedTo(boardLocation, chessPiece.Colour, MoveType.Move));

            var validMoves = new List<ChessMove>();
            if (Validations.IsValidLocation(boardLocation)
                && destinationCheck(board, newMove.To, chessPiece.Colour))
            {
                validMoves.Add(newMove);
                if (board[at].Location.Rank == StartingPawnRankFor(chessPiece.Colour))
                {
                    var location = new BoardLocation(at.File, at.Rank + (direction * 2));
                    newMove = new ChessMove(at, location, PromotedTo(location, chessPiece.Colour, MoveType.Move));

                    if (destinationCheck(board, newMove.To, chessPiece.Colour))
                        validMoves.Add(newMove);
                }
            }

            return validMoves;
        }

        private static IEnumerable<BoardLocation> CalcNormalTakes(ChessBoard board, BoardLocation at) 
            => TakeLocations(board, at).Where(p => board.CanTakeAt(p, board[at].Piece.Colour));

        private static IEnumerable<BoardLocation> TakeLocations(ChessBoard board, BoardLocation at)
            => CreateTakeLocations(board, at, CalcTakeLocation);

        private static IEnumerable<BoardLocation> CalcEnPassants(ChessBoard board, BoardLocation at) 
            => CreateEnPassantLocations(board, at)
                .Where(p => 
                    EnPassantRules.Check(board, ChessMove.Create(at, p)).All(rr => rr.Passed));

        private static IEnumerable<BoardLocation> CreateEnPassantLocations(ChessBoard board, BoardLocation at) 
            => CreateTakeLocations(board, at, CalcEnPassantLocation);

        private static BoardLocation CalcTakeLocation(ChessBoard board, BoardLocation at, System.Board.DirectionModifiers direction)
        {
            var vertical = System.Board.ForwardDirectionModifierFor(board[at].Piece);

            if (System.Board.NotOnEdge(at, direction))
            {
                return BoardLocation.At((int)at.File + (int) direction, at.Rank + vertical);
            }

            return null;
        }
        private static BoardLocation CalcEnPassantLocation(ChessBoard board, BoardLocation at, System.Board.DirectionModifiers direction)
        {
            var vertical = System.Board.ForwardDirectionModifierFor(board[at].Piece);

            var enpassantRank = EnpassantFromRankFor(board[at].Piece.Colour);

            if (at.Rank == enpassantRank && System.Board.NotOnEdge(at, direction))
            {
                return BoardLocation.At((int) at.File + (int) direction, at.Rank + vertical);
            }

            return null;
        }

        private static IEnumerable<BoardLocation> CreateTakeLocations(ChessBoard board, BoardLocation at, 
            Func<ChessBoard, BoardLocation, System.Board.DirectionModifiers, BoardLocation> positionCalculator)
        {
            var directions = new[] { System.Board.DirectionModifiers.LeftDirectionModifier, System.Board.DirectionModifiers.RightDirectionModifier };

            var positions = new List<BoardLocation>();
            foreach (var direction in directions)
            {
                BoardLocation loc;
                if ((loc = positionCalculator(board, at, direction)) != null && Validations.IsValidLocation(loc))
                {
                    positions.Add(loc);
                }
            }
            return positions;
        }

        private static MoveType PromotedTo(BoardLocation location, Chess.Colours colour, MoveType dflt)
        {
            return location.Rank == PromotionRankFor(colour)
                ? MoveType.Promotion
                : dflt;
        }
    }
}