using System;

namespace Chess.Common.Tests.Panels
{
    public class ConsoleCellColour
    {
        public static ConsoleCellColour FromConsole = new ConsoleCellColour(Console.ForegroundColor, Console.BackgroundColor);

        public ConsoleColor Background { get; }
        public ConsoleColor Foreground { get; }
        public ConsoleCellColour(ConsoleColor foreground, ConsoleColor background)
        {
            Foreground = foreground;
            Background = background;
        }
    }
}