using System;
using System.Collections.Generic;
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

        protected override IEnumerable<ChessMove> Moves(ChessBoard board, BoardLocation at) =>
            AddMoveIf(board, at, (b, f, t) => b.IsEmptyAt(t), MoveType.Move);

        protected override IEnumerable<ChessMove> Covers(ChessBoard board, BoardLocation at) =>
            AddMoveIf(board, at, (b, f, t) => board.IsEmptyAt(t) || !board.IsEmptyAt(t) && board[f].Piece.Colour == board[t].Piece.Colour, MoveType.Cover);

        protected override IEnumerable<ChessMove> Takes(ChessBoard board, BoardLocation at) => 
            AddMoveIf(board, at, (b, f, t) => Chess.CanTakeAt(b, t, b[f].Piece.Colour), MoveType.Take);
    }
}