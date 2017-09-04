using System;
using ConsoleStuff.Panels;
// ReSharper disable InconsistentNaming

namespace CsChess
{
    public class Options
    {

        public bool ColouredSquares = true;
        public bool ShowRanksAndFiles = true;
        public bool ShowMenu = true;

        // Board Rendering options
        public BoardSize Size = BoardSize.medium;
        public readonly ConsoleCellColour BlackSquareColour = new ConsoleCellColour(ConsoleColor.White, ConsoleColor.Black);
        public readonly ConsoleCellColour WhiteSquareColour = new ConsoleCellColour(ConsoleColor.Black, ConsoleColor.White);

        public int PiecePanelSize => ((int)Size);
        public int BorderedCellSize => PiecePanelSize + 2;
    }

    public enum BoardSize
    {
        // NOTE: Keep these lower case to avoid issues with Enum.Parse
        // ReSharper disable once UnusedMember.Global
        small = 1, medium = 3, large = 5
    }
}