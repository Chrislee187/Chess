namespace CSharpChess.TheBoard
{
    public class ChessMove
    {
        public ChessMove(BoardLocation from, BoardLocation to)
        {
            From = from;
            To = to;
        }

        public BoardLocation From { get; }
        public BoardLocation To { get; }
    }
}