using System;
using System.Linq;
using Chess.Common.Tests.Panels;
using CSharpChess.Extensions;

namespace Chess.Common.Tests
{
    public class TextConsolePanel : ConsolePanel
    {
        private const int LengthOfLongestLine = -1;
        public TextConsolePanel(string text) : this(text, LengthOfLongestLine, null)
        { }
        public TextConsolePanel(string text, int width) : this(text, width, null)
        { }
        public TextConsolePanel(string text, ConsoleCellColour errorTextColour) : this(text, LengthOfLongestLine, errorTextColour)
        { }

        public TextConsolePanel(string text, int width, ConsoleCellColour errorTextColour = null) : base(text.Length, 1)
        {
            var lines = text.ToLines(StringSplitOptions.RemoveEmptyEntries).ToList();

            Height = lines.Count();
            Width = width <= 0
                ? lines.Max(x => x.Length) 
                : width;

            InitialisePanel();

            var y = 1;
                foreach (var line in lines)
                {
                    PrintAt(1, y++, line, errorTextColour);
                }
        }

    }
}