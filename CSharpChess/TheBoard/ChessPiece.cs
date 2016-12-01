namespace CSharpChess.TheBoard
{
    public class ChessPiece
    {
        public static ChessPiece NullPiece = new ChessPiece(Chess.Colours.White, Chess.PieceNames.Blank);

        public Chess.Colours Colour { get; }
        public Chess.PieceNames Name { get; }

        public ChessPiece(Chess.Colours colour, Chess.PieceNames name)
        {
            Name = name;
            Colour = colour;
        }

        private ChessPiece()
        {
        }

        public override string ToString() => $"{Colour} {Name}";
    }
}