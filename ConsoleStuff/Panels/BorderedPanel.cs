using CSharpChess.System.Extensions;

namespace ConsoleStuff.Panels
{
    public class BorderedPanel : ConsolePanel
    {

        public BorderedPanel(int width, int height, char vertex = '+', char hedge ='-', char vedge = '|')
        {
            Width = width;
            Height = height;
            var outerRow = $"{vertex}{hedge.ToString().Repeat(width - 2)}{vertex}";
            var innerRow = $"{vedge}{" ".Repeat(width - 2)}{vedge}";

            PrintAt(1, 1, outerRow);
            for (var i = 2; i <= height - 1; i++)
            {
                PrintAt(1, i, innerRow);
            }
            PrintAt(1, height, outerRow);
        }
//        public BorderedPanel(int width, int height, bool gfxMode = true)
//            : base(width, height)
//        {
//            // TODO: Add option to use '╚', '╝', '╔', '╗','║', '═', '╬'
//            var topRow = $"╔{"═".Repeat(width - 2)}╗";
//            var bottomRow = $"╚{"═".Repeat(width - 2)}╝";
//            var innerRow = $"║{" ".Repeat(width - 2)}║";
//
//            PrintAt(1, 1, topRow);
//            for (var i = 2; i <= height - 1; i++)
//            {
//                    PrintAt(1, i, innerRow);
//            }
//            PrintAt(1, height, bottomRow);
//        }
    }
}