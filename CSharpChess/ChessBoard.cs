using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSharpChess.System;
using CSharpChess.System.Extensions;
using CSharpChess.System.Metrics;
using CSharpChess.TheBoard;

namespace CSharpChess
{
    public class ChessBoard
    {
        public List<ChessMove> Moves { get; } = new List<ChessMove>();
        private readonly BoardPiece[,] _boardPieces = new BoardPiece[9, 9];
        public Chess.Colours WhoseTurn { get; set; }

        private readonly bool _constructing;
        public ChessBoard(bool newGame = true)
        {
            BoardCreatedCounter();
            _constructing = true;

            SetEngineState(EngineState.Initialising, () =>
            {
                if (newGame)
                {
                    InitialiseNewGameBoard();
                }
                else
                {
                    InitialiseEmptyBoard();
                }
            });

            _constructing = false;
        }
        public ChessBoard(IEnumerable<BoardPiece> pieces, Chess.Colours whoseTurn)
        {
            BoardCreatedCounter();
            _constructing = true;
            _engineStates.Push(EngineState.Started);

            Timers.Time(TimerIds.Board.Custom, () =>
            {
                SetEngineState(EngineState.Initialising, () =>
                {
                    InitialiseCustomBoard(pieces, whoseTurn);
                });
            });

            _constructing = false;
        }

        private void InitialiseEmptyBoard()
        {
            Timers.Time(TimerIds.Board.Empty, () =>
            {
                EmptyBoard();
                GameState = Chess.GameState.Unknown;
                WhoseTurn = Chess.Colours.None;
                MoveHandler = new MoveHandler(this);
            });
        }
        private void InitialiseNewGameBoard()
        {
            Timers.Time(TimerIds.Board.New, () =>
            {
                NewBoard();
                GameState = Chess.GameState.WaitingForMove;
                WhoseTurn = Chess.Colours.White;
                MoveHandler = new MoveHandler(this);
            });
        }
        private void InitialiseCustomBoard(IEnumerable<BoardPiece> pieces, Chess.Colours whoseTurn)
        {
            Timers.Time(TimerIds.Board.Custom, () =>
            {
                EmptyBoard();
                foreach (var boardPiece in pieces)
                {
                    this[boardPiece.Location] = boardPiece;
                }
                GameState = Chess.GameState.WaitingForMove;

                WhoseTurn = whoseTurn;
                MoveHandler = new MoveHandler(this);
                ValidateInitialBoardState();
                CheckForCheck();
            });
        }

        public EngineState EngineState => _engineStates.Peek();
        private readonly Stack<EngineState> _engineStates = new Stack<EngineState>();
        private void SetEngineState(EngineState state, Action action)
        {
            _engineStates.Push(state);
            try
            {
                action();
            }
            finally
            {
                _engineStates.Pop();
            }
        }
        private void CheckForCheck()
        {
            foreach (var attacker in Chess.BothColours)
            {
                var defender = Chess.ColourOfEnemy(attacker);
                var king = this.GetKingFor(defender);
                if (Pieces.OfColour(attacker).Any(p => ChessMoveListExtensions.ContainsMoveTo(p.PossibleMoves, king.Location)))
                {
                    GameState = defender == Chess.Colours.Black
                        ? Chess.GameState.BlackKingInCheck
                        : Chess.GameState.WhiteKingInCheck;

                    CheckForCheckMate(defender);
                }
            }
        }

        /// <summary>
        /// WARNING: If called during object construction this will recurse indefinetly as it tries
        /// to generate the move lists to see if any kings are in check
        /// </summary>
        /// <param name="defender"></param>
        private void CheckForCheckMate(Chess.Colours defender)
        {
            if (!_constructing)
            {
                var c = ShallowClone();
                if (c.Pieces.OfColour(defender).SelectMany(o => RemoveMovesThatLeaveBoardInCheck(o.Location)).None())
                {
                    GameState = defender == Chess.Colours.Black
                        ? Chess.GameState.CheckMateWhiteWins
                        : Chess.GameState.CheckMateBlackWins;
                }
            }
        }

        public MoveResult Move(string move) => Move((ChessMove)move);
        public MoveResult Move(ChessMove move)
        {
            ThrowIfGameOver();
            MoveResult result = null;
            SetEngineState(EngineState.Moving, () =>
            {
                var boardPiece = this[move.From];

                if (boardPiece.Piece.Colour != WhoseTurn && WhoseTurn != Chess.Colours.None)
                {
                    result = MoveResult.IncorrectPlayer(move);
                    return;
                }

                var validMove = CheckMoveIsValid(move);
                if (validMove != null)
                {
                    var moveResult = MoveHandler.Move(move, validMove, boardPiece);
                    RecordMove(move);
                    if (moveResult.Succeeded)
                    {
                        GameState = Chess.GameState.WaitingForMove;
                        CheckForCheck();
                        result = moveResult;
                    }
                }
            });
            return result ?? MoveResult.Failure($"Invalid move {move}", move);
        }

        private void RecordMove(ChessMove move) => Moves.Add(move);

        public IEnumerable<ChessMove> RemoveMovesThatLeaveBoardInCheck(BoardLocation at)
        {
            IEnumerable<ChessMove> result = null;
            SetEngineState(EngineState.GeneratingPieceMoves, () =>
            {
                result = this[at].PossibleMoves
                    .Where(m => !Validations.MovesLeaveOwnSideInCheck(this, m));
            });
            return result ?? new List<ChessMove>();
        }

        public void ClearSquare(BoardLocation takenLocation) 
            => this[takenLocation] = BoardPiece.Empty(takenLocation);

        private ChessMove CheckMoveIsValid(ChessMove move)
            => RemoveMovesThatLeaveBoardInCheck(move.From).Where(m => !m.MoveType.IsCover()).FirstOrDefault(vm => vm.Equals(move));

        #region this[] and other basic public stuff
        // ReSharper disable once MemberCanBePrivate.Global
        public BoardPiece this[int file, int rank]
        {
            get { return GetPiece((Chess.ChessFile)file, rank); }
            internal set { _boardPieces[file, rank] = value; }
        }
        public BoardPiece this[Chess.ChessFile file, int rank]
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
        private BoardPiece GetPiece(Chess.ChessFile file, int rank)
        {
            Validations.ThrowInvalidRank(rank);
            Validations.ThrowInvalidFile(file);
            return _boardPieces[(int)file, rank];
        }

        public IEnumerable<BoardPiece> Pieces
        {
            get
            {
                foreach (var rank in Chess.Ranks)
                {
                    foreach (var file in Chess.Files)
                    {
                        if (this[file, rank].Piece.Name != Chess.PieceNames.Blank)
                            yield return this[file, rank];
                    }
                }
            }
            set { throw new NotImplementedException(); }
        }

        private MoveHandler MoveHandler { get; set; }
        public Chess.GameState GameState { get; private set; }

        public ChessBoard ShallowClone()
            => new ChessBoard(Pieces.Select(bp => bp.Clone()), WhoseTurn);

        private void ThrowIfGameOver()
        {
            if (GameState == Chess.GameState.CheckMateBlackWins || GameState == Chess.GameState.CheckMateWhiteWins)
                throw new Exception("Game over moves no longer accepted.");
        }

        private void NewBoard()
        {
            _boardPieces[(int)Chess.ChessFile.A, 8] = new BoardPiece(1, 8, PiecesFactory.Black.Rook);
            _boardPieces[(int)Chess.ChessFile.B, 8] = new BoardPiece(2, 8, PiecesFactory.Black.Knight);
            _boardPieces[(int)Chess.ChessFile.C, 8] = new BoardPiece(3, 8, PiecesFactory.Black.Bishop);
            _boardPieces[(int)Chess.ChessFile.D, 8] = new BoardPiece(4, 8, PiecesFactory.Black.Queen);
            _boardPieces[(int)Chess.ChessFile.E, 8] = new BoardPiece(5, 8, PiecesFactory.Black.King);
            _boardPieces[(int)Chess.ChessFile.F, 8] = new BoardPiece(6, 8, PiecesFactory.Black.Bishop);
            _boardPieces[(int)Chess.ChessFile.G, 8] = new BoardPiece(7, 8, PiecesFactory.Black.Knight);
            _boardPieces[(int)Chess.ChessFile.H, 8] = new BoardPiece(8, 8, PiecesFactory.Black.Rook);

            _boardPieces[(int)Chess.ChessFile.A, 7] = new BoardPiece(1, 7, PiecesFactory.Black.Pawn);
            _boardPieces[(int)Chess.ChessFile.B, 7] = new BoardPiece(2, 7, PiecesFactory.Black.Pawn);
            _boardPieces[(int)Chess.ChessFile.C, 7] = new BoardPiece(3, 7, PiecesFactory.Black.Pawn);
            _boardPieces[(int)Chess.ChessFile.D, 7] = new BoardPiece(4, 7, PiecesFactory.Black.Pawn);
            _boardPieces[(int)Chess.ChessFile.E, 7] = new BoardPiece(5, 7, PiecesFactory.Black.Pawn);
            _boardPieces[(int)Chess.ChessFile.F, 7] = new BoardPiece(6, 7, PiecesFactory.Black.Pawn);
            _boardPieces[(int)Chess.ChessFile.G, 7] = new BoardPiece(7, 7, PiecesFactory.Black.Pawn);
            _boardPieces[(int)Chess.ChessFile.H, 7] = new BoardPiece(8, 7, PiecesFactory.Black.Pawn);

            for (int rank = 3; rank < 7; rank++)
            {
                _boardPieces[(int)Chess.ChessFile.A, rank] = new BoardPiece(1, rank, ChessPiece.NullPiece);
                _boardPieces[(int)Chess.ChessFile.B, rank] = new BoardPiece(2, rank, ChessPiece.NullPiece);
                _boardPieces[(int)Chess.ChessFile.C, rank] = new BoardPiece(3, rank, ChessPiece.NullPiece);
                _boardPieces[(int)Chess.ChessFile.D, rank] = new BoardPiece(4, rank, ChessPiece.NullPiece);
                _boardPieces[(int)Chess.ChessFile.E, rank] = new BoardPiece(5, rank, ChessPiece.NullPiece);
                _boardPieces[(int)Chess.ChessFile.F, rank] = new BoardPiece(6, rank, ChessPiece.NullPiece);
                _boardPieces[(int)Chess.ChessFile.G, rank] = new BoardPiece(7, rank, ChessPiece.NullPiece);
                _boardPieces[(int)Chess.ChessFile.H, rank] = new BoardPiece(8, rank, ChessPiece.NullPiece);
            }

            _boardPieces[(int)Chess.ChessFile.A, 2] = new BoardPiece(1, 2, PiecesFactory.White.Pawn);
            _boardPieces[(int)Chess.ChessFile.B, 2] = new BoardPiece(2, 2, PiecesFactory.White.Pawn);
            _boardPieces[(int)Chess.ChessFile.C, 2] = new BoardPiece(3, 2, PiecesFactory.White.Pawn);
            _boardPieces[(int)Chess.ChessFile.D, 2] = new BoardPiece(4, 2, PiecesFactory.White.Pawn);
            _boardPieces[(int)Chess.ChessFile.E, 2] = new BoardPiece(5, 2, PiecesFactory.White.Pawn);
            _boardPieces[(int)Chess.ChessFile.F, 2] = new BoardPiece(6, 2, PiecesFactory.White.Pawn);
            _boardPieces[(int)Chess.ChessFile.G, 2] = new BoardPiece(7, 2, PiecesFactory.White.Pawn);
            _boardPieces[(int)Chess.ChessFile.H, 2] = new BoardPiece(8, 2, PiecesFactory.White.Pawn);

            _boardPieces[(int)Chess.ChessFile.A, 1] = new BoardPiece(1, 1, PiecesFactory.White.Rook);
            _boardPieces[(int)Chess.ChessFile.B, 1] = new BoardPiece(2, 1, PiecesFactory.White.Knight);
            _boardPieces[(int)Chess.ChessFile.C, 1] = new BoardPiece(3, 1, PiecesFactory.White.Bishop);
            _boardPieces[(int)Chess.ChessFile.D, 1] = new BoardPiece(4, 1, PiecesFactory.White.Queen);
            _boardPieces[(int)Chess.ChessFile.E, 1] = new BoardPiece(5, 1, PiecesFactory.White.King);
            _boardPieces[(int)Chess.ChessFile.F, 1] = new BoardPiece(6, 1, PiecesFactory.White.Bishop);
            _boardPieces[(int)Chess.ChessFile.G, 1] = new BoardPiece(7, 1, PiecesFactory.White.Knight);
            _boardPieces[(int)Chess.ChessFile.H, 1] = new BoardPiece(8, 1, PiecesFactory.White.Rook);
        }
        private void EmptyBoard()
        {
            foreach (var rank in Chess.Ranks)
            {
                foreach (var file in Chess.Files)
                {
                    if (file != 0 && rank != 0)
                        this[file, rank] = new BoardPiece(file, rank, PiecesFactory.Blank);
                    else
                    {
                        this[file, rank] = null;
                    }
                }
            }
        }

        private void ValidateInitialBoardState()
        {
            if (this.GetKingFor(Chess.Colours.Black) == null)
                throw new InvalidBoardStateException("Black king not found", this);

            if (this.GetKingFor(Chess.Colours.White) == null)
                throw new InvalidBoardStateException("White king not found", this);
        }

        #endregion

        /// <summary>
        /// Special internal access to the move method to allow playing moves out
        /// quickly, typically on cloned boards to calculate something post-move
        /// </summary>
        /// <param name="move"></param>
        internal void MovePiece(ChessMove move) => MoveHandler.MovePiece(move);

        private static void BoardCreatedCounter() => Counters.Increment(CounterIds.Board.Created);

        private static readonly IDictionary<Chess.PieceNames, char> AsciiPieceNames = new Dictionary<Chess.PieceNames, char>
        {
            {Chess.PieceNames.Pawn   ,'P' },
            {Chess.PieceNames.Knight ,'N' },
            {Chess.PieceNames.Bishop ,'B' },
            {Chess.PieceNames.Rook   ,'R' },
            {Chess.PieceNames.Queen  ,'Q' },
            {Chess.PieceNames.King   ,'K' }
        };


        public string ToAsciiBoard()
        {
            var sb = new StringBuilder();
            foreach (var rank in Chess.Ranks.Reverse())
            {
                foreach (var file in Chess.Files)
                {
                    var piece = this[file, rank];
                    char ascii = AsciiPieceNames.ContainsKey(piece.Piece.Name) ? AsciiPieceNames[piece.Piece.Name] : '.';

                    if (piece.Piece.Colour == Chess.Colours.Black)
                    {
                        ascii = char.ToLower(ascii);
                    }

                    sb.Append(ascii);
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}