using CSharpChess.System;
using CSharpChess.TheBoard;
using static CSharpChess.Chess;

namespace CSharpChess.UnitTests.Helpers
{
    public static class BoardBuilder
    {
        public static ChessBoard EmptyBoard => new ChessBoard(false);

        public static ChessBoard NewGame => new ChessBoard();

        public static ChessBoard CustomBoard(string boardInOneCharNotation, Colours toPlay)
        {
            var customboard = ChessBoardHelper.OneCharBoardToBoardPieces(boardInOneCharNotation);
            var board = new ChessBoard(customboard, toPlay);
            return board;
        }

    }
}
