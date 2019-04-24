using System.Collections.Generic;

namespace chess.engine
{
    public class Move
    {
        public static Move CreateMoveOrTake(BoardLocation from, BoardLocation to) => new Move(from, to, MoveType.MoveOrTake);
        public static Move CreateMoveOnly(BoardLocation from, BoardLocation to) => new Move(from, to, MoveType.MoveOnly);
        public static Move CreateTakeOnly(BoardLocation from, BoardLocation to) => new Move(from, to, MoveType.TakeOnly);

        public static int DirectionModifierFor(Colours player) => player == Colours.White ? +1 : -1;
        public static int EndRankFor(Colours player)
            => player == Colours.White ? 8 : 1;

        public static Move Create(string from, string to, MoveType moveType = MoveType.MoveOrTake) 
            => Create(BoardLocation.At(from), BoardLocation.At(to), moveType);
        public static Move Create(BoardLocation from, BoardLocation to, MoveType moveType = MoveType.MoveOrTake)
            => new Move(from, to, moveType);

        public BoardLocation From { get; }
        public BoardLocation To { get; }
        public MoveType MoveType { get; }

        private Move(BoardLocation from, BoardLocation to, MoveType moveType = MoveType.MoveOrTake)
        {
            From = from;
            To = to;
            MoveType = moveType;
        }

        #region Equality & ToString()

        protected bool Equals(Move other)
        {
            return Equals(From, other.From) 
                   && Equals(To, other.To)
                && MoveType.Equals(other.MoveType);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Move) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((From != null ? From.GetHashCode() : 0) * 397) ^ (To != null ? To.GetHashCode() : 0);
            }
        }


        public override string ToString()
        {
            return $"{From}{To}{MoveType}";
        }

        #endregion
    }
}