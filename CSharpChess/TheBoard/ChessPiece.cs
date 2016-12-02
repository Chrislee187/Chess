// ReSharper disable MemberCanBePrivate.Global
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

        public bool Is(Chess.Colours colour, Chess.PieceNames name) => Is(colour) && Is(name);
        public bool Is(Chess.Colours colour) => Colour == colour;
        public bool Is(Chess.PieceNames name) => name == Name;
        public bool IsNot(Chess.Colours colour, Chess.PieceNames name) => IsNot(colour) && IsNot(name);
        public bool IsNot(Chess.Colours colour) => Colour != colour;
        public bool IsNot(Chess.PieceNames name) => name != Name;

        public override string ToString() => $"{Colour} {Name}";

        protected bool Equals(ChessPiece other)
        {
            return Colour == other.Colour && Name == other.Name;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            // ReSharper disable once ConvertIfStatementToReturnStatement
            if (obj.GetType() != GetType()) return false;
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