using System;
using System.Linq;
using CSharpChess.Extensions;
using CSharpChess.Movement;

namespace CSharpChess
{
    public class Move
    {
        public static Move Null => new Move();

        private Move()
        {
            From = To = null;
            MoveType = MoveType.Unknown;
            PromotedTo = PieceNames.Blank;
        }

        private Move(string from, string to, MoveType moveType,
            PieceNames promotedTo = PieceNames.Blank) : this((BoardLocation) @from, (BoardLocation) to, moveType, promotedTo)
        {
        }

        public Move(BoardLocation @from, BoardLocation to, MoveType moveType, PieceNames promotedTo = PieceNames.Blank,
            string pgnText = "")
        {
            From = from;
            To = to;
            MoveType = moveType;
            PromotedTo = promotedTo;
            PgnText = pgnText;
        }

        public BoardLocation From { get; }
        public BoardLocation To { get; }
        public MoveType MoveType { get; private set; }
        public PieceNames PromotedTo { get; }
        public string PgnText { get; }
        public static Move Taken(BoardLocation location) => new Move(location, MoveType.Taken);

        private Move(BoardLocation location, MoveType taken)
        {
            From = location;
            MoveType = taken;
        }

        public override string ToString()
        {
            return $"{From}-{To}{PromotedTo.ToPromotionCharacter()}";
        }

        #region object overrides
        public static explicit operator Move(string move)
        {
            var moveType = MoveType.Unknown;
            const string validMoveChars = "ABCDEFGH12345678";

            var moveU = move.ToUpper();

            var from = moveU.Substring(0, 2);

            var idx = 2;
            if (validMoveChars.All(c => c != moveU[idx]))
            {
                idx++;
            }

            var to = moveU.Substring(idx, 2);
            idx = idx + 2;
            var left = moveU.Substring(idx);

            if (left == string.Empty)
            {
                return new Move(from, to, moveType);
            }

            if (left.First() == '=')
            {
                left = left.Substring(1);

                if (left == string.Empty)
                {
                    throw new ArgumentException($"'{move}' is missing promotion character", nameof(move));
                }
            }

            var promotedTo = GetPromotionPiece(left);
            if(promotedTo != PieceNames.Blank)
                moveType = MoveType.Promotion;

            return new Move(from, to, moveType, promotedTo);
        }

        private static PieceNames GetPromotionPiece(string piece)
        {
            switch (piece.ToUpper())
            {
                case "R": return PieceNames.Rook;
                case "B": return PieceNames.Bishop;
                case "N": return PieceNames.Knight;
                case "Q": return PieceNames.Queen;
            }

            throw new ArgumentException($"'{piece}' is not a valid promotion", nameof(piece));

        }

        // ReSharper disable once MemberCanBePrivate.Global
        protected bool Equals(Move other)
        {
            return Equals(From, other.From) && Equals(To, other.To);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Move) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((From?.GetHashCode() ?? 0)*397) ^ (To?.GetHashCode() ?? 0);
            }
        }
        #endregion

        internal void UpdateUnknownMoveType(MoveType moveType)
        {
            MoveType = MoveType == MoveType.Unknown ? moveType : MoveType;
        }

        public static Move Create(BoardLocation from, BoardLocation to) 
            => new Move(@from, to, MoveType.Unknown);
    }
}