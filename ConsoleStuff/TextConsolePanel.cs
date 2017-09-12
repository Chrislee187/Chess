using System;
using System.Linq;
using ConsoleStuff.Panels;
using CSharpChess.Extensions;

namespace ConsoleStuff
{
    public class TextConsolePanel : ConsolePanel
    {
        public TextConsolePanel(string text) : this(text, text.Length, null)
        { }
        public TextConsolePanel(string text, int width) : this(text, width, null)
        { }
        public TextConsolePanel(string text, ConsoleCellColour errorTextColour) : this(text, -1, errorTextColour)
        { }

        public TextConsolePanel(string text, int width, ConsoleCellColour errorTextColour = null) : base(text.Length, 1)
        {
            var lines = text.ToLines(StringSplitOptions.RemoveEmptyEntries).ToList();

            Height = lines.Count();
            Width = width == -1 
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