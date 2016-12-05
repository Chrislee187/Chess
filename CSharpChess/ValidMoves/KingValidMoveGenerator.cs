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

        protected override IEnumerable<ChessMove> Moves(ChessBoard board, BoardLocation at)
        {
            var result = new List<ChessMove>();
            var directions = Chess.Rules.Queens.DirectionTransformations;
            var piece = board[at].Piece;
            foreach (var direction in directions)
            {
                var to = StraightLineValidMoveGenerator.ApplyDirection(at, direction);

                if (to != null)
                {
                    if (!board.DefendingFrom(to, piece.Colour).Any())
                    {
                        if (board.IsEmptyAt(to))
                            result.Add(new ChessMove(at, to, MoveType.Move));
                    }
                }
            }

            return result;
        }
        protected override IEnumerable<ChessMove> Takes(ChessBoard board, BoardLocation at)
        {
            var result = new List<ChessMove>();
            var directions = Chess.Rules.Queens.DirectionTransformations;

            foreach (var direction in directions)
            {
                var to = StraightLineValidMoveGenerator.ApplyDirection(at, direction);

                var attackerColour = board[at].Piece.Colour;
                if (to != null && Chess.CanTakeAt(board, to, attackerColour))
                    result.Add(new ChessMove(at, to, MoveType.Take));
            }

            return result;
        }
    }
}