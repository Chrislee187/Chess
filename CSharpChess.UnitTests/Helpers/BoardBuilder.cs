using CSharpChess.TheBoard;

namespace CSharpChess.UnitTests.Helpers
{
    public class BoardBuilder
    {
        public static ChessBoard EmptyBoard => new ChessBoard(false);

        public static ChessBoard NewGame => new ChessBoard(true);
    }
}