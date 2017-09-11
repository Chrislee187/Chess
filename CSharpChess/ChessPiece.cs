// ReSharper disable MemberCanBePrivate.Global

namespace CSharpChess
{
    public class ChessPiece
    {
        public static readonly ChessPiece NullPiece = new ChessPiece(Colours.None, PieceNames.Blank);

        public Colours Colour { get; }
        public PieceNames Name { get; }

        public ChessPiece(Colours colour, PieceNames name)
        {
            Name = name;
            Colour = colour;
        }

        public bool Is(Colours colour, PieceNames name) => Is(colour) && Is(name);
        public bool Is(Colours colour) => Colour == colour;
        public bool Is(PieceNames name) => name == Name;
        public bool IsNot(Colours colour, PieceNames name) => IsNot(colour) && IsNot(name);
        public bool IsNot(Colours colour) => Colour != colour;
        public bool IsNot(PieceNames name) => name != Name;

        public ChessPiece Clone() => new ChessPiece(Colour, Name);

        public override string ToString() => $"{Colour} {Name}";

        #region Equality
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
        #endregion
    }
}