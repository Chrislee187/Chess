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
                public static readonly ChessPiece Pawn = new ChessPiece(Colours.White, PieceNames.Pawn);
                public static readonly ChessPiece Bishop = new ChessPiece(Colours.White, PieceNames.Bishop);
                public static readonly ChessPiece Knight = new ChessPiece(Colours.White, PieceNames.Knight);
                public static readonly ChessPiece Rook = new ChessPiece(Colours.White, PieceNames.Rook);
                public static readonly ChessPiece King = new ChessPiece(Colours.White, PieceNames.King);
                public static readonly ChessPiece Queen = new ChessPiece(Colours.White, PieceNames.Queen);
            }
            public static class Black
            {
                public static readonly ChessPiece Pawn = new ChessPiece(Colours.Black, PieceNames.Pawn);
                public static readonly ChessPiece Bishop = new ChessPiece(Colours.Black, PieceNames.Bishop);
                public static readonly ChessPiece Knight = new ChessPiece(Colours.Black, PieceNames.Knight);
                public static readonly ChessPiece Rook = new ChessPiece(Colours.Black, PieceNames.Rook);
                public static readonly ChessPiece King = new ChessPiece(Colours.Black, PieceNames.King);
                public static readonly ChessPiece Queen = new ChessPiece(Colours.Black, PieceNames.Queen);
            }
        }
    }
}