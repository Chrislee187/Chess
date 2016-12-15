using System.Collections.Generic;
using System.Linq;
using CSharpChess.Extensions;

namespace CSharpChess.TheBoard
{
    public class BoardPiece
    {
        public static BoardPiece Empty(BoardLocation fromLocation) => new BoardPiece(fromLocation, ChessPiece.NullPiece);

        private IEnumerable<ChessMove> _moves;

        private readonly List<ChessMove> _moveHistory = new List<ChessMove>();
        public IEnumerable<ChessMove> MoveHistory => _moveHistory;

        public BoardLocation Location { get; private set; }
        public ChessPiece Piece { get; }

        public BoardPiece(int file, int rank, ChessPiece chessPiece)
            : this (new BoardLocation((Chess.Board.ChessFile) file, rank), chessPiece)
        {}
        public BoardPiece(Chess.Board.ChessFile file, int rank, ChessPiece chessPiece)
            : this(new BoardLocation(file, rank), chessPiece)
        {}

        public BoardPiece(BoardLocation location, ChessPiece piece)
        {
            Location = location;
            Piece = piece;

            if(piece.Name != Chess.PieceNames.Blank)
            {
            }
        }

        internal void MoveTo(BoardLocation moveTo, MoveType type)
        {
            _moveHistory.Add(new ChessMove(Location, moveTo, type));
            Location = moveTo;
        }

        internal void Taken(BoardLocation takenLocation)
        {
            _moveHistory.Add(ChessMove.Taken(takenLocation));
        }

        internal IEnumerable<ChessMove> PossibleMoves => _moves.Where(m => !m.MoveType.IsCover());

        public override string ToString()
        {
            return $"{Piece} @ {Location}";
        }

        internal void SetAll(IEnumerable<ChessMove> moves)
        {
            _moves = moves;
        }

        public BoardPiece Clone()
        {
            return new BoardPiece(Location.File, Location.Rank, Piece.Clone());
        }
    }
}