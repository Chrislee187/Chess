using System.Collections.Generic;
using System.Linq;
using CSharpChess.Extensions;
using CSharpChess.TheBoard;

namespace CSharpChess
{
    public static partial class Chess
    {
        public static partial class Board
        {
            public enum ChessFile { A = 1, B, C, D, E, F, G, H };
            public static IEnumerable<ChessFile> Files => EnumExtensions.GetAll<ChessFile>();

            public static IEnumerable<int> Ranks => Enumerable.Range(1, 8);

            // These need to be an enum
            public const int LeftDirectionModifier = -1;
            public const int RightDirectionModifier = 1;

            public static int ForwardDirectionModifierFor(ChessPiece piece)
            {
                return piece.Colour == Colours.White
                    ? +1
                    : piece.Colour == Colours.Black
                        ? -1 : 0;

            }

            public static IEnumerable<BoardLocation> CastleLocationsBetween(BoardLocation fromLoc, BoardLocation toLoc)
            {
                int fromFile, toFile;
                if (toLoc.File == ChessFile.C)
                {
                    fromFile = (int)ChessFile.C;
                    toFile = (int)ChessFile.D;
                }
                else
                {
                    fromFile = (int)ChessFile.F;
                    toFile = (int)ChessFile.G;
                }

                return Enumerable.Range(fromFile, toFile - fromFile + 1).Select(v => BoardLocation.At(v, fromLoc.Rank));
            }
        }
    }
}