using System;
using ConsoleStuff.Panels;

namespace ConsoleStuff
{
    public class ChangeConsoleColour : IDisposable
    {
        private ConsoleColor _back;
        private ConsoleColor _fore;

        private bool _colourChanged;

        public ChangeConsoleColour(ConsoleColor background, ConsoleColor foreground)
        {
            ChangeColour(background, foreground);
        }

        public ChangeConsoleColour(ConsoleCellColour fromConsole)
        {

            if (fromConsole != null)
            {
                ChangeColour(fromConsole.Background, fromConsole.Foreground);
            }
        }

        private void ChangeColour(ConsoleColor background, ConsoleColor foreground)
        {
            if (background != Console.BackgroundColor)
            {
                _colourChanged = true;
                _back = Console.BackgroundColor;
                Console.BackgroundColor = background;
            }
            if (foreground != Console.ForegroundColor)
            {
                _colourChanged = true;
                _fore = Console.ForegroundColor;
                Console.ForegroundColor = foreground;
            }
        }

        public void Dispose()
        {
            if (_colourChanged)
            {
                Console.ForegroundColor = _fore;
                Console.BackgroundColor = _back;
                _colourChanged = false;
            }
        }
    }
}