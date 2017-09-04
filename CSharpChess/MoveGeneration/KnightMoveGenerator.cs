using System.Collections.Generic;
using CSharpChess.Rules;
using CSharpChess.System.Extensions;
using CSharpChess.TheBoard;

namespace CSharpChess.MoveGeneration
{
    public class KnightMoveGenerator : MoveGeneratorBase
    {
        protected override IEnumerable<ChessMove> ValidMoves(ChessBoard board, BoardLocation at) 
            => CreateMovesIf(board, at, (chessBoard, from, to) => chessBoard.IsEmptyAt(to), MoveType.Move);

        protected override IEnumerable<ChessMove> ValidTakes(ChessBoard board, BoardLocation at) 
            => CreateMovesIf(board, at, (chessBoard, from, to) => chessBoard.CanTakeAt(to, chessBoard[from].Piece.Colour), MoveType.Take);

        protected override IEnumerable<ChessMove> ValidCovers(ChessBoard board, BoardLocation at)
            => CreateMovesIf(board, at, (chessBoard, from, to) => chessBoard.IsCoveringAt(to, chessBoard[from].Piece.Colour), MoveType.Cover);

        private IEnumerable<ChessMove> CreateMovesIf(ChessBoard board, BoardLocation from, DestinationCheck destinationCheck, MoveType moveType)
            =>AddTransformationsIf(board, from, destinationCheck, moveType, Knights.MovementTransformations);
    }
}