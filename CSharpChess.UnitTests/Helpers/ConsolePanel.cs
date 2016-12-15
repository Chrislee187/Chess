using System;
using System.Collections.Generic;
using System.Linq;
// ReSharper disable UnusedParameter.Local

namespace CSharpChess.UnitTests.Helpers
{
    public class ConsolePanel
    {
        private int Width { get; }
        private int Height { get; }
        private readonly char[,] _panel;

        public ConsolePanel(int width, int height)
        {
            Width = width;
            Height = height;
            _panel = new char[width, height];
        }

        // ReSharper disable once UnusedMethodReturnValue.Global
        public ConsolePanel PrintAt(int x, int y, char c)
        {
            CheckXY(x, y);

            _panel[x - 1, y - 1] = c;

            return this;
        }


        // ReSharper disable once UnusedMethodReturnValue.Global
        public ConsolePanel PrintAt(int x, int y, string s)
        {
            for (int x1 = 0; x1 < s.Length; x1++)
            {
                int newX = x + x1;
                CheckXY(newX, y);
                _panel[newX - 1, y - 1] = s[x1];
            }

            return this;
        }

        private void CheckXY(int x, int y)
        {
            if (x < 1 || y < 1)
                throw new ArgumentException($"Panel co-ordinates are one-based.");

            if (x > Width || y > Height)
                throw new ArgumentException($"Panel co-ordinates are out of bounds.");
        }

        // ReSharper disable once UnusedMethodReturnValue.Global
        public ConsolePanel PrintAt(int panelX, int panelY, ConsolePanel panel)
        {
            for (var y = 0; y < panel.Height; y++)
            {
                for (var x = 0; x < panel.Width; x++)
                {
//                    _panel[(panelX-1) + x, (panelY-1) + y] = panel[x, y];
                    PrintAt((panelX) + x, (panelY) + y, panel[x, y]);
                }
            }

            return this;
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

        public void Fill(char c)
        {
            TopLeftToBottomRight().ToList().ForEach(t => _panel[t.Item1 - 1, t.Item2 - 1] = c);
        }

        public string[] ToStrings()
        {
            var s = new List<string>();

            for (var y = 1; y <= Height; y++)
            {
                var c = new List<char>();
                for (var x = 1; x <= Width; x++)
                {
                    var item = _panel[x - 1, y - 1];
                    c.Add(item == '\0' ? ' ' : item);
                }
                s.Add(new string(c.ToArray()));
            }
            return s.ToArray();
        }

        protected char this[int x, int y] => _panel[x, y];

        public override string ToString()
        {
            return string.Join("\n", ToStrings());
        }
    }
}