using CSharpChess.System.Extensions;
using CSharpChess.TheBoard;

// ReSharper disable MemberCanBePrivate.Global

namespace CSharpChess.System
{
    public static class Board
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
            return (int) (piece.Colour == Chess.Colours.White
                ? DirectionModifiers.UpBoardDirectionModifer
                : piece.Colour == Chess.Colours.Black
                    ? DirectionModifiers.DownBoardDirectionModifer : DirectionModifiers.NoDirectionModifier);

        }

        public static bool NotOnEdge(BoardLocation at, DirectionModifiers horizontal)
        {
            var notOnHorizontalEdge = horizontal > 0
                ? at.File < Chess.ChessFile.H
                : at.File > Chess.ChessFile.A;
            return notOnHorizontalEdge;
        }
    }
}