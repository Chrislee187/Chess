using System;
using chess.engine.Chess;
using chess.engine.Game;

namespace chess.engine.Movement
{
    public class ChessMove 
    {
        public static ChessMove CreateMoveOrTake(BoardLocation from, BoardLocation to) => new ChessMove(@from, to, ChessMoveType.MoveOrTake);
        public static ChessMove CreateMoveOnly(BoardLocation from, BoardLocation to) => new ChessMove(@from, to, ChessMoveType.MoveOnly);
        public static ChessMove CreateTakeOnly(BoardLocation from, BoardLocation to) => new ChessMove(@from, to, ChessMoveType.TakeOnly);
        public static ChessMove CreatePawnPromotion(BoardLocation from, BoardLocation to, ChessPieceName promotionPiece) => new ChessMove(@from, to, promotionPiece);

        public static int DirectionModifierFor(Colours player) => player == Colours.White ? +1 : -1;
        public static int EndRankFor(Colours player)
            => player == Colours.White ? 8 : 1;

        public static ChessMove Create(string from, string to, ChessMoveType moveType)
            => Create(BoardLocation.At(from), BoardLocation.At(to), moveType);
        public static ChessMove Create(BoardLocation from, BoardLocation to, ChessMoveType moveType)
            => new ChessMove(from, to, moveType);

        public BoardLocation From { get; }
        public BoardLocation To { get; }
        public ChessMoveType ChessMoveType { get; }

        public ChessPieceName PromotionPiece { get; }
        public ChessMove(BoardLocation from, BoardLocation to, ChessPieceName promotionPiece) : this(from, to,
            ChessMoveType.UpdatePiece)
        {
            PromotionPiece = promotionPiece;
            
        }

        public ChessMove(BoardLocation from, BoardLocation to, ChessMoveType chessMoveType)
        {
            From = from;
            To = to;
            ChessMoveType = chessMoveType;
        }

        #region Equality & ToString()

        protected bool Equals(ChessMove other)
        {
            return Equals(From, other.From)
                   && Equals(To, other.To)
//                   && (
//                       ChessMoveType != ChessMoveType.PawnPromotion 
//                       || Equals(PromotionPiece, other.PromotionPiece)
//                       )
                   && ChessMoveType.Equals(other.ChessMoveType);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ChessMove)obj);
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
            return $"{From}{To}{ChessMoveType}";
        }

        #endregion

    }
}