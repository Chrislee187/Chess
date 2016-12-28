using System;
using ConsoleStuff.Panels;

namespace CsChess
{
    public class BoardOptions
    {

        public bool ColouredSquares = true;
        public bool ShowRanksAndFiles = true;
        public bool ShowMenu = true;

        // Board Rendering options
        public BoardSize Size = BoardSize.medium;
        public ConsoleCellColour BlackSquareColour = new ConsoleCellColour(ConsoleColor.White, ConsoleColor.Black);
        public ConsoleCellColour WhiteSquareColour = new ConsoleCellColour(ConsoleColor.Black, ConsoleColor.White);

        public int PiecePanelSize => ((int)Size);
        public int BorderedCellSize => PiecePanelSize + 2;
    }

    public enum BoardSize
    {
        // NOTE: Keep these lower case to avoid issues with Enum.Parse
        small = 1, medium = 3, large = 5
    }
}