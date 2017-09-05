using System;
using System.Linq;
using CSharpChess.System.Extensions;

namespace CSharpChess.TheBoard
{
    public class ChessMove
    {
        public static ChessMove Null => new ChessMove();

        private ChessMove()
        {
            From = To = null;
            MoveType = MoveType.Unknown;
            PromotedTo = Chess.PieceNames.Blank;
        }

        private ChessMove(string from, string to, MoveType moveType,
            Chess.PieceNames promotedTo = Chess.PieceNames.Blank) : this((BoardLocation) from, (BoardLocation) to, moveType, promotedTo)
        {
        }

        public ChessMove(BoardLocation from, BoardLocation to, MoveType moveType, Chess.PieceNames promotedTo = Chess.PieceNames.Blank)
        {
            From = from;
            To = to;
            MoveType = moveType;
            PromotedTo = promotedTo;
        }

        public BoardLocation From { get; }
        public BoardLocation To { get; }
        public MoveType MoveType { get; private set; }
        public Chess.PieceNames PromotedTo { get; }
        public static ChessMove Taken(BoardLocation location) => new ChessMove(location, MoveType.Taken);

        private ChessMove(BoardLocation location, MoveType taken)
        {
            From = location;
            MoveType = taken;
        }

        public override string ToString()
        {
            var promotion = PromotedTo != Chess.PieceNames.Blank && PromotedTo != Chess.PieceNames.Pawn ? $"={PromotedTo.ToString().First()}" : "";
            return $"{From}-{To}{promotion}";
        }

        #region object overrides
        public static explicit operator ChessMove(string move)
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
                return new ChessMove(from, to, moveType);
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
            if(promotedTo != Chess.PieceNames.Blank)
                moveType = MoveType.Promotion;

            return new ChessMove(from, to, moveType, promotedTo);
        }

        private static Chess.PieceNames GetPromotionPiece(string piece)
        {
            switch (piece.ToUpper())
            {
                case "R": return Chess.PieceNames.Rook;
                case "B": return Chess.PieceNames.Bishop;
                case "N": return Chess.PieceNames.Knight;
                case "Q": return Chess.PieceNames.Queen;
            }

            throw new ArgumentException($"'{piece}' is not a valid promotion", nameof(piece));

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
            if (obj.GetType() != GetType()) return false;
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

        internal void UpdateUnknownMoveType(MoveType moveType)
        {
            MoveType = MoveType == MoveType.Unknown ? moveType : MoveType;
        }

        public static ChessMove Create(BoardLocation from, BoardLocation to) 
            => new ChessMove(@from, to, MoveType.Unknown);
    }
}