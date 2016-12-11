using System;
using System.Collections.Generic;
using System.Linq;
using CSharpChess.TheBoard;
using static CSharpChess.Chess.Rules;

namespace CSharpChess.ValidMoves
{
    public class KingMoveGenerator : MoveGeneratorBase
    {
        public override IEnumerable<ChessMove> All(ChessBoard board, BoardLocation at)
        {
            // TODO: This is common in all top level move generators, sort it out
            return ValidMoves(board, at)
                .Concat(ValidTakes(board, at))
                .Concat(ValidCovers(board, at)).ToList();
        }

        private IEnumerable<ChessMove> AddMoveIf(ChessBoard board, BoardLocation at,
            Func<ChessBoard, BoardLocation, BoardLocation, bool> predicate, MoveType moveType)
        {
            var result = new List<ChessMove>();
            var possibleMoves = MovementTransformation.ApplyTo(at, KingAndQueen.DirectionTransformations);
            foreach (var to in possibleMoves)
            {
                if (predicate(board, at, to))
                {
                    result.Add(new ChessMove(at, to, moveType));
                }
            }

            return result; //Where(r => !InCheck(board, r.To, board[at].Piece.Colour));

        }

//        private bool InCheck(ChessBoard board, BoardLocation at, Chess.Board.Colours asPlayer)
//        {
////            board.Pieces.EnemyOf(asPlayer).ToList()[3].MoveFactory
////                .Moves(board, BoardLocation.At("F8")).ToList().Any(m => m.To.Equals(at))
////
////            var enemyPieces = board.Pieces.EnemyOf(asPlayer);
////            Func<ChessBoard, BoardPiece, BoardLocation, bool> pieceIsAttackingLocation =
////                delegate(ChessBoard b, BoardPiece p, BoardLocation l)
////                {
////                    var moves = p.MoveFactory.Moves(board, p.Location).ToList();
////                    var covers = p.MoveFactory.Covers(board, p.Location).ToList();
////                    return false; //moves.Any(m => m.To.Equals(l))|| covers.Any(m => m.To.Equals(l));
////                };
////            var checkPieces = enemyPieces.Where(p => pieceIsAttackingLocation(board, p, at));
////
////            return checkPieces.Any();
//        }

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
            if (piece.Piece.IsNot(Chess.Board.PieceNames.King) || piece.MoveHistory.Any()) return moves;

            var leftRookLoc = BoardLocation.At(Chess.Board.ChessFile.A, at.Rank);
            var rightRookLoc = BoardLocation.At(Chess.Board.ChessFile.H, at.Rank);

            ChessMove to = board.CanCastle(at, leftRookLoc);
            if (to != null) moves.Add(to);

            to = board.CanCastle(at, rightRookLoc);
            if (to != null) moves.Add(to);

            return moves;
        }

        public static IEnumerable<BoardLocation> LocationsBetweenAndNotUnderAttack(BoardLocation at, BoardLocation rookLoc)
        {
            int from, to;
            if (rookLoc.File == Chess.Board.ChessFile.A)
            {
                @from = 2;
                to = (int) at.File - 1;
            }
            else
            {
                @from = (int) at.File + 1;
                to = 7;
            }

            var mustBeEmpty = Enumerable.Range(@from, to - from +1).Select(v => BoardLocation.At(v, at.Rank));
            return mustBeEmpty;
        }

    }
}