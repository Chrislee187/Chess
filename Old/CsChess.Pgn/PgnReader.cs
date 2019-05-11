using System;
using System.IO;
using System.Text;
using CSharpChess.Extensions;

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
            var nextLine = ReadUntilNonEmptyLine();
            if (nextLine == null) return null;

            var tagPairText = ReadUntilEmptyLine(nextLine);
            if(tagPairText == null) throw new InvalidDataException($"Expected Tag Pair text, found EOF!");

            nextLine = ReadUntilNonEmptyLine();
            if (nextLine == null) throw new InvalidDataException($"No move text found.");

            var moveText = ReadUntilEmptyLine(nextLine);

            return tagPairText + Environment.NewLine + moveText;
        }

        private string ReadUntilEmptyLine(string nextLine)
        {
            var line = ReadLine();
            var sb = new StringBuilder();
            sb.AppendLine(nextLine);
            while (!string.IsNullOrEmpty(line))
            {
                sb.AppendLine(line);
                line = ReadLine();
            }

            return sb.ToString();
        }

        private string ReadUntilNonEmptyLine()
        {
            var line = ReadLine();
            if (_reader.EndOfStream) return null;

            while (line == string.Empty)
            {
                line = ReadLine();
            }

            return line;
        }

        private string ReadLine()
        {
            return _reader.ReadLine()?.Trim();
        }

        public void Dispose()
        {
            _reader?.Dispose();
        }
    }
}