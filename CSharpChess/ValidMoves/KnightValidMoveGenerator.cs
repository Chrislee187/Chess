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

        protected override IEnumerable<ChessMove> Moves(ChessBoard board, BoardLocation at)
        {
            var possibleMoves = Chess.Rules.Knights.MovesFrom(at);

            Func<BoardLocation, bool> locationIsValidToMoveTo = 
                board.IsEmptyAt;

            return possibleMoves
                .Where(locationIsValidToMoveTo)
                .Select(m => new ChessMove(at, m, MoveType.Move));
        }

        protected override IEnumerable<ChessMove> Takes(ChessBoard board, BoardLocation at)
        {
            var possibleMoves = Chess.Rules.Knights.MovesFrom(at);
            var chessPiece = board[at].Piece;

            Func<BoardLocation, bool> locationIsValidToTake = l =>
                    Chess.CanTakeAt(board, l, chessPiece.Colour);

            return possibleMoves
                .Where(locationIsValidToTake)
                .Select(m => new ChessMove(at, m, MoveType.Take));
        }
    }
}