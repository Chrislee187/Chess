using System.Collections.Generic;
using CSharpChess.TheBoard;

namespace CSharpChess.ValidMoves
{
    public abstract class ValidMoveGeneratorBase
    {
        private Chess.PieceNames ForPiece { get; }

        protected abstract IEnumerable<ChessMove> Moves(ChessBoard board, BoardLocation at);
        protected abstract IEnumerable<ChessMove> Takes(ChessBoard board, BoardLocation at);

        protected ValidMoveGeneratorBase(Chess.PieceNames forPiece)
        {
            ForPiece = forPiece;
        }

        public IEnumerable<ChessMove> For(ChessBoard board, string location) =>
            For(board, (BoardLocation)location);

        public IEnumerable<ChessMove> For(ChessBoard board, BoardLocation at)
        {
            var possibleMoves = new List<ChessMove>();
            var chessPiece = board[at].Piece;

            if (chessPiece.Colour == Chess.Colours.None) return possibleMoves;
            if (chessPiece.Name != ForPiece) return possibleMoves;

            AddMoves(board, at, possibleMoves);

            return possibleMoves;
        }

        private void AddMoves(ChessBoard board, BoardLocation at, List<ChessMove> possibleMoves)
        {
            possibleMoves.AddRange(Moves(board, at));

            possibleMoves.AddRange(Takes(board, at));
        }
    }
}