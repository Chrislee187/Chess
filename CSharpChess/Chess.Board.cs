using System.Collections.Generic;
using System.Linq;
using CSharpChess.System.Extensions;
using CSharpChess.TheBoard;
// ReSharper disable MemberCanBePrivate.Global

namespace CSharpChess
{
    public static partial class Chess
    {
        public static partial class Board
        {
            public enum ChessFile { A = 1, B, C, D, E, F, G, H };
            public static IEnumerable<ChessFile> Files => new List<ChessFile> {ChessFile.A, ChessFile.B, ChessFile.C, ChessFile.D, ChessFile.E, ChessFile.F, ChessFile.G, ChessFile.H};
            public static IEnumerable<int> Ranks => new [] {1,2,3,4,5,6,7,8};

            public enum DirectionModifiers 
            {
                LeftDirectionModifier = -1,
                RightDirectionModifier = 1,
                UpBoardDirectionModifer = 1,
                DownBoardDirectionModifer = -1,
                NoDirectionModifier = 0
            }

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