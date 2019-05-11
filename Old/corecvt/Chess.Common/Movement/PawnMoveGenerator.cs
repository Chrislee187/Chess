using System;
using System.Collections.Generic;
using System.Linq;
using Chess.Common.Extensions;
using Chess.Common.System;

//using static CSharpChess.Info;

namespace Chess.Common.Movement
{
    public class PawnMoveGenerator : MoveGeneratorBase
    {
        protected override IEnumerable<Move> ValidMoves(Common.Board board, BoardLocation at) 
            => CalcMoves(board, at, (b, l, c) => b.IsEmptyAt(l));

        protected override IEnumerable<Move> ValidCovers(Common.Board board, BoardLocation at) 
            => CalcMoves(board, at, (b, l, c) => b.IsCoveringAt(l, c));

        protected override IEnumerable<Move> ValidTakes(Common.Board board, BoardLocation at) 
            => CalcTakes(board, at);

        private static IEnumerable<Move> CalcTakes(Common.Board board, BoardLocation at)
        {
            var moves = new List<Move>();
            var pieceColour = board[at].Piece.Colour;

            moves.AddRange(CalcNormalTakes(board, at)
                .Select(l => new Move(at, l, PromotedTo(l, pieceColour, MoveType.Take)))
            );

            moves.AddRange(CalcEnPassants(board, at)
                .Select(l => new Move(at, l, PromotedTo(l, pieceColour, MoveType.TakeEnPassant)))
            );

            return moves;
        }

        private static IEnumerable<Move> CalcMoves(Common.Board board, BoardLocation at, Func<Common.Board, BoardLocation, Colours, bool> destinationCheck)
        {
            var chessPiece = board[at].Piece;
            var direction = Board.ForwardDirectionModifierFor(chessPiece);
            var boardLocation = new BoardLocation(at.File, at.Rank + direction);
            var newMove = new Move(at, boardLocation, PromotedTo(boardLocation, chessPiece.Colour, MoveType.Move));

            var validMoves = new List<Move>();
            if (Validations.IsValidLocation(boardLocation)
                && destinationCheck(board, newMove.To, chessPiece.Colour))
            {
                validMoves.Add(newMove);
                if (board[at].Location.Rank == Pawns.StartingPawnRankFor(chessPiece.Colour))
                {
                    var location = new BoardLocation(at.File, at.Rank + (direction * 2));
                    newMove = new Move(at, location, PromotedTo(location, chessPiece.Colour, MoveType.Move));

                    if (destinationCheck(board, newMove.To, chessPiece.Colour))
                        validMoves.Add(newMove);
                }
            }

            return validMoves;
        }

        private static IEnumerable<BoardLocation> CalcNormalTakes(Common.Board board, BoardLocation at) 
            => TakeLocations(board, at).Where(p => board.CanTakeAt(p, board[at].Piece.Colour));

        private static IEnumerable<BoardLocation> TakeLocations(Common.Board board, BoardLocation at)
            => CreateTakeLocations(board, at, CalcTakeLocation);

        private static IEnumerable<BoardLocation> CalcEnPassants(Common.Board board, BoardLocation at) 
            => CreateEnPassantLocations(board, at)
                .Where(p => 
                    Pawns.EnPassantRules.Check(board, Move.Create(at, p)).All(rr => rr.Passed));

        private static IEnumerable<BoardLocation> CreateEnPassantLocations(Common.Board board, BoardLocation at) 
            => CreateTakeLocations(board, at, CalcEnPassantLocation);

        private static BoardLocation CalcTakeLocation(Common.Board board, BoardLocation at, Board.DirectionModifiers direction)
        {
            var vertical = Board.ForwardDirectionModifierFor(board[at].Piece);

            if (Board.NotOnEdge(at, direction))
            {
                return BoardLocation.At((int)at.File + (int) direction, at.Rank + vertical);
            }

            return null;
        }
        private static BoardLocation CalcEnPassantLocation(Common.Board board, BoardLocation at, Board.DirectionModifiers direction)
        {
            var vertical = Board.ForwardDirectionModifierFor(board[at].Piece);

            var enpassantRank = Pawns.EnpassantFromRankFor(board[at].Piece.Colour);

            if (at.Rank == enpassantRank && Board.NotOnEdge(at, direction))
            {
                return BoardLocation.At((int) at.File + (int) direction, at.Rank + vertical);
            }

            return null;
        }

        private static IEnumerable<BoardLocation> CreateTakeLocations(Common.Board board, BoardLocation at, 
            Func<Common.Board, BoardLocation, Board.DirectionModifiers, BoardLocation> positionCalculator)
        {
            var directions = new[] { Board.DirectionModifiers.LeftDirectionModifier, Board.DirectionModifiers.RightDirectionModifier };

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

        private static MoveType PromotedTo(BoardLocation location, Colours colour, MoveType dflt)
        {
            return location.Rank == Pawns.PromotionRankFor(colour)
                ? MoveType.Promotion
                : dflt;
        }
    }
}