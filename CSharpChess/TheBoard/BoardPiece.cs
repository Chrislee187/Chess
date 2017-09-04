using System.Collections.Generic;
using System.Linq;
using CSharpChess.System.Extensions;

namespace CSharpChess.TheBoard
{
    public class BoardPiece
    {
        public static BoardPiece Empty(BoardLocation fromLocation) => new BoardPiece(fromLocation, ChessPiece.NullPiece);
        public IEnumerable<ChessMove> MoveHistory => _moveHistory;
        public BoardLocation Location { get; private set; }
        public ChessPiece Piece { get; }

        private readonly List<ChessMove> _moveHistory = new List<ChessMove>();
        private IEnumerable<ChessMove> _moves;

        public BoardPiece(int file, int rank, ChessPiece chessPiece)
            : this(new BoardLocation((Chess.ChessFile) file, rank), chessPiece)
        {
        }

        public BoardPiece(Chess.ChessFile file, int rank, ChessPiece chessPiece)
            : this(new BoardLocation(file, rank), chessPiece)
        {
        }

        public BoardPiece(BoardLocation location, ChessPiece piece)
        {
            Location = location;
            Piece = piece;
        }

        public IEnumerable<ChessMove> PossibleMoves => _moves.Where(m => !m.MoveType.IsCover());

        internal void MoveTo(BoardLocation moveTo, MoveType type)
        {
            _moveHistory.Add(new ChessMove(Location, moveTo, type));
            Location = moveTo;
        }
        internal void Taken(BoardLocation takenLocation)
        {
            _moveHistory.Add(ChessMove.Taken(takenLocation));
        }
        internal void SetAll(IEnumerable<ChessMove> moves)
        {
            _moves = moves;
        }

        public override string ToString()
        {
            return $"{Piece} @ {Location}";
        }

        public BoardPiece Clone()
        {
            return new BoardPiece(Location.File, Location.Rank, Piece.Clone());
        }

        #region Equality

        protected bool Equals(BoardPiece other)
        {
            return _moves.SequenceEqual(other._moves) && Equals(Location, other.Location) && Equals(Piece, other.Piece);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((BoardPiece) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = _moves?.GetHashCode() ?? 0;
                hashCode = (hashCode * 397) ^ (Location?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ (Piece?.GetHashCode() ?? 0);
                return hashCode;
            }
        }

        #endregion
    }
}