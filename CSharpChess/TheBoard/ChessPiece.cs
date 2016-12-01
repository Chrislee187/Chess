namespace CSharpChess.TheBoard
{
    public class ChessPiece
    {
        public static ChessPiece NullPiece = new ChessPiece(Chess.Colours.None, Chess.PieceNames.Blank);

        public Chess.Colours Colour { get; }
        public Chess.PieceNames Name { get; }

        public ChessPiece(Chess.Colours colour, Chess.PieceNames name)
        {
            Name = name;
            Colour = colour;
        }

        public override string ToString() => $"{Colour} {Name}";

        protected bool Equals(ChessPiece other)
        {
            return Colour == other.Colour && Name == other.Name;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
//            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ChessPiece) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((int) Colour*397) ^ (int) Name;
            }
        }
    }
}