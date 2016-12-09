using System.Collections.Generic;
using System.Linq;
using CSharpChess.TheBoard;

namespace CSharpChess.ValidMoves
{
    public abstract class ValidMoveGeneratorBase
    {
        private Chess.PieceNames ForPiece { get; }

        public abstract IEnumerable<ChessMove> Moves(ChessBoard board, BoardLocation at);
        public abstract IEnumerable<ChessMove> Takes(ChessBoard board, BoardLocation at);
        public abstract IEnumerable<ChessMove> Covers(ChessBoard board, BoardLocation at);
        protected virtual IEnumerable<BoardLocation> Threats(ChessBoard board, BoardLocation at)
        {
            var threats = new List<BoardLocation>();
            threats.AddRange(Moves(board, at).Select(m => m.To));
            threats.AddRange(Takes(board, at).Select(m => m.To));
            threats.AddRange(Covers(board, at).Select(m => m.To));
            return threats;
        }

        protected ValidMoveGeneratorBase(Chess.PieceNames forPiece)
        {
            ForPiece = forPiece;
        }

        public IEnumerable<ChessMove> ValidMoves(ChessBoard board, string location) =>
            ValidMoves(board, (BoardLocation)location);

        public IEnumerable<ChessMove> ValidMoves(ChessBoard board, BoardLocation at)
        {
            var possibleMoves = new List<ChessMove>();
            var chessPiece = board[at].Piece;

            if (chessPiece.Colour == Chess.Colours.None
                || chessPiece.Name != ForPiece)
                    return possibleMoves;

            AddMoves(board, at, possibleMoves);

            return possibleMoves;
        }

        public IEnumerable<BoardLocation> ValidThreats(ChessBoard board, BoardLocation at) 
            => Threats(board, at);

        private void AddMoves(ChessBoard board, BoardLocation at, List<ChessMove> possibleMoves)
        {
            possibleMoves.AddRange(Moves(board, at));

            possibleMoves.AddRange(Takes(board, at));
        }
    }
}