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

        public override string ToString() => $"{From}-{To}";

        protected bool Equals(ChessMove other)
        {
            return Equals(From, other.From) && Equals(To, other.To);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ChessMove) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((From?.GetHashCode() ?? 0)*397) ^ (To?.GetHashCode() ?? 0);
            }
        }
    }
}