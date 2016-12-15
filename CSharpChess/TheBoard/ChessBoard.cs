using System;
using System.Collections.Generic;
using System.Linq;
using CSharpChess.Extensions;
using CSharpChess.ValidMoves;

namespace CSharpChess.TheBoard
{
    public class ChessBoard
    {
        private readonly BoardPiece[,] _boardPieces = new BoardPiece[9,9];
        public Chess.Board.Colours WhoseTurn { get; set; }

        public ChessBoard(bool newGame = true)
        {
            if (newGame)
            {
                NewBoard();
                WhoseTurn = Chess.Board.Colours.White;
            }
            else
            {
                EmptyBoard();
                WhoseTurn = Chess.Board.Colours.None;
            }

            MoveHandler = new MoveHandler(this);
        }

        public ChessBoard(IEnumerable<BoardPiece> pieces, Chess.Board.Colours whoseTurn)
        {
            EmptyBoard();
            foreach (var boardPiece in pieces)
            {
                this[boardPiece.Location] = boardPiece;
            }
            WhoseTurn = whoseTurn;
            MoveHandler = new MoveHandler(this);
        }

        public MoveResult Move(string move) => Move((ChessMove)move);
        public MoveResult Move(ChessMove move)
        {
            var boardPiece = this[move.From];

            if (boardPiece.Piece.Colour != WhoseTurn && WhoseTurn != Chess.Board.Colours.None)
                return MoveResult.IncorrectPlayer(move);

            var validMove = CheckMoveIsValid(move);
            if (validMove != null)
            {
                return MoveHandler.Move(move, validMove, boardPiece);
            }

            throw new ArgumentException($"Invalid move {move}", nameof(move));
        }

        /// <summary>
        /// Using available MoveGenerators return the moves available to the piece at the specified the location
        /// </summary>
        /// <param name="at"></param>
        /// <returns></returns>
        public IEnumerable<ChessMove> MovesFor(BoardLocation at)
        {
            return this[at].AllMoves.Where(this.MoveDoesNotPutOwnKingInCheck);
        }

        public void MovePiece(ChessMove move, MoveType moveType)
        {
            var piece = this[move.From];
            ClearSquare(move.From);
            piece.MoveTo(move.To, moveType);
            this[move.To] = piece;
        }

        public void ClearSquare(BoardLocation takenLocation) => this[takenLocation] = BoardPiece.Empty(takenLocation);

        private ChessMove CheckMoveIsValid(ChessMove move) 
            => this[move.From].AllMoves.FirstOrDefault(vm => vm.Equals(move));

        private void NewBoard()
        {
            _boardPieces[(int)Chess.Board.ChessFile.A, 8] = new BoardPiece(1, 8, Chess.Pieces.Black.Rook);
            _boardPieces[(int)Chess.Board.ChessFile.B, 8] = new BoardPiece(2, 8, Chess.Pieces.Black.Knight);
            _boardPieces[(int)Chess.Board.ChessFile.C, 8] = new BoardPiece(3, 8, Chess.Pieces.Black.Bishop);
            _boardPieces[(int)Chess.Board.ChessFile.D, 8] = new BoardPiece(4, 8, Chess.Pieces.Black.Queen);
            _boardPieces[(int)Chess.Board.ChessFile.E, 8] = new BoardPiece(5, 8, Chess.Pieces.Black.King);
            _boardPieces[(int)Chess.Board.ChessFile.F, 8] = new BoardPiece(6, 8, Chess.Pieces.Black.Bishop);
            _boardPieces[(int)Chess.Board.ChessFile.G, 8] = new BoardPiece(7, 8, Chess.Pieces.Black.Knight);
            _boardPieces[(int)Chess.Board.ChessFile.H, 8] = new BoardPiece(8, 8, Chess.Pieces.Black.Rook);

            _boardPieces[(int)Chess.Board.ChessFile.A, 7] = new BoardPiece(1, 7, Chess.Pieces.Black.Pawn);
            _boardPieces[(int)Chess.Board.ChessFile.B, 7] = new BoardPiece(2, 7, Chess.Pieces.Black.Pawn);
            _boardPieces[(int)Chess.Board.ChessFile.C, 7] = new BoardPiece(3, 7, Chess.Pieces.Black.Pawn);
            _boardPieces[(int)Chess.Board.ChessFile.D, 7] = new BoardPiece(4, 7, Chess.Pieces.Black.Pawn);
            _boardPieces[(int)Chess.Board.ChessFile.E, 7] = new BoardPiece(5, 7, Chess.Pieces.Black.Pawn);
            _boardPieces[(int)Chess.Board.ChessFile.F, 7] = new BoardPiece(6, 7, Chess.Pieces.Black.Pawn);
            _boardPieces[(int)Chess.Board.ChessFile.G, 7] = new BoardPiece(7, 7, Chess.Pieces.Black.Pawn);
            _boardPieces[(int)Chess.Board.ChessFile.H, 7] = new BoardPiece(8, 7, Chess.Pieces.Black.Pawn);

            for (int rank = 3; rank < 7; rank++)
            {
                _boardPieces[(int)Chess.Board.ChessFile.A, rank] = new BoardPiece(1, rank, ChessPiece.NullPiece);
                _boardPieces[(int)Chess.Board.ChessFile.B, rank] = new BoardPiece(2, rank, ChessPiece.NullPiece);
                _boardPieces[(int)Chess.Board.ChessFile.C, rank] = new BoardPiece(3, rank, ChessPiece.NullPiece);
                _boardPieces[(int)Chess.Board.ChessFile.D, rank] = new BoardPiece(4, rank, ChessPiece.NullPiece);
                _boardPieces[(int)Chess.Board.ChessFile.E, rank] = new BoardPiece(5, rank, ChessPiece.NullPiece);
                _boardPieces[(int)Chess.Board.ChessFile.F, rank] = new BoardPiece(6, rank, ChessPiece.NullPiece);
                _boardPieces[(int)Chess.Board.ChessFile.G, rank] = new BoardPiece(7, rank, ChessPiece.NullPiece);
                _boardPieces[(int)Chess.Board.ChessFile.H, rank] = new BoardPiece(8, rank, ChessPiece.NullPiece);
            }

            _boardPieces[(int)Chess.Board.ChessFile.A, 2] = new BoardPiece(1, 2, Chess.Pieces.White.Pawn);
            _boardPieces[(int)Chess.Board.ChessFile.B, 2] = new BoardPiece(2, 2, Chess.Pieces.White.Pawn);
            _boardPieces[(int)Chess.Board.ChessFile.C, 2] = new BoardPiece(3, 2, Chess.Pieces.White.Pawn);
            _boardPieces[(int)Chess.Board.ChessFile.D, 2] = new BoardPiece(4, 2, Chess.Pieces.White.Pawn);
            _boardPieces[(int)Chess.Board.ChessFile.E, 2] = new BoardPiece(5, 2, Chess.Pieces.White.Pawn);
            _boardPieces[(int)Chess.Board.ChessFile.F, 2] = new BoardPiece(6, 2, Chess.Pieces.White.Pawn);
            _boardPieces[(int)Chess.Board.ChessFile.G, 2] = new BoardPiece(7, 2, Chess.Pieces.White.Pawn);
            _boardPieces[(int)Chess.Board.ChessFile.H, 2] = new BoardPiece(8, 2, Chess.Pieces.White.Pawn);

            _boardPieces[(int)Chess.Board.ChessFile.A, 1] = new BoardPiece(1, 1, Chess.Pieces.White.Rook);
            _boardPieces[(int)Chess.Board.ChessFile.B, 1] = new BoardPiece(2, 1, Chess.Pieces.White.Knight);
            _boardPieces[(int)Chess.Board.ChessFile.C, 1] = new BoardPiece(3, 1, Chess.Pieces.White.Bishop);
            _boardPieces[(int)Chess.Board.ChessFile.D, 1] = new BoardPiece(4, 1, Chess.Pieces.White.Queen);
            _boardPieces[(int)Chess.Board.ChessFile.E, 1] = new BoardPiece(5, 1, Chess.Pieces.White.King);
            _boardPieces[(int)Chess.Board.ChessFile.F, 1] = new BoardPiece(6, 1, Chess.Pieces.White.Bishop);
            _boardPieces[(int)Chess.Board.ChessFile.G, 1] = new BoardPiece(7, 1, Chess.Pieces.White.Knight);
            _boardPieces[(int)Chess.Board.ChessFile.H, 1] = new BoardPiece(8, 1, Chess.Pieces.White.Rook);
        }
        private void EmptyBoard()
        {
            foreach (var rank in Chess.Board.Ranks)
            {
                foreach (var file in Chess.Board.Files)
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

        #region this[] and other basic public stuff
        // ReSharper disable once MemberCanBePrivate.Global
        public BoardPiece this[int file, int rank]
        {
            get { return GetPiece((Chess.Board.ChessFile)file, rank); }
            internal set { _boardPieces[file, rank] = value; }
        }
        public BoardPiece this[Chess.Board.ChessFile file, int rank]
        {
            get { return this[(int)file, rank]; }
            internal set { this[(int)file, rank] = value; }
        }
        public BoardPiece this[BoardLocation location]
        {
            get { return this[location.File, location.Rank]; }
            internal set { this[location.File, location.Rank] = value; }
        }
        public BoardPiece this[string location]
        {
            get { return this[(BoardLocation)location]; }
            // ReSharper disable once UnusedMember.Local
            internal set { this[(BoardLocation)location] = value; }
        }
        private BoardPiece GetPiece(Chess.Board.ChessFile file, int rank)
        {
            Chess.Validations.ThrowInvalidRank(rank);
            Chess.Validations.ThrowInvalidFile(file);
            return _boardPieces[(int)file, rank];
        }

        public IEnumerable<BoardPiece> Pieces
        {
            get
            {
                foreach (var rank in Chess.Board.Ranks)
                {
                    foreach (var file in Chess.Board.Files)
                    {
                        if (this[file, rank].Piece.Name != Chess.Board.PieceNames.Blank)
                            yield return this[file, rank];
                    }
                }
            }
            set { throw new NotImplementedException(); }
        }

        internal MoveHandler MoveHandler { get; }

        public ChessBoard ShallowClone()
            => new ChessBoard(Pieces.Select(bp => bp.Clone()), WhoseTurn);
        #endregion

        /// <summary>
        /// Special internal access to the move method to allow playing moves out
        /// quickly, typically on cloned boards to calculate something post-move
        /// </summary>
        /// <param name="move"></param>
        internal void MovePiece(ChessMove move)
        {
            MovePiece(move, move.MoveType);
        }
    }
}