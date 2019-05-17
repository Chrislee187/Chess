using System;
using System.IO;
using System.Text;

namespace chess.pgn
{
    public class PgnReader : IDisposable, IPgnReader
    {
        private readonly StreamReader _reader;

        public static PgnReader FromString(string pgnString) 
            => new PgnReader(pgnString);

        private PgnReader(Stream stream) 
            => _reader = new StreamReader(stream);

        private PgnReader(string pgnString) => 
            _reader = new StreamReader(pgnString.ToStream());

        public PgnGame ReadGame()
        {
            if (_reader.EndOfStream) return null;

            var text = ReadGameText();
            return PgnGame.Parse(text);
        }

        private string ReadGameText()
        {
            var nextLine = ReadUntilNonEmptyLine();
            if (nextLine == null) return null;

            var tagPairText = ReadUntilEmptyLine(nextLine);
            if (tagPairText == null) throw new InvalidDataException($"Expected Tag Pair text, found EOF!");

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