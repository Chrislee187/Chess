using System.Collections.Generic;
using System.Linq;

namespace CSharpChess.TheBoard
{
    public class ChessBoard
    {
        private readonly BoardPiece[,] _boardPieces = new BoardPiece[9,9];

        public ChessBoard(bool newGame)
        {
            if (newGame)
                NewBoard();
            else
                EmptyBoard();
        }

        public ChessBoard(IEnumerable<BoardPiece> pieces)
        {
            EmptyBoard();
            foreach (var boardPiece in pieces)
            {
                _boardPieces[boardPiece.File, boardPiece.Rank] = boardPiece;
            }
        }

        private void NewBoard()
        {
            _boardPieces[(int)Chess.ChessFile.A, 8] = new BoardPiece(1, 1, Chess.Pieces.Black.Rook);
            _boardPieces[(int)Chess.ChessFile.B, 8] = new BoardPiece(1, 1, Chess.Pieces.Black.Knight);
            _boardPieces[(int)Chess.ChessFile.C, 8] = new BoardPiece(1, 1, Chess.Pieces.Black.Bishop);
            _boardPieces[(int)Chess.ChessFile.D, 8] = new BoardPiece(1, 1, Chess.Pieces.Black.Queen);
            _boardPieces[(int)Chess.ChessFile.E, 8] = new BoardPiece(1, 1, Chess.Pieces.Black.King);
            _boardPieces[(int)Chess.ChessFile.F, 8] = new BoardPiece(1, 1, Chess.Pieces.Black.Bishop);
            _boardPieces[(int)Chess.ChessFile.G, 8] = new BoardPiece(1, 1, Chess.Pieces.Black.Knight);
            _boardPieces[(int)Chess.ChessFile.H, 8] = new BoardPiece(1, 1, Chess.Pieces.Black.Rook);

            _boardPieces[(int)Chess.ChessFile.A, 7] = new BoardPiece(1, 1, Chess.Pieces.Black.Pawn);
            _boardPieces[(int)Chess.ChessFile.B, 7] = new BoardPiece(1, 1, Chess.Pieces.Black.Pawn);
            _boardPieces[(int)Chess.ChessFile.C, 7] = new BoardPiece(1, 1, Chess.Pieces.Black.Pawn);
            _boardPieces[(int)Chess.ChessFile.D, 7] = new BoardPiece(1, 1, Chess.Pieces.Black.Pawn);
            _boardPieces[(int)Chess.ChessFile.E, 7] = new BoardPiece(1, 1, Chess.Pieces.Black.Pawn);
            _boardPieces[(int)Chess.ChessFile.F, 7] = new BoardPiece(1, 1, Chess.Pieces.Black.Pawn);
            _boardPieces[(int)Chess.ChessFile.G, 7] = new BoardPiece(1, 1, Chess.Pieces.Black.Pawn);
            _boardPieces[(int)Chess.ChessFile.H, 7] = new BoardPiece(1, 1, Chess.Pieces.Black.Pawn);

            for (int rank = 3; rank < 7; rank++)
            {
                _boardPieces[(int)Chess.ChessFile.A, rank] = new BoardPiece(1, 1, ChessPiece.NullPiece);
                _boardPieces[(int)Chess.ChessFile.B, rank] = new BoardPiece(1, 1, ChessPiece.NullPiece);
                _boardPieces[(int)Chess.ChessFile.C, rank] = new BoardPiece(1, 1, ChessPiece.NullPiece);
                _boardPieces[(int)Chess.ChessFile.D, rank] = new BoardPiece(1, 1, ChessPiece.NullPiece);
                _boardPieces[(int)Chess.ChessFile.E, rank] = new BoardPiece(1, 1, ChessPiece.NullPiece);
                _boardPieces[(int)Chess.ChessFile.F, rank] = new BoardPiece(1, 1, ChessPiece.NullPiece);
                _boardPieces[(int)Chess.ChessFile.G, rank] = new BoardPiece(1, 1, ChessPiece.NullPiece);
                _boardPieces[(int)Chess.ChessFile.H, rank] = new BoardPiece(1, 1, ChessPiece.NullPiece);
            }

            _boardPieces[(int)Chess.ChessFile.A, 2] = new BoardPiece(1, 1, Chess.Pieces.White.Pawn);
            _boardPieces[(int)Chess.ChessFile.B, 2] = new BoardPiece(1, 1, Chess.Pieces.White.Pawn);
            _boardPieces[(int)Chess.ChessFile.C, 2] = new BoardPiece(1, 1, Chess.Pieces.White.Pawn);
            _boardPieces[(int)Chess.ChessFile.D, 2] = new BoardPiece(1, 1, Chess.Pieces.White.Pawn);
            _boardPieces[(int)Chess.ChessFile.E, 2] = new BoardPiece(1, 1, Chess.Pieces.White.Pawn);
            _boardPieces[(int)Chess.ChessFile.F, 2] = new BoardPiece(1, 1, Chess.Pieces.White.Pawn);
            _boardPieces[(int)Chess.ChessFile.G, 2] = new BoardPiece(1, 1, Chess.Pieces.White.Pawn);
            _boardPieces[(int)Chess.ChessFile.H, 2] = new BoardPiece(1, 1, Chess.Pieces.White.Pawn);

            _boardPieces[(int) Chess.ChessFile.A, 1] = new BoardPiece(1,1, Chess.Pieces.White.Rook);
            _boardPieces[(int) Chess.ChessFile.B, 1] = new BoardPiece(1,1, Chess.Pieces.White.Knight);
            _boardPieces[(int) Chess.ChessFile.C, 1] = new BoardPiece(1,1, Chess.Pieces.White.Bishop);
            _boardPieces[(int) Chess.ChessFile.D, 1] = new BoardPiece(1,1, Chess.Pieces.White.Queen);
            _boardPieces[(int) Chess.ChessFile.E, 1] = new BoardPiece(1,1, Chess.Pieces.White.King);
            _boardPieces[(int) Chess.ChessFile.F, 1] = new BoardPiece(1,1, Chess.Pieces.White.Bishop);
            _boardPieces[(int) Chess.ChessFile.G, 1] = new BoardPiece(1,1, Chess.Pieces.White.Knight);
            _boardPieces[(int) Chess.ChessFile.H, 1] = new BoardPiece(1,1, Chess.Pieces.White.Rook);
        }

        private void EmptyBoard()
        {
            foreach (var rank in Chess.Ranks)
            {
                foreach (var file in Chess.Files)
                {
                    if (file != 0 && rank != 0)
                        this[file, rank] = new BoardPiece(file, rank, Chess.Pieces.Blank);
                    else
                    {
                        this[file, rank] = null;
                    }
                }
            }
        }

        public IEnumerable<BoardPiece> Pieces {
            get
            {
                foreach (var file in Chess.Files)
                {
                    foreach (var rank in Chess.Ranks)
                    {
                        yield return this[file, rank];
                    }
                }
            }
        }

        public IEnumerable<BoardRank> Ranks
        {
            get
            {
                foreach (var rank in Chess.Ranks)
                {
                    yield return new BoardRank(rank, Rank((int)rank).ToArray());
                }
            }
        }

        public IEnumerable<BoardPiece> Rank(int rank)
        {
            Chess.Validations.ThrowInvalidRank(rank);
            foreach (var file in Chess.Files)
            {
                yield return this[file, rank];
            }
        }

        public IEnumerable<BoardFile> Files
        {
            get
            {
                foreach (var file in Chess.Files)
                {
                    yield return new BoardFile(file, File(file).ToArray());
                }
            }
        }
        internal IEnumerable<BoardPiece> File(Chess.ChessFile file)
        {
            Chess.Validations.ThrowInvalidFile(file);
            foreach (var rank in Chess.Ranks)
            {
                yield return this[file, rank];
            }
        }

        public BoardPiece this[Chess.ChessFile file, int rank]
        {
            get { return getPiece(file, rank); }
            private set { _boardPieces[(int) file, rank] = value; }
        }

        public BoardPiece this[int file, int rank]
        {
            get { return getPiece((Chess.ChessFile) file, rank); }
            private set { _boardPieces[(int)file, rank] = value; }
        }
        public BoardPiece this[BoardLocation location] => getPiece(location.File, location.Rank);
        private BoardPiece getPiece(Chess.ChessFile file, int rank)
        {
            Chess.Validations.ThrowInvalidRank(rank);
            Chess.Validations.ThrowInvalidFile(file);
            return _boardPieces[(int)file, rank];
        }

        public bool IsEmptyAt(BoardLocation boardLocation)
        {
            return this[boardLocation].Piece.Equals(ChessPiece.NullPiece);
        }
    }
}