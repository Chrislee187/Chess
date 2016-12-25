using System;

namespace ConsoleStuff
{
    public class ConsoleColour : IDisposable
    {
        private readonly ConsoleColor _back;
        private readonly ConsoleColor _fore;


        public ConsoleColour(ConsoleColor background, ConsoleColor foreground)
        {
            _back = Console.BackgroundColor;
            _fore = Console.ForegroundColor;

            Console.BackgroundColor = background;
            Console.ForegroundColor = foreground;
        }

        public void Dispose()
        {
            Console.ForegroundColor = _fore;
            Console.BackgroundColor = _back;
        }
    }
}