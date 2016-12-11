using System;
using System.Collections.Generic;
using System.Linq;
using CSharpChess.TheBoard;
using static CSharpChess.Chess.Rules;

namespace CSharpChess.ValidMoves
{
    public class KingValidMoveGenerator : ValidMoveGeneratorBase
    {
        public override IEnumerable<ChessMove> All(ChessBoard board, BoardLocation at)
        {
            return ValidMoves(board, at)
                .Concat(ValidTakes(board, at))
                .Concat(ValidCovers(board, at));
        }

        private IEnumerable<ChessMove> AddMoveIf(ChessBoard board, BoardLocation at,
            Func<ChessBoard, BoardLocation, BoardLocation, bool> predicate, MoveType moveType)
        {
            var result = new List<ChessMove>();
            var possibleMoves = MovementTransformation.ApplyTo(KingAndQueen.DirectionTransformations, at);
            foreach (var to in possibleMoves)
            {
                if (predicate(board, at, to)
                    //&& wouldNotBeInCheck(board, to, board[at].Piece.Colour)
                    )
                {
                    result.Add(new ChessMove(at, to, moveType));
                }
            }

            return result;
        }

        private bool wouldNotBeInCheck(ChessBoard board, BoardLocation at, Chess.Board.Colours asPlayer)
        {
            // TODO: Need to place the analyser outside of the generators, recursion alert
//            var a = new ThreatAnalyser(board);
//
//            return a.ThreatsAgainst(asPlayer, at).Any();
            throw new ArgumentException();
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
            if (piece.Piece.IsNot(Chess.Board.PieceNames.King) || piece.MoveHistory.Any()) return moves;

            var leftRookLoc = BoardLocation.At(Chess.Board.ChessFile.A, at.Rank);
            var rightRookLoc = BoardLocation.At(Chess.Board.ChessFile.H, at.Rank);

            ChessMove to = CanCastle(board, at, leftRookLoc);
            if (to != null) moves.Add(to);

            to = CanCastle(board, at, rightRookLoc);
            if (to != null) moves.Add(to);

            return moves;
        }

        private static ChessMove CanCastle(ChessBoard board, BoardLocation at, BoardLocation leftRookLoc)
        {
            var rookLoc = board[leftRookLoc];
            if (rookLoc.Piece.IsNot(Chess.Board.PieceNames.Rook) || rookLoc.MoveHistory.Any()) return null;

            var mustBeEmpty = LocationsBetweenAndNotUnderAttack(at, rookLoc.Location);
            if (mustBeEmpty.Any(board.IsNotEmptyAt)) return null;

            var castleFile = rookLoc.Location.File == Chess.Board.ChessFile.A ? Chess.Board.ChessFile.C : Chess.Board.ChessFile.G;
            return new ChessMove(at, BoardLocation.At(castleFile, at.Rank), MoveType.Castle);
        }

        private static IEnumerable<BoardLocation> LocationsBetweenAndNotUnderAttack(BoardLocation at, BoardLocation rookLoc)
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