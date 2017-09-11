using System.Collections.Generic;
using CSharpChess.Extensions;

namespace CSharpChess.Movement
{
    public class KnightMoveGenerator : MoveGeneratorBase
    {
        protected override IEnumerable<Move> ValidMoves(CSharpChess.Board board, BoardLocation at) 
            => CreateMovesIf(board, at, (chessBoard, from, to) => chessBoard.IsEmptyAt(to), MoveType.Move);

        protected override IEnumerable<Move> ValidTakes(CSharpChess.Board board, BoardLocation at) 
            => CreateMovesIf(board, at, (chessBoard, from, to) => chessBoard.CanTakeAt(to, chessBoard[from].Piece.Colour), MoveType.Take);

        protected override IEnumerable<Move> ValidCovers(CSharpChess.Board board, BoardLocation at)
            => CreateMovesIf(board, at, (chessBoard, from, to) => chessBoard.IsCoveringAt(to, chessBoard[from].Piece.Colour), MoveType.Cover);

        private IEnumerable<Move> CreateMovesIf(CSharpChess.Board board, BoardLocation from, DestinationCheck destinationCheck, MoveType moveType)
            =>AddTransformationsIf(board, from, destinationCheck, moveType, Knights.MovementTransformations);
    }
}