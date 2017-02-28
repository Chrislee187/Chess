using System;
using System.IO;
using System.Text;
using CSharpChess.System.Extensions;

namespace CsChess.Pgn
{
    public class PgnReader : IDisposable
    {
        private readonly StreamReader _reader;

        public PgnReader(Stream stream)
        {
            _reader = new StreamReader(stream);
        }
        public PgnReader(string pgnText)
        {
            _reader = new StreamReader(pgnText.ToStream());
        }

        public string ReadGame()
        {
            var nextLine = SkipEmptyLines();
            if (nextLine == null) return null; // End of stream?

            var tagPairText = ReadUntilEmptyLine(nextLine);
            if(tagPairText == null) throw new InvalidDataException($"Expected Tag Pairs, found EOF!");

            nextLine = SkipEmptyLines();
            if (nextLine == null) throw new InvalidDataException($"No move text found.");

            var moveText = ReadUntilEmptyLine(nextLine);

            return tagPairText + "\n" + moveText;
        }

        private string ReadUntilEmptyLine(string nextLine)
        {
            var line = ReadLine();
            var sb = new StringBuilder(nextLine);
            while (!string.IsNullOrEmpty(line))
            {
                sb.AppendLine(line);
                line = ReadLine();
            }

            return sb.ToString();
        }

        private string ReadLine()
        {
            return _reader.ReadLine()?.Trim();
        }

        private string SkipEmptyLines()
        {
            var line = ReadLine();
            if (line == null) return null;

            while (line == string.Empty)
            {
                line = ReadLine();
            }

            return line;
        }

        public void Dispose()
        {
            _reader?.Dispose();
        }
    }
}