using System.Collections.Generic;
using chess.engine.Pieces;

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

        public static Move Create(string from, string to, MoveType moveType = MoveType.MoveOrTake) => new Move(BoardLocation.At(from), BoardLocation.At(to), moveType);

        public BoardLocation From { get; }
        public BoardLocation To { get; }
        public MoveType MoveType { get; }

        private Move(BoardLocation from, BoardLocation to, MoveType moveType = MoveType.MoveOrTake)
        {
            From = from;
            To = to;
            MoveType = moveType;
        }

        protected bool Equals(Move other)
        {
            return Equals(From, other.From) && Equals(To, other.To);
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

        private readonly Dictionary<MoveType, string> _moveTypeDelimiters = new Dictionary<MoveType, string>
        {
            {MoveType.MoveOnly, "-" },
            {MoveType.TakeOnly, "x" },
            {MoveType.MoveOrTake, "" },
        };

        public override string ToString()
        {
            return $"{From}{_moveTypeDelimiters[MoveType]}{To}";
        }

        
    }
}