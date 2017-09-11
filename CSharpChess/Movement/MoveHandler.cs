using System.Linq;
using CSharpChess.Extensions;
using CSharpChess.System.Metrics;

namespace CSharpChess.Movement
{
    // TODO: Unit-test against different move types
    public class MoveHandler
    {
        private readonly CSharpChess.Board _board;

        public MoveHandler(CSharpChess.Board board)
        {
            _board = board;
            RebuildMoveLists();
        }

        public MoveResult Move(Move move, Move validMove, BoardPiece boardPiece)
        {
            move.UpdateUnknownMoveType(validMove.MoveType);

            PreMoveActions(move);

            MovePiece(move);

            var movePerformed = PostMoveActions(move, boardPiece);

            RebuildMoveLists();

            return movePerformed;
        }

        internal void MovePiece(Move move)
        {
            var piece = _board[move.From];
            _board.ClearSquare(move.From);
            piece.MoveTo(move.To, move.MoveType);
            _board[move.To] = piece;

            RebuildMoveLists();
        }

        private void RebuildMoveLists()
        {
            Counters.Increment(CounterIds.Board.MovelistRebuildAll);
            Timers.Time(TimerIds.Board.RebuildMoveList, () =>
            {
                foreach (var boardPiece in _board.Pieces)
                {
                    RebuildMoveListFor(boardPiece);
                }
            });
        }

        private void RebuildMoveListFor(BoardPiece boardPiece)
        {
            var all = MoveFactory.For[boardPiece.Piece.Name]().All(_board, boardPiece.Location);
            // TODO: MoveList still contains moves that would uncover check!
            boardPiece.SetAll(all.ToList());
        }

        private void PreMoveActions(Move move)
        {
            switch (move.MoveType)
            {
                case MoveType.Take:
                    TakeSquare(move.To);
                    break;
                case MoveType.Promotion:
                    if (_board.IsNotEmptyAt(move.To)) TakeSquare(move.To);
                    break;
                case MoveType.Castle:
                    var rookMove = King.CreateRookMoveForCastling(move);
                    MovePiece(rookMove);
                    break;
            }
        }

        private MoveResult PostMoveActions(Move move, BoardPiece boardPiece)
        {
            MoveResult result;
            switch (move.MoveType)
            {
                case MoveType.TakeEnPassant:
                    var takenLocation = new BoardLocation(move.To.File, move.From.Rank);
                    TakeSquare(takenLocation);
                    result = MoveResult.Enpassant(move);
                    break;
                case MoveType.Promotion:
                    Promote(move.To, boardPiece.Piece.Colour, move.PromotedTo);
                    result = MoveResult.Promotion(move);
                    break;
                default:
                    result = MoveResult.Success(move);
                    break;
            }
            NextTurn();
            return result;
        }

        private void TakeSquare(BoardLocation takenLocation)
        {
            _board[takenLocation].Taken(takenLocation);
            _board.ClearSquare(takenLocation);
        }

        private void Promote(BoardLocation at, Colours colour, PieceNames pieceName) 
            => _board[at] = new BoardPiece(at, new ChessPiece(colour, pieceName));

        private static MoveType DefaultMoveType(MoveType moveType, MoveType @default) 
            => moveType == MoveType.Unknown ? @default : moveType;

        private void NextTurn()
        {
            if (_board.WhoseTurn == Colours.White) _board.WhoseTurn = Colours.Black;
            else if(_board.WhoseTurn == Colours.Black) _board.WhoseTurn = Colours.White;
        }
    }
}