using System;
using ConsoleStuff.Panels;

namespace CsChess
{
    public class BoardOptions
    {
        public BoardSize Size = BoardSize.Medium;

        public bool ColouredSquares = true;
        public bool ShowRanksAndFiles = true;
        public bool ShowMenu = true;

        // Board Rendering options
        public ConsoleCellColour BlackSquareColour = new ConsoleCellColour(ConsoleColor.White, ConsoleColor.Black);
        public ConsoleCellColour WhiteSquareColour = new ConsoleCellColour(ConsoleColor.Black, ConsoleColor.White);
        public int BorderedCellSize => PiecePanelSize + 2;
        public int PiecePanelSize => ((int) Size);
    }

    public enum BoardSize
    {
        Small = 1, Medium = 3, Large = 5
    }
}