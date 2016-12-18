using System.Linq;
using CSharpChess.MoveGeneration;
using CSharpChess.System.Extensions;
using CSharpChess.System.Metrics;

namespace CSharpChess.TheBoard
{
    // TODO: Unit-test against different move types
    public class MoveHandler
    {
        private readonly ChessBoard _chessBoard;

        public MoveHandler(ChessBoard chessBoard)
        {
            _chessBoard = chessBoard;
            RebuildMoveLists();
        }

        public MoveResult Move(ChessMove move, ChessMove validMove, BoardPiece boardPiece)
        {
            move.UpdateUnknownMoveType(validMove.MoveType);

            PreMoveActions(move);

            MovePiece(move);

            var movePerformed = PostMoveActions(move, boardPiece);

            RebuildMoveLists();

            return movePerformed;
        }

        internal void MovePiece(ChessMove move)
        {
            var piece = _chessBoard[move.From];
            _chessBoard.ClearSquare(move.From);
            piece.MoveTo(move.To, move.MoveType);
            _chessBoard[move.To] = piece;

            RebuildMoveLists();
        }

        private void RebuildMoveLists()
        {
            Counters.Increment(CounterIds.Board.MovelistRebuild);
            Timers.Time("board-creation.rebuild-movelists", () =>
            {
                foreach (var boardPiece in _chessBoard.Pieces)
                {
                    RebuildMoveListFor(boardPiece);
                }
            });
        }

        private void RebuildMoveListFor(BoardPiece boardPiece)
        {
            var all = MoveFactory.For[boardPiece.Piece.Name]().All(_chessBoard, boardPiece.Location);
            boardPiece.SetAll(all.ToList());
        }

        private void PreMoveActions(ChessMove move)
        {
            switch (move.MoveType)
            {
                case MoveType.Take:
                    TakeSquare(move.To);
                    break;
                case MoveType.Promotion:
                    if (_chessBoard.IsNotEmptyAt(move.To)) TakeSquare(move.To);
                    break;
                case MoveType.Castle:
                    var rookMove = Chess.Rules.King.CreateRookMoveForCastling(move);
                    MovePiece(rookMove);
                    break;
            }
        }

        private MoveResult PostMoveActions(ChessMove move, BoardPiece boardPiece)
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
            _chessBoard[takenLocation].Taken(takenLocation);
            _chessBoard.ClearSquare(takenLocation);
        }

        private void Promote(BoardLocation at, Chess.Colours colour, Chess.PieceNames pieceName) 
            => _chessBoard[at] = new BoardPiece(at, new ChessPiece(colour, pieceName));

        private static MoveType DefaultMoveType(MoveType moveType, MoveType @default) 
            => moveType == MoveType.Unknown ? @default : moveType;

        private void NextTurn()
        {
            if (_chessBoard.WhoseTurn == Chess.Colours.White) _chessBoard.WhoseTurn = Chess.Colours.Black;
            else if(_chessBoard.WhoseTurn == Chess.Colours.Black) _chessBoard.WhoseTurn = Chess.Colours.White;
        }
    }
}