using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharpChess.TheBoard
{
    public class ChessBoard
    {
        private readonly BoardPiece[,] _boardPieces = new BoardPiece[9,9];
        private readonly ValidMoveFactory _validMoveFactory = new ValidMoveFactory();
        private Chess.Board.Colours ToPlay { get; set; } = Chess.Board.Colours.None;

        public IEnumerable<BoardPiece> Pieces
        {
            get
            {
                foreach (var rank in Chess.Board.Ranks)
                {
                    foreach (var file in Chess.Board.Files)
                    {
                        yield return this[file, rank];
                    }
                }
            }
        }
        [Obsolete("Stop using it!!!!!!!!", false)]
        public IEnumerable<BoardRank> Ranks
        {
            get
            {
                foreach (var rank in Chess.Board.Ranks)
                {
                    yield return new BoardRank(rank, Rank(rank).ToArray());
                }
            }
        }
        public IEnumerable<BoardFile> Files
        {
            get
            {
                foreach (var file in Chess.Board.Files)
                {
                    yield return new BoardFile(file, File(file).ToArray());
                }
            }
        }

        // ReSharper disable once MemberCanBePrivate.Global
        public BoardPiece this[int file, int rank]
        {
            get { return GetPiece((Chess.Board.ChessFile)file, rank); }
            private set { _boardPieces[file, rank] = value; }
        }
        public BoardPiece this[Chess.Board.ChessFile file, int rank]
        {
            get { return this[(int)file, rank]; }
            private set { this[(int)file, rank] = value; }
        }
        public BoardPiece this[BoardLocation location]
        {
            get { return this[location.File, location.Rank]; }
            private set { this[location.File, location.Rank] = value; }
        }
        public BoardPiece this[string location]
        {
            get { return this[(BoardLocation) location]; }
            // ReSharper disable once UnusedMember.Local
            private set { this[(BoardLocation) location] = value; }
        }

        public ChessBoard(bool newGame = true)
        {
            if (newGame)
            {
                NewBoard();
                ToPlay = Chess.Board.Colours.White;
            }
            else
                EmptyBoard();
        }

        public ChessBoard(IEnumerable<BoardPiece> pieces, Chess.Board.Colours toPlay)
        {
            EmptyBoard();
            foreach (var boardPiece in pieces)
            {
                this[boardPiece.Location] = boardPiece;
            }
            ToPlay = toPlay;
        }

        public MoveResult Move(string move) => Move((ChessMove)move);

        public MoveResult Move(ChessMove move)
        {
            var boardPiece = this[move.From];

            if (boardPiece.Piece.Colour != ToPlay && ToPlay != Chess.Board.Colours.None)
                return MoveResult.IncorrectPlayer(move);

            var validMove = CheckMoveIsValid(move);
            if (validMove != null)
            {
                var moveType = IfUnknownMoveType(move.MoveType, validMove.MoveType);

                PreMoveActions(move, moveType);

                MovePiece(move, moveType);

                return PostMoveTidyUp(move, moveType, boardPiece);
            }

            throw new ArgumentException($"Invalid move {move}", nameof(move));
        }

        public bool CanTakeAt(BoardLocation takeLocation, Chess.Board.Colours attackerColour)
            => IsNotEmptyAt(takeLocation)
            && this[takeLocation].Piece.Colour == Chess.ColourOfEnemy(attackerColour);

        public bool IsCoveringAt(BoardLocation coverLocation, Chess.Board.Colours attackerColour)
            => IsNotEmptyAt(coverLocation)
            && this[coverLocation].Piece.Colour == attackerColour;

        private void PreMoveActions(ChessMove move, MoveType moveType)
        {
            switch (moveType)
            {
                case MoveType.Take:
                    TakeSquare(move.To);
                    break;
                case MoveType.Promotion:
                    if (IsNotEmptyAt(move.To)) TakeSquare(move.To);
                    break;
                case MoveType.Castle:
                    var rookMove = Chess.Rules.KingAndQueen.CreateRookMoveForCastling(move);
                    MovePiece(rookMove, MoveType.Castle);
                    break;
            }
        }

        private void TakeSquare(BoardLocation takenLocation)
        {
            this[takenLocation].Taken(takenLocation);
            ClearSquare(takenLocation);
        }

        private void Promote(BoardLocation at, Chess.Board.Colours colour, Chess.Board.PieceNames pieceName)
        {
            this[at] = new BoardPiece(at, new ChessPiece(colour, pieceName));
        }

        private void MovePiece(ChessMove move, MoveType moveType)
        {
            var piece = this[move.From];
            ClearSquare(move.From);
            piece.MoveTo(move.To, moveType);
            this[move.To] = piece;
        }

        private MoveResult PostMoveTidyUp(ChessMove move, MoveType moveType, BoardPiece boardPiece)
        {
            MoveResult result;
            switch (moveType)
            {
                case MoveType.TakeEnPassant:
                    var takenLocation = new BoardLocation(move.To.File, move.From.Rank);
                    TakeSquare(takenLocation);
                    result = UpdateTurn(MoveResult.Enpassant(move));
                    break;
                case MoveType.Promotion:
                    Promote(move.To, boardPiece.Piece.Colour, move.PromotedTo);
                    result = UpdateTurn(MoveResult.Promotion(move));
                    break;
                default:
                    result = UpdateTurn(MoveResult.Success(move, moveType));
                    break;
            }
            return result;
        }

        private static MoveType IfUnknownMoveType(MoveType moveType, MoveType @default) 
            => moveType == MoveType.Unknown 
                ? @default 
                : moveType;

        private ChessMove CheckMoveIsValid(ChessMove move)
        {
            var validMove = _validMoveFactory.GetValidMoves(this, move.From)
                                .FirstOrDefault(vm => vm.Equals(move));
            return validMove;
        }

        private void ClearSquare(BoardLocation takenLocation)
        {
            this[takenLocation] = BoardPiece.Empty(takenLocation);
        }

        public bool IsEmptyAt(BoardLocation location) 
            => this[location].Piece.Equals(ChessPiece.NullPiece);

        public bool IsNotEmptyAt(BoardLocation location) 
            => !IsEmptyAt(location);

        public bool IsEmptyAt(string location)
            => this[(BoardLocation) location].Piece.Equals(ChessPiece.NullPiece);

        public bool IsNotEmptyAt(string location)
            => !IsEmptyAt((BoardLocation) location);

        private IEnumerable<BoardPiece> Rank(int rank)
        {
            Chess.Validations.ThrowInvalidRank(rank);
            foreach (var file in Chess.Board.Files)
            {
                yield return this[file, rank];
            }
        }

        private IEnumerable<BoardPiece> File(Chess.Board.ChessFile file)
        {
            Chess.Validations.ThrowInvalidFile(file);
            foreach (var rank in Chess.Board.Ranks)
            {
                yield return this[file, rank];
            }
        }

        private MoveResult UpdateTurn(MoveResult result)
        {
            if (ToPlay == Chess.Board.Colours.White) ToPlay = Chess.Board.Colours.Black;
            else if(ToPlay == Chess.Board.Colours.Black) ToPlay = Chess.Board.Colours.White;

            return result;
        }

        private BoardPiece GetPiece(Chess.Board.ChessFile file, int rank)
        {
            Chess.Validations.ThrowInvalidRank(rank);
            Chess.Validations.ThrowInvalidFile(file);
            return _boardPieces[(int)file, rank];
        }

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
    }

}