using System.Collections.Generic;

namespace CSharpChess.TheBoard
{
    public class BoardPiece
    {
        public static BoardPiece Empty(BoardLocation fromLocation) => new BoardPiece(fromLocation, ChessPiece.NullPiece);

        private readonly List<ChessMove> _moveHistory = new List<ChessMove>();

        public IEnumerable<ChessMove> MoveHistory { get { return _moveHistory; } }

        public BoardLocation Location { get; set; }

        public ChessPiece Piece { get; }

        public BoardPiece(int file, int rank, ChessPiece chessPiece)
        {
            Location = new BoardLocation((Chess.ChessFile) file, rank);
            Piece = chessPiece;
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

        public void MoveTo(BoardLocation moveTo, MoveType type)
        {
            _moveHistory.Add(new ChessMove(Location, moveTo, type));
            Location = moveTo;
        }

    }
}