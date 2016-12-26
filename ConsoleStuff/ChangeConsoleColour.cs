using System;

namespace ConsoleStuff
{
    public class ChangeConsoleColour : IDisposable
    {
        private ConsoleColor _back;
        private ConsoleColor _fore;

        private bool _colourChanged = false;

        public ChangeConsoleColour(ConsoleColor background, ConsoleColor foreground)
        {
            ChangeColour(background, foreground);
        }

        public ChangeConsoleColour(ConsolePanel.ConsolePanel.ConsoleCellColour fromConsole)
        {

            if (fromConsole != null)
            {
                ChangeColour(fromConsole.Background, fromConsole.Foreground);
            }
        }

        private void ChangeColour(ConsoleColor background, ConsoleColor foreground)
        {
            _colourChanged = true;
            _back = Console.BackgroundColor;
            _fore = Console.ForegroundColor;

            Console.BackgroundColor = background;
            Console.ForegroundColor = foreground;
        }

        public void Dispose()
        {
            if (_colourChanged)
            {
                Console.ForegroundColor = _fore;
                Console.BackgroundColor = _back;
            }
        }
    }
}