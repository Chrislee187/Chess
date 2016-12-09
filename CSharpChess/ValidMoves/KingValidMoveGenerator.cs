using System;
using System.Collections.Generic;
using System.Linq;
using CSharpChess.TheBoard;

namespace CSharpChess.ValidMoves
{
    public class KingValidMoveGenerator : ValidMoveGeneratorBase
    {
        public KingValidMoveGenerator() : base(Chess.PieceNames.King)
        { }

        private IEnumerable<ChessMove> AddMoveIf(ChessBoard board, BoardLocation at,
            Func<ChessBoard, BoardLocation, BoardLocation, bool> predicate, MoveType moveType)
        {
            var result = new List<ChessMove>();
            var directions = Chess.Rules.KingAndQueen.DirectionTransformations;
            
            foreach (var direction in directions)
            {
                var to = StraightLineValidMoveGenerator.ApplyDirection(at, direction);

                if (to != null
                    && predicate(board, at, to))
                {
                    result.Add(new ChessMove(at, to, moveType));
                }
            }

            return result;
        }

        public override IEnumerable<ChessMove> Moves(ChessBoard board, BoardLocation at)
        {
            var moves = new List<ChessMove>();
            moves.AddRange(AddMoveIf(board, at, (b, f, t) => b.IsEmptyAt(t), MoveType.Move));

            // Castles
            moves.AddRange(Castles(board, at));

            return moves;
        }

        private IEnumerable<ChessMove> Castles(ChessBoard board, BoardLocation at)
        {
            var moves = new List<ChessMove>();

            var piece = board[at];
            if (piece.Piece.IsNot(Chess.PieceNames.King) || piece.MoveHistory.Any()) return moves;

            var leftRookLoc = BoardLocation.At(Chess.ChessFile.A, at.Rank);
            var rightRookLoc = BoardLocation.At(Chess.ChessFile.H, at.Rank);

            ChessMove to = CanCastle(board, at, leftRookLoc);
            if (to != null) moves.Add(to);

            to = CanCastle(board, at, rightRookLoc);
            if (to != null) moves.Add(to);

            return moves;
        }

        private static ChessMove CanCastle(ChessBoard board, BoardLocation at, BoardLocation leftRookLoc)
        {
            var rookLoc = board[leftRookLoc];
            if (rookLoc.Piece.IsNot(Chess.PieceNames.Rook) || rookLoc.MoveHistory.Any()) return null;

            var mustBeEmpty = LocationsBetween(at, rookLoc.Location);
            if (mustBeEmpty.Any(board.IsNotEmptyAt)) return null;

            var castleFile = rookLoc.Location.File == Chess.ChessFile.A ? Chess.ChessFile.C : Chess.ChessFile.G;
            return new ChessMove(at, BoardLocation.At(castleFile, at.Rank), MoveType.Castle);
        }

        private static IEnumerable<BoardLocation> LocationsBetween(BoardLocation at, BoardLocation rookLoc)
        {
            int from, to;
            if (rookLoc.File == Chess.ChessFile.A)
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

        public override IEnumerable<ChessMove> Covers(ChessBoard board, BoardLocation at) =>
            AddMoveIf(board, at, (b, f, t) => board.IsEmptyAt(t) || !board.IsEmptyAt(t) && board[f].Piece.Colour == board[t].Piece.Colour, MoveType.Cover);

        public override IEnumerable<ChessMove> Takes(ChessBoard board, BoardLocation at) => 
            AddMoveIf(board, at, (b, f, t) => Chess.CanTakeAt(b, t, b[f].Piece.Colour), MoveType.Take);
    }
}