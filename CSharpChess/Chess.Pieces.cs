using CSharpChess.TheBoard;

namespace CSharpChess
{
    public static partial class Chess
    {
        public static class Pieces
        {
            public static readonly ChessPiece Blank = ChessPiece.NullPiece;

            public static class White
            {
                public static readonly ChessPiece Pawn = new ChessPiece(Chess.Colours.White, Chess.PieceNames.Pawn);
                public static readonly ChessPiece Bishop = new ChessPiece(Chess.Colours.White, Chess.PieceNames.Bishop);
                public static readonly ChessPiece Knight = new ChessPiece(Chess.Colours.White, Chess.PieceNames.Knight);
                public static readonly ChessPiece Rook = new ChessPiece(Chess.Colours.White, Chess.PieceNames.Rook);
                public static readonly ChessPiece King = new ChessPiece(Chess.Colours.White, Chess.PieceNames.King);
                public static readonly ChessPiece Queen = new ChessPiece(Chess.Colours.White, Chess.PieceNames.Queen);
            }
            public static class Black
            {
                public static readonly ChessPiece Pawn = new ChessPiece(Chess.Colours.Black, Chess.PieceNames.Pawn);
                public static readonly ChessPiece Bishop = new ChessPiece(Chess.Colours.Black, Chess.PieceNames.Bishop);
                public static readonly ChessPiece Knight = new ChessPiece(Chess.Colours.Black, Chess.PieceNames.Knight);
                public static readonly ChessPiece Rook = new ChessPiece(Chess.Colours.Black, Chess.PieceNames.Rook);
                public static readonly ChessPiece King = new ChessPiece(Chess.Colours.Black, Chess.PieceNames.King);
                public static readonly ChessPiece Queen = new ChessPiece(Chess.Colours.Black, Chess.PieceNames.Queen);
            }
        }
    }
}