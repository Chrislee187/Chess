using System;
using System.Collections.Generic;
using System.Linq;
using CSharpChess.System.Extensions;
using CSharpChess.TheBoard;

namespace CSharpChess.MoveGeneration
{
    public class PawnMoveGenerator : MoveGeneratorBase
    {
        public override IEnumerable<ChessMove> All(ChessBoard board, BoardLocation at)
        {
            return ValidMoves(board, at)
                .Concat(ValidTakes(board, at))
                .Concat(ValidCovers(board, at)).ToList();
        }

        private IEnumerable<ChessMove> ValidTakes(ChessBoard board, BoardLocation at)
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
        
        private IEnumerable<ChessMove> ValidMoves(ChessBoard board, BoardLocation at)
        {
            var chessPiece = board[at].Piece;
            var direction = Chess.Board.ForwardDirectionModifierFor(chessPiece);
            var boardLocation = new BoardLocation(at.File, at.Rank + direction);
            var newMove = new ChessMove(at, boardLocation, PromotedTo(boardLocation, chessPiece.Colour, MoveType.Move));

            var validMoves = new List<ChessMove>();
            if (Chess.Board.Validations.IsValidLocation(boardLocation) && board.IsEmptyAt(newMove.To))
            {
                validMoves.Add(newMove);
                if (board[at].Location.Rank == Chess.Rules.Pawns.StartingPawnRankFor(chessPiece.Colour) )
                {
                    var location = new BoardLocation(at.File, at.Rank + (direction * 2));
                    newMove = new ChessMove(at, location, PromotedTo(location, chessPiece.Colour, MoveType.Move));

                    if (board.IsEmptyAt(newMove.To))
                        validMoves.Add(newMove);
                }
            }

            return validMoves;
        }

        private IEnumerable<ChessMove> ValidCovers(ChessBoard board, BoardLocation at)
        {
            var chessPiece = board[at].Piece;
            var direction = Chess.Board.ForwardDirectionModifierFor(chessPiece);
            var boardLocation = new BoardLocation(at.File, at.Rank + direction);
            var newMove = new ChessMove(at, boardLocation, PromotedTo(boardLocation, chessPiece.Colour, MoveType.Move));

            var validMoves = new List<ChessMove>();
            if (Chess.Board.Validations.IsValidLocation(boardLocation) && !board.IsEmptyAt(newMove.To) 
                && board.IsCoveringAt(newMove.To, chessPiece.Colour))
            {
                validMoves.Add(newMove);
                if (board[at].Location.Rank == Chess.Rules.Pawns.StartingPawnRankFor(chessPiece.Colour))
                {
                    var location = new BoardLocation(at.File, at.Rank + (direction * 2));
                    newMove = new ChessMove(at, location, PromotedTo(location, chessPiece.Colour, MoveType.Move));

                    if (!board.IsEmptyAt(newMove.To) && board[newMove.To].Piece.Colour == chessPiece.Colour)
                        validMoves.Add(newMove);
                }
            }

            return validMoves;
        }

        private static IEnumerable<BoardLocation> ValidCaptures(ChessBoard board, BoardLocation at) 
            => CaptureLocations(board, at).Where(p => board.CanTakeAt(p, board[at].Piece.Colour));

        private static IEnumerable<BoardLocation> CaptureLocations(ChessBoard board, BoardLocation at)
            => CalcCaptureLocations(board, at, CalcNormalTakePosition);

        private static IEnumerable<BoardLocation> ValidEnPassants(ChessBoard board, BoardLocation at) 
            => EnPassantLocations(board, at).Where(p => Chess.Rules.Pawns.CanEnPassant(board, at, p));

        private static IEnumerable<BoardLocation> EnPassantLocations(ChessBoard board, BoardLocation at) 
            => CalcCaptureLocations(board, at, CalcEnPassantPosition);

        private static IEnumerable<BoardLocation> CalcCaptureLocations(ChessBoard board, BoardLocation at, Func<ChessBoard, BoardLocation, Chess.Board.DirectionModifiers, BoardLocation> positionCalculator)
        {
            var directions = new[] { Chess.Board.DirectionModifiers.LeftDirectionModifier, Chess.Board.DirectionModifiers.RightDirectionModifier };

            var positions = new List<BoardLocation>();
            foreach (var direction in directions)
            {
                BoardLocation loc;
                if ((loc = positionCalculator(board, at, direction)) != null && Chess.Board.Validations.IsValidLocation(loc))
                {
                    positions.Add(loc);
                }
            }
            return positions;
        }

        private static BoardLocation CalcNormalTakePosition(ChessBoard board, BoardLocation at, Chess.Board.DirectionModifiers horizontal)
        {
            var vertical = Chess.Board.ForwardDirectionModifierFor(board[at].Piece);

            if (Chess.Board.NotOnEdge(at, horizontal))
            {
                return BoardLocation.At((int)at.File + (int) horizontal, at.Rank + vertical);
            }

            return null;
        }

        private static BoardLocation CalcEnPassantPosition(ChessBoard board, BoardLocation at, Chess.Board.DirectionModifiers horizontal)
        {
            var vertical = Chess.Board.ForwardDirectionModifierFor(board[at].Piece);

            var enpassantRank = Chess.Rules.Pawns.EnpassantFromRankFor(board[at].Piece.Colour);

            if (at.Rank == enpassantRank && Chess.Board.NotOnEdge(at, horizontal))
            {
                return BoardLocation.At((int) at.File + (int) horizontal, at.Rank + vertical);
            }

            return null;
        }

        private static MoveType PromotedTo(BoardLocation location, Chess.Colours colour, MoveType dflt)
        {
            return location.Rank == Chess.Rules.Pawns.PromotionRankFor(colour)
                ? MoveType.Promotion
                : dflt;
        }
    }
}