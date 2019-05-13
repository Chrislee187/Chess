using System;
using System.Diagnostics;
using board.engine.Board;

namespace board.engine.Movement
{
    [DebuggerDisplay("{DebuggerDisplay, nq}")]
    public class BoardMove : ICloneable
    {
        private string DebuggerDisplay => $"{From},{To}";

        public static BoardMove Create(BoardLocation from, BoardLocation to, int moveType)
            => new BoardMove(from, to, moveType);

        public BoardLocation From { get; }
        public BoardLocation To { get; }
        public int MoveType { get; }

        public object ExtraData { get; private set; }

        public BoardMove(BoardLocation from, BoardLocation to, int moveType, object extraData = null)
        {
            From = from;
            To = to;
            MoveType = moveType;
            ExtraData = extraData;
        }

        #region Equality & ToString()

        protected bool Equals(BoardMove other)
        {
            return Equals(From, other.From)
                   && Equals(To, other.To)
//                   && (
//                       MoveType != MoveType.PawnPromotion 
//                       || Equals(ExtraData, other.ExtraData)
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

        public object Clone()
        {
            var boardMove = new BoardMove(From, To, MoveType);
            boardMove.ExtraData = ExtraData;
            return boardMove;
        }

        #endregion
    }
}