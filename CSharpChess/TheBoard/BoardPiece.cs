using System;
using System.Collections.Generic;
using CSharpChess.ValidMoves;

namespace CSharpChess.TheBoard
{
    public class BoardPiece
    {
        public static BoardPiece Empty(BoardLocation fromLocation) => new BoardPiece(fromLocation, ChessPiece.NullPiece);

        private readonly List<ChessMove> _moveHistory = new List<ChessMove>();
        private readonly Func<IMoveGenerator> _moveGenerator;
        private readonly MoveFactory _moveFactory = new MoveFactory();
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

            if(piece.Name != Chess.Board.PieceNames.Blank)
            {
                _moveGenerator = _moveFactory.For[piece.Name];
            }
        }

        public void MoveTo(BoardLocation moveTo, MoveType type)
        {
            _moveHistory.Add(new ChessMove(Location, moveTo, type));
            Location = moveTo;
        }

        public void Taken(BoardLocation takenLocation)
        {
            _moveHistory.Add(ChessMove.Taken(takenLocation));
        }

        public IEnumerable<ChessMove> Moves(ChessBoard board) => _moveGenerator().Moves(board, Location);
        public IEnumerable<ChessMove> Covers(ChessBoard board) => _moveGenerator().Covers(board, Location);
        public IEnumerable<ChessMove> Takes(ChessBoard board) => _moveGenerator().Takes(board, Location);


        public override string ToString()
        {
            return $"{Piece} @ {Location}";
        }
    }
}