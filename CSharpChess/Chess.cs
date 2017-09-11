using System.Collections.Generic;

namespace CSharpChess
{
    public static class Chess
    {
        public static readonly IEnumerable<Colours> BothColours = new[] {Colours.White, Colours.Black};

        public static Colours ColourOfEnemy(Colours colour) 
            => colour == Colours.Black
                ? Colours.White
                : colour == Colours.White
                    ? Colours.Black
                    : colour;

        public static IEnumerable<ChessFile> Files => new List<ChessFile> {ChessFile.A, ChessFile.B, ChessFile.C, ChessFile.D, ChessFile.E, ChessFile.F, ChessFile.G, ChessFile.H};
        public static IEnumerable<int> Ranks => new [] {1,2,3,4,5,6,7,8};
    }
}