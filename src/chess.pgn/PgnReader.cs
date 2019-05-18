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

        public static PgnReader FromFile(string filename)
            => new PgnReader(File.Open(filename, FileMode.Open, FileAccess.Read));

        private PgnReader(Stream stream) 
            => _reader = new StreamReader(stream);

        private PgnReader(string pgnString) => 
            _reader = new StreamReader(pgnString.ToStream());

        public PgnGame ReadGame()
        {
            if (_reader.EndOfStream) return null;

            var text = ReadSingleGame();
            return PgnGame.Parse(text);
        }

        private string ReadSingleGame()
        {
            var remainder = ReadNextNonEmptyLine();
            if (remainder == null) return null;

            var tagPairText = $"{remainder}\n{ReadUntilNextEmptyLine()}";
            if (tagPairText == null) throw new InvalidDataException($"Expected Tag Pair text, found EOF!");

            remainder = ReadNextNonEmptyLine();
            if (remainder == null) throw new InvalidDataException($"No move text found.");

            var moveText = $"{remainder}\n{ReadUntilNextEmptyLine()}"; 

            return tagPairText + Environment.NewLine + moveText;
        }

        private string ReadUntilNextEmptyLine()
        {
            var line = ReadLine();
            var sb = new StringBuilder();

            while (!string.IsNullOrEmpty(line))
            {
                sb.AppendLine(line);
                line = ReadLine();
            }

            return sb.ToString();
        }

        private string ReadNextNonEmptyLine()
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