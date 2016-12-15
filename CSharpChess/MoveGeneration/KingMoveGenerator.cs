using System;
using System.Collections.Generic;
using System.Linq;
using CSharpChess.Extensions;
using CSharpChess.TheBoard;

namespace CSharpChess.MoveGeneration
{
    public class KingMoveGenerator : MoveGeneratorBase
    {
        public override IEnumerable<ChessMove> All(ChessBoard board, BoardLocation at)
        {
            // TODO: This is common in all top level move generators, sort it out
            return ValidMoves(board, at)
                .Concat(ValidTakes(board, at))
                .Concat(ValidCovers(board, at)).ToList()
                ;
        }

        private IEnumerable<ChessMove> AddMoveIf(ChessBoard board, BoardLocation at,
            Func<ChessBoard, BoardLocation, BoardLocation, bool> predicate, MoveType moveType)
        {
            var result = new List<ChessMove>();
            var possibleMoves = Chess.Rules.MovementTransformation.ApplyTo(at, Chess.Rules.KingAndQueen.DirectionTransformations);
            foreach (var to in possibleMoves)
            {
                if (predicate(board, at, to))
                {
                    result.Add(new ChessMove(at, to, moveType));
                }
            }

            return result;

        }

        private IEnumerable<ChessMove> ValidMoves(ChessBoard board, BoardLocation at)
        {
            var moves = new List<ChessMove>();
            moves.AddRange(AddMoveIf(board, at, (b, f, t) => b.IsEmptyAt(t), MoveType.Move));

            // Castles
            moves.AddRange(Castles(board, at));

            return moves;
        }

        private IEnumerable<ChessMove> ValidCovers(ChessBoard board, BoardLocation at) =>
            AddMoveIf(board, at, (b, f, t) => board.IsCoveringAt(t, board[f].Piece.Colour), MoveType.Cover);

        private IEnumerable<ChessMove> ValidTakes(ChessBoard board, BoardLocation at) =>
            AddMoveIf(board, at, (b, f, t) => b.CanTakeAt(t, b[f].Piece.Colour), MoveType.Take);

        private IEnumerable<ChessMove> Castles(ChessBoard board, BoardLocation at)
        {
            var moves = new List<ChessMove>();

            var piece = board[at];
            if (piece.Piece.IsNot(Chess.PieceNames.King) || piece.MoveHistory.Any()) return moves;

            var leftRookLoc = BoardLocation.At(Chess.Board.ChessFile.A, at.Rank);
            var rightRookLoc = BoardLocation.At(Chess.Board.ChessFile.H, at.Rank);

            ChessMove to = board.CanCastle(at, leftRookLoc);
            if (to != null) moves.Add(to);

            to = board.CanCastle(at, rightRookLoc);
            if (to != null) moves.Add(to);

            return moves;
        }

    }
}