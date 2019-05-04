using chess.engine.Game;

namespace chess.engine.Movement
{
    public class BoardMove
    {
        public static BoardMove CreateMoveOrTake(BoardLocation from, BoardLocation to) => new BoardMove(@from, to, MoveType.MoveOrTake);
        public static BoardMove CreateMoveOnly(BoardLocation from, BoardLocation to) => new BoardMove(@from, to, MoveType.MoveOnly);
        public static BoardMove CreateTakeOnly(BoardLocation from, BoardLocation to) => new BoardMove(@from, to, MoveType.TakeOnly);
        public static BoardMove CreateUpdatePiece(BoardLocation from, BoardLocation to, object updatedPiece) => new BoardMove(@from, to, updatedPiece);

        public static int DirectionModifierFor(Colours player) => player == Colours.White ? +1 : -1;
        public static int EndRankFor(Colours player)
            => player == Colours.White ? 8 : 1;

        public static BoardMove Create(string from, string to, MoveType moveType)
            => Create(BoardLocation.At(from), BoardLocation.At(to), moveType);
        public static BoardMove Create(BoardLocation from, BoardLocation to, MoveType moveType)
            => new BoardMove(from, to, moveType);

        public BoardLocation From { get; }
        public BoardLocation To { get; }
        public MoveType MoveType { get; }

        public object UpdateEntityType { get; }
        public BoardMove(BoardLocation from, BoardLocation to, object updateEntityType) : this(from, to, MoveType.UpdatePiece)
        {
            UpdateEntityType = updateEntityType;
            
        }

        public BoardMove(BoardLocation from, BoardLocation to, MoveType moveType)
        {
            From = from;
            To = to;
            MoveType = moveType;
        }

        #region Equality & ToString()

        protected bool Equals(BoardMove other)
        {
            return Equals(From, other.From)
                   && Equals(To, other.To)
//                   && (
//                       MoveType != MoveType.PawnPromotion 
//                       || Equals(UpdateEntityType, other.UpdateEntityType)
//                       )
                   && MoveType.Equals(other.MoveType);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((BoardMove)obj);
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