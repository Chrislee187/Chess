using System.Linq;
using CSharpChess.MoveGeneration;
using CSharpChess.System.Extensions;
using CSharpChess.System.Metrics;

namespace CSharpChess.TheBoard
{
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
            var moveType = DefaultMoveType(move.MoveType, validMove.MoveType);

            PreMoveActions(move, moveType);

            _chessBoard.MovePiece(move, moveType);

            var movePerformed = PostMoveActions(move, moveType, boardPiece);

            RebuildMoveLists();

            return movePerformed;
        }

        internal void RebuildMoveLists()
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

        private void PreMoveActions(ChessMove move, MoveType moveType)
        {
            switch (moveType)
            {
                case MoveType.Take:
                    TakeSquare(move.To);
                    break;
                case MoveType.Promotion:
                    if (_chessBoard.IsNotEmptyAt(move.To)) TakeSquare(move.To);
                    break;
                case MoveType.Castle:
                    var rookMove = Chess.Rules.King.CreateRookMoveForCastling(move);
                    _chessBoard.MovePiece(rookMove, MoveType.Castle);
                    break;
            }
        }

        private MoveResult PostMoveActions(ChessMove move, MoveType moveType, BoardPiece boardPiece)
        {
            MoveResult result;
            switch (moveType)
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
                    result = MoveResult.Success(move, moveType);
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