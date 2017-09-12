using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSharpChess.Extensions;
using CSharpChess.Movement;
using CSharpChess.System;
using CSharpChess.System.Metrics;

namespace CSharpChess
{
    public class Board
    {
        public List<Move> Moves { get; } = new List<Move>();
        private readonly BoardPiece[,] _boardPieces = new BoardPiece[9, 9];
        public Colours WhoseTurn { get; set; }

        private readonly bool _constructing;
        public Board(bool newGame = true)
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
        public Board(IEnumerable<BoardPiece> pieces, Colours whoseTurn)
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
                GameState = GameState.Unknown;
                WhoseTurn = Colours.None;
                MoveHandler = new MoveHandler(this);
            });
        }
        private void InitialiseNewGameBoard()
        {
            Timers.Time(TimerIds.Board.New, () =>
            {
                NewBoard();
                GameState = GameState.WaitingForMove;
                WhoseTurn = Colours.White;
                MoveHandler = new MoveHandler(this);
            });
        }
        private void InitialiseCustomBoard(IEnumerable<BoardPiece> pieces, Colours whoseTurn)
        {
            Timers.Time(TimerIds.Board.Custom, () =>
            {
                EmptyBoard();
                foreach (var boardPiece in pieces)
                {
                    this[boardPiece.Location] = boardPiece;
                }
                GameState = GameState.WaitingForMove;

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
                    GameState = defender == Colours.Black
                        ? GameState.BlackKingInCheck
                        : GameState.WhiteKingInCheck;

                    CheckForCheckMate(defender);
                }
            }
        }

        /// <summary>
        /// WARNING: If called during object construction this will recurse indefinetly as it tries
        /// to generate the move lists to see if any kings are in check
        /// </summary>
        /// <param name="defender"></param>
        private void CheckForCheckMate(Colours defender)
        {
            if (!_constructing)
            {
                var c = ShallowClone();
                if (c.Pieces.OfColour(defender).SelectMany(o => RemoveMovesThatLeaveBoardInCheck(o.Location)).None())
                {
                    GameState = defender == Colours.Black
                        ? GameState.CheckMateWhiteWins
                        : GameState.CheckMateBlackWins;
                }
            }
        }

        public MoveResult Move(string move) => Move((Move)move);
        public MoveResult Move(Move move)
        {
            ThrowIfGameOver();
            MoveResult result = null;
            SetEngineState(EngineState.Moving, () =>
            {
                var boardPiece = this[move.From];

                if (boardPiece.Piece.Colour != WhoseTurn && WhoseTurn != Colours.None)
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
                        GameState = GameState.WaitingForMove;
                        CheckForCheck();
                        result = moveResult;
                    }
                }
            });
            return result ?? MoveResult.Failure($"Invalid move {move}", move);
        }

        private void RecordMove(Move move) => Moves.Add(move);

        public IEnumerable<Move> RemoveMovesThatLeaveBoardInCheck(BoardLocation at)
        {
            IEnumerable<Move> result = null;
            SetEngineState(EngineState.GeneratingPieceMoves, () =>
            {
                result = this[at].PossibleMoves
                    .Where(m => !Validations.MovesLeaveOwnSideInCheck(this, m));
            });
            return result ?? new List<Move>();
        }

        public void ClearSquare(BoardLocation takenLocation) 
            => this[takenLocation] = BoardPiece.Empty(takenLocation);

        private Move CheckMoveIsValid(Move move)
            => RemoveMovesThatLeaveBoardInCheck(move.From).Where(m => !m.MoveType.IsCover()).FirstOrDefault(vm => vm.Equals(move));

        #region this[] and other basic public stuff
        // ReSharper disable once MemberCanBePrivate.Global
        public BoardPiece this[int file, int rank]
        {
            get { return GetPiece((ChessFile)file, rank); }
            internal set { _boardPieces[file, rank] = value; }
        }
        public BoardPiece this[ChessFile file, int rank]
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
        private BoardPiece GetPiece(ChessFile file, int rank)
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
                        if (this[file, rank].Piece.Name != PieceNames.Blank)
                            yield return this[file, rank];
                    }
                }
            }
            set { throw new NotImplementedException(); }
        }

        private MoveHandler MoveHandler { get; set; }
        public GameState GameState { get; private set; }

        public Board ShallowClone()
            => new Board(Pieces.Select(bp => bp.Clone()), WhoseTurn);

        private void ThrowIfGameOver()
        {
            if (GameState == GameState.CheckMateBlackWins || GameState == GameState.CheckMateWhiteWins)
                throw new Exception("Game over moves no longer accepted.");
        }

        private void NewBoard()
        {
            _boardPieces[(int)ChessFile.A, 8] = new BoardPiece(1, 8, PiecesFactory.Black.Rook);
            _boardPieces[(int)ChessFile.B, 8] = new BoardPiece(2, 8, PiecesFactory.Black.Knight);
            _boardPieces[(int)ChessFile.C, 8] = new BoardPiece(3, 8, PiecesFactory.Black.Bishop);
            _boardPieces[(int)ChessFile.D, 8] = new BoardPiece(4, 8, PiecesFactory.Black.Queen);
            _boardPieces[(int)ChessFile.E, 8] = new BoardPiece(5, 8, PiecesFactory.Black.King);
            _boardPieces[(int)ChessFile.F, 8] = new BoardPiece(6, 8, PiecesFactory.Black.Bishop);
            _boardPieces[(int)ChessFile.G, 8] = new BoardPiece(7, 8, PiecesFactory.Black.Knight);
            _boardPieces[(int)ChessFile.H, 8] = new BoardPiece(8, 8, PiecesFactory.Black.Rook);

            _boardPieces[(int)ChessFile.A, 7] = new BoardPiece(1, 7, PiecesFactory.Black.Pawn);
            _boardPieces[(int)ChessFile.B, 7] = new BoardPiece(2, 7, PiecesFactory.Black.Pawn);
            _boardPieces[(int)ChessFile.C, 7] = new BoardPiece(3, 7, PiecesFactory.Black.Pawn);
            _boardPieces[(int)ChessFile.D, 7] = new BoardPiece(4, 7, PiecesFactory.Black.Pawn);
            _boardPieces[(int)ChessFile.E, 7] = new BoardPiece(5, 7, PiecesFactory.Black.Pawn);
            _boardPieces[(int)ChessFile.F, 7] = new BoardPiece(6, 7, PiecesFactory.Black.Pawn);
            _boardPieces[(int)ChessFile.G, 7] = new BoardPiece(7, 7, PiecesFactory.Black.Pawn);
            _boardPieces[(int)ChessFile.H, 7] = new BoardPiece(8, 7, PiecesFactory.Black.Pawn);

            for (int rank = 3; rank < 7; rank++)
            {
                _boardPieces[(int)ChessFile.A, rank] = new BoardPiece(1, rank, ChessPiece.NullPiece);
                _boardPieces[(int)ChessFile.B, rank] = new BoardPiece(2, rank, ChessPiece.NullPiece);
                _boardPieces[(int)ChessFile.C, rank] = new BoardPiece(3, rank, ChessPiece.NullPiece);
                _boardPieces[(int)ChessFile.D, rank] = new BoardPiece(4, rank, ChessPiece.NullPiece);
                _boardPieces[(int)ChessFile.E, rank] = new BoardPiece(5, rank, ChessPiece.NullPiece);
                _boardPieces[(int)ChessFile.F, rank] = new BoardPiece(6, rank, ChessPiece.NullPiece);
                _boardPieces[(int)ChessFile.G, rank] = new BoardPiece(7, rank, ChessPiece.NullPiece);
                _boardPieces[(int)ChessFile.H, rank] = new BoardPiece(8, rank, ChessPiece.NullPiece);
            }

            _boardPieces[(int)ChessFile.A, 2] = new BoardPiece(1, 2, PiecesFactory.White.Pawn);
            _boardPieces[(int)ChessFile.B, 2] = new BoardPiece(2, 2, PiecesFactory.White.Pawn);
            _boardPieces[(int)ChessFile.C, 2] = new BoardPiece(3, 2, PiecesFactory.White.Pawn);
            _boardPieces[(int)ChessFile.D, 2] = new BoardPiece(4, 2, PiecesFactory.White.Pawn);
            _boardPieces[(int)ChessFile.E, 2] = new BoardPiece(5, 2, PiecesFactory.White.Pawn);
            _boardPieces[(int)ChessFile.F, 2] = new BoardPiece(6, 2, PiecesFactory.White.Pawn);
            _boardPieces[(int)ChessFile.G, 2] = new BoardPiece(7, 2, PiecesFactory.White.Pawn);
            _boardPieces[(int)ChessFile.H, 2] = new BoardPiece(8, 2, PiecesFactory.White.Pawn);

            _boardPieces[(int)ChessFile.A, 1] = new BoardPiece(1, 1, PiecesFactory.White.Rook);
            _boardPieces[(int)ChessFile.B, 1] = new BoardPiece(2, 1, PiecesFactory.White.Knight);
            _boardPieces[(int)ChessFile.C, 1] = new BoardPiece(3, 1, PiecesFactory.White.Bishop);
            _boardPieces[(int)ChessFile.D, 1] = new BoardPiece(4, 1, PiecesFactory.White.Queen);
            _boardPieces[(int)ChessFile.E, 1] = new BoardPiece(5, 1, PiecesFactory.White.King);
            _boardPieces[(int)ChessFile.F, 1] = new BoardPiece(6, 1, PiecesFactory.White.Bishop);
            _boardPieces[(int)ChessFile.G, 1] = new BoardPiece(7, 1, PiecesFactory.White.Knight);
            _boardPieces[(int)ChessFile.H, 1] = new BoardPiece(8, 1, PiecesFactory.White.Rook);
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
            if (this.GetKingFor(Colours.Black) == null)
                throw new InvalidBoardStateException("Black king not found", this);

            if (this.GetKingFor(Colours.White) == null)
                throw new InvalidBoardStateException("White king not found", this);
        }

        #endregion

        /// <summary>
        /// Special internal access to the move method to allow playing moves out
        /// quickly, typically on cloned boards to calculate something post-move
        /// </summary>
        /// <param name="move"></param>
        internal void MovePiece(Move move) => MoveHandler.QuickMovePiece(move);

        private static void BoardCreatedCounter() => Counters.Increment(CounterIds.Board.Created);

        private static readonly IDictionary<PieceNames, char> AsciiPieceNames = new Dictionary<PieceNames, char>
        {
            {PieceNames.Pawn   ,'P' },
            {PieceNames.Knight ,'N' },
            {PieceNames.Bishop ,'B' },
            {PieceNames.Rook   ,'R' },
            {PieceNames.Queen  ,'Q' },
            {PieceNames.King   ,'K' }
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

                    if (piece.Piece.Colour == Colours.Black)
                    {
                        ascii = char.ToLower(ascii);
                    }

                    sb.Append(ascii);
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }
        

    public static class Rules
    {
        public enum DirectionModifiers 
        {
            LeftDirectionModifier = -1,
            RightDirectionModifier = 1,
            UpBoardDirectionModifer = 1,
            DownBoardDirectionModifer = -1,
            NoDirectionModifier = 0
        }

        // TODO: Unit Test these
        public static int ForwardDirectionModifierFor(ChessPiece piece)
        {
            return (int) (piece.Colour == Colours.White
                ? DirectionModifiers.UpBoardDirectionModifer
                : piece.Colour == Colours.Black
                    ? DirectionModifiers.DownBoardDirectionModifer : DirectionModifiers.NoDirectionModifier);

        }

        public static bool NotOnEdge(BoardLocation at, DirectionModifiers horizontal)
        {
            var notOnHorizontalEdge = horizontal > 0
                ? at.File < ChessFile.H
                : at.File > ChessFile.A;
            return notOnHorizontalEdge;
        }
    }
    }
}