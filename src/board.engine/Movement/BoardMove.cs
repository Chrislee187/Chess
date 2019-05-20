using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace board.engine.Movement
{
    [DebuggerDisplay("{DebuggerDisplay, nq}")]
    public class BoardMove : ICloneable
    {
        private string DebuggerDisplay => $"{From}{To}{MoveType}";

        public static BoardMove Create(BoardLocation from, BoardLocation to, int moveType)
            => new BoardMove(from, to, moveType);

        public BoardLocation From { get; }
        public BoardLocation To { get; }
        public int MoveType { get; }

        public object ExtraData { get; private set; }

        public BoardMove(BoardLocation from, BoardLocation to, int moveType, object extraData = null)
        {
            Guard.ArgumentException(() => from.Equals(to), "Cannot move to same place as starting location");
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
                   && MoveType.Equals(other.MoveType)
//                   && (
//                       MoveType != MoveType.PawnPromotion 
//                       || Equals(ExtraData, other.ExtraData)
//                       )
                ;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            // ReSharper disable once ArrangeThisQualifier
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

        public object Clone()
        {
            var boardMove = new BoardMove(From, To, MoveType);
            boardMove.ExtraData = ExtraData;
            return boardMove;
        }

        #endregion
    }

    public static class BoardMoveExtensions
    {

        // TODO: extraData should be IEquatable<>?
        public static BoardMove FindMove(this IEnumerable<BoardMove> moves, BoardLocation from, BoardLocation to, object extraData = null)
        {
            return moves.SingleOrDefault(mv => mv.From.Equals(from)
                                               && mv.To.Equals(to)
                                               && (extraData == null
                                                   || mv.ExtraData.Equals(extraData))
            );
        }
    }

}