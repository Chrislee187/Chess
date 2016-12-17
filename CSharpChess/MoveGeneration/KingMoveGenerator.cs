using System.Collections.Generic;
using System.Linq;
using CSharpChess.System.Extensions;
using CSharpChess.TheBoard;

namespace CSharpChess.MoveGeneration
{
    public class KingMoveGenerator : MoveGeneratorBase
    {
        protected override IEnumerable<ChessMove> ValidMoves(ChessBoard board, BoardLocation at)
            => AddTransformationsIf(board, at, (b, f, t) => b.IsEmptyAt(t), 
                MoveType.Move, Chess.Rules.King.MovementTransformations)
            .Concat(Castles(board, at));

        protected override IEnumerable<ChessMove> ValidCovers(ChessBoard board, BoardLocation at) 
            => AddTransformationsIf(board, at, (b, f, t) => board.IsCoveringAt(t, board[f].Piece.Colour), 
                MoveType.Cover, Chess.Rules.King.MovementTransformations);

        protected override IEnumerable<ChessMove> ValidTakes(ChessBoard board, BoardLocation at) 
            => AddTransformationsIf(board, at, (b, f, t) => b.CanTakeAt(t, b[f].Piece.Colour), 
                MoveType.Take, Chess.Rules.King.MovementTransformations);

        private IEnumerable<ChessMove> Castles(ChessBoard board, BoardLocation at)
        {
            var moves = new List<ChessMove>();

            var piece = board[at];
            if (piece.Piece.IsNot(Chess.PieceNames.King) || piece.MoveHistory.Any()) return moves;

            var leftRookLoc = BoardLocation.At(Chess.Board.ChessFile.A, at.Rank);
            var rightRookLoc = BoardLocation.At(Chess.Board.ChessFile.H, at.Rank);

            var to = board.CanCastle(at, leftRookLoc);
            if (to != null) moves.Add(to);

            to = board.CanCastle(at, rightRookLoc);
            if (to != null) moves.Add(to);

            return moves;
        }
    }
}