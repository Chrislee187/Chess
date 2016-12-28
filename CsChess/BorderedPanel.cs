using ConsoleStuff.Panels;
using CSharpChess.System.Extensions;

namespace CsChess
{
    internal class BorderedPanel : ConsolePanel
    {
        public BorderedPanel(int width, int height, char vertex = '+', char hedge ='-', char vedge = '|')
            :base(width, height)
        {
            var outer_row = $"{vertex}{hedge.ToString().Repeat(width - 2)}{vertex}";
            var inner_row = $"{vedge}{" ".Repeat(width - 2)}{vedge}";

            PrintAt(1, 1, outer_row);
            for (int i = 2; i <= height - 1; i++)
            {
                PrintAt(1, i, inner_row);
            }
            PrintAt(1, height, outer_row);
        }
    }
}