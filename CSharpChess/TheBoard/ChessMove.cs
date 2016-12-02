using System;

namespace CSharpChess.TheBoard
{
    public class ChessMove
    {
        public ChessMove(BoardLocation from, BoardLocation to, MoveType moveType)
        {
            From = from;
            To = to;
            MoveType = moveType;
        }

        public BoardLocation From { get; }
        public BoardLocation To { get; }
        public MoveType MoveType { get; }

        public override string ToString() => $"{From}-{To}";


        #region object overrides
        public static implicit operator ChessMove(string move)
        {
            var from = "";
            var to = "";

            if (move.Length == 5)
            {
                from = move.Substring(0, 2);
                to = move.Substring(3, 2);
            }
            else if (move.Length == 4)
            {
                from = move.Substring(0, 2);
                to = move.Substring(2, 2);
            }
            else
            {
                throw new ArgumentException($"'{move}' is not a valid move.");
            }

            return new ChessMove(from, to,MoveType.Unknown);
        }
        // ReSharper disable once MemberCanBePrivate.Global
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
        #endregion
    }
}