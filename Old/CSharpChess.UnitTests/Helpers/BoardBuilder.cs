using CSharpChess.System;

namespace CSharpChess.UnitTests.Helpers
{
    public static class BoardBuilder
    {
        public static Board EmptyBoard => new Board(false);

        public static Board NewGame => new Board();

        public static Board CustomBoard(string boardInOneCharNotation, Colours toPlay)
        {
            var customboard = ChessBoardHelper.OneCharBoardToBoardPieces(boardInOneCharNotation);
            var board = new Board(customboard, toPlay);
            return board;
        }

    }
}
