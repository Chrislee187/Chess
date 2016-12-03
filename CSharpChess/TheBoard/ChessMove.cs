using System;
using System.Linq;

namespace CSharpChess.TheBoard
{
    public class ChessMove
    {
        public ChessMove(string from, string to, MoveType moveType,
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
        public MoveType MoveType { get; }
        public Chess.PieceNames PromotedTo { get; }
        public override string ToString() => $"{From}-{To}";


        #region object overrides
        // TODO: Change to explicit
        public static explicit operator ChessMove(string move)
        {
            var moveType = MoveType.Unknown;
            Chess.PieceNames promotedTo = Chess.PieceNames.Blank;
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

            promotedTo = GetPromotionPiece(left);
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