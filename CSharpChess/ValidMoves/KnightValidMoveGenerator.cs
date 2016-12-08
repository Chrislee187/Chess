using System;
using System.Collections.Generic;
using System.Linq;
using CSharpChess.TheBoard;

namespace CSharpChess.ValidMoves
{
    public class KnightValidMoveGenerator : ValidMoveGeneratorBase
    {
        public KnightValidMoveGenerator() : base(Chess.PieceNames.Knight)
        {}

        private IEnumerable<ChessMove> AddMoveIf(ChessBoard board, BoardLocation at,
            Func<ChessBoard, BoardLocation, BoardLocation, bool> predicate, MoveType moveType)
        {
            var possibleMoves = Chess.Rules.Knights.MovesFrom(at);

            return possibleMoves
                .Where(to => predicate(board, at, to))
                .Select(m => new ChessMove(at, m, moveType));
        }

        public override IEnumerable<ChessMove> Moves(ChessBoard board, BoardLocation at) 
            => AddMoveIf(board, at, (b, f, t) => b.IsEmptyAt(t), MoveType.Move);

        public override IEnumerable<ChessMove> Takes(ChessBoard board, BoardLocation at) 
            => AddMoveIf(board, at, (b, f, t) => Chess.CanTakeAt(b, t, b[f].Piece.Colour), MoveType.Take);

        public override IEnumerable<ChessMove> Covers(ChessBoard board, BoardLocation at)
            => AddMoveIf(board, at, (b, f, t) => b[f].Piece.Colour == b[t].Piece.Colour, MoveType.Cover);

    }
}