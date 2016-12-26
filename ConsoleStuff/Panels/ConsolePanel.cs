using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable UnusedParameter.Local

namespace ConsoleStuff.Panels
{
    // ReSharper disable UnusedMethodReturnValue.Global
    public class ConsolePanel
    {
        public int Width { get; protected set; }
        public int Height { get; protected set; }
        private char[,] _cells;
        private ConsoleCellColour[,] _consoleCellColours;

        private char this[int x, int y] => _cells[x, y];

        protected ConsolePanel()
        {
        }
        public ConsolePanel(int width, int height)
        {
            Width = width;
            Height = height;
            InitialisePanel();
        }

        public ConsolePanel PrintAt(int x, int y, char c, ConsoleCellColour colour = null)
        {
            CheckXY(x, y);
            _cells[x - 1, y - 1] = c;
            if (colour != null)
            {
                _consoleCellColours[x - 1, y - 1] = colour;
            }
            return this;
        }

        public ConsolePanel PrintAt(int x, int y, string s, ConsoleCellColour colour = null)
        {
            var text = s;
            if (x + s.Length - 1> Width)
            {
                text = s.Substring(0, Width - x);
            }
            for (var x1 = 0; x1 < text.Length; x1++)
            {
                var newX = x + x1;
                CheckXY(newX, y);
                PrintAt(newX, y, s[x1], colour);
            }

            return this;
        }

        public ConsolePanel PrintAt(int x, int y, ConsolePanel panel, ConsoleCellColour colour = null)
        {
            for (var panelY = 0; panelY < panel.Height; panelY++)
            {
                for (var panelX = 0; panelX < panel.Width; panelX++)
                {
                    var colourOverride = colour ?? panel.GetCellColour(panelX + 1, panelY + 1);

                    PrintAt((x) + panelX, (y) + panelY, panel[panelX, panelY], colourOverride);
                }
            }

            return this;
        }

        public void Fill(char c, ConsoleCellColour consoleCellColour = null)
        {
            TopLeftToBottomRight().ToList().ForEach(t =>
            {
                PrintAt(t.Item1, t.Item2, c, consoleCellColour);
            });
        }

        public string[] ToStrings()
        {
            var s = new List<string>();

            for (var y = 1; y <= Height; y++)
            {
                var c = new List<char>();
                for (var x = 1; x <= Width; x++)
                {
                    var item = _cells[x - 1, y - 1];
                    c.Add(item == '\0' ? ' ' : item);
                }
                s.Add(new string(c.ToArray()));
            }
            return s.ToArray();
        }
        public override string ToString()
        {
            return string.Join("\n", ToStrings());
        }

        private void CheckXY(int x, int y)
        {
            if (x < 1 || y < 1)
                throw new ArgumentException($"Panel co-ordinates are one-based.");

            if (x > Width || y > Height)
                throw new ArgumentException($"Panel co-ordinates are out of bounds.");
        }

        private IEnumerable<Tuple<int, int>> TopLeftToBottomRight()
        {
            for (var y = 1; y <= Height; y++)
            {
                for (var x = 1; x <= Width; x++)
                {
                    yield return new Tuple<int, int>(x, y);
                }
            }
        }

        protected void InitialisePanel()
        {
            _cells = new char[Width, Height];
            _consoleCellColours = new ConsoleCellColour[Width, Height];
        }

        public Action ToColouredConsole()
        {
            var cells = new List<Action>();

            for (var y = 1; y <= Height; y++)
            {
                for (var x = 1; x <= Width; x++)
                {
                    var x1 = x;
                    var y1 = y;
                    Action writeAction = () =>
                    {
                        var c = GetOutputCharacter(x1, y1);
                        var colours = GetCellColour(x1, y1);
                        using (new ChangeConsoleColour(colours))
                        {
                            Console.Write(c);
                        }
                    };
                    cells.Add(writeAction);
                }
                cells.Add(Console.WriteLine);
            }
            return () => cells.ForEach(c => c());
        }

        private static void SetConsoleColours(ConsoleCellColour colours)
        {
            Console.ForegroundColor = colours.Foreground;
            Console.BackgroundColor = colours.Background;
        }

        private ConsoleCellColour GetCellColour(int x, int y)
        {
            var x1 = x - 1;
            var y1 = y - 1;

            return _consoleCellColours[x1, y1];
        }

        private char GetOutputCharacter(int x, int y)
        {
            var item = _cells[x - 1, y - 1];
            return item == '\0' ? ' ' : item;
        }
    }
}