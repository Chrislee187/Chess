using System.Collections.Generic;
using Chess.Common.Extensions;

namespace Chess.Common.Movement
{
    public class KnightMoveGenerator : MoveGeneratorBase
    {
        protected override IEnumerable<Move> ValidMoves(Common.Board board, BoardLocation at) 
            => CreateMovesIf(board, at, (chessBoard, from, to) => chessBoard.IsEmptyAt(to), MoveType.Move);

        protected override IEnumerable<Move> ValidTakes(Common.Board board, BoardLocation at) 
            => CreateMovesIf(board, at, (chessBoard, from, to) => chessBoard.CanTakeAt(to, chessBoard[from].Piece.Colour), MoveType.Take);

        protected override IEnumerable<Move> ValidCovers(Common.Board board, BoardLocation at)
            => CreateMovesIf(board, at, (chessBoard, from, to) => chessBoard.IsCoveringAt(to, chessBoard[from].Piece.Colour), MoveType.Cover);

        private IEnumerable<Move> CreateMovesIf(Common.Board board, BoardLocation from, DestinationCheck destinationCheck, MoveType moveType)
            =>AddTransformationsIf(board, from, destinationCheck, moveType, Knights.MovementTransformations);
    }
}