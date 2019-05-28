using System;
using System.IO;

namespace chess.pgn.Parsing
{
    public class TextParser : IDisposable
    {
        private readonly StreamReader _reader;

        public TextParser(StreamReader reader)
        {
            _reader = reader;
        }
        public TextParser(string text)
        {
            _reader = new StreamReader(text.ToStream());
        }

        public bool IsNextChar(char c) => _reader.Peek() == c;
        public bool EndOfStream => _reader.EndOfStream;

        public void SkipWhiteSpace() => ReadUntil(c => !char.IsWhiteSpace(c));
        public string ReadUntil(char until) => ReadUntil(c => c == until);
        public string ReadUntil(Predicate<char> predicate)
        {
            var output = "";
            while (!_reader.EndOfStream)
            {
                var c = (char)_reader.Peek();

                if (predicate(c))
                {
                    return output;
                }
                else
                {
                    output += (char)_reader.Read();
                }
            }

            return output;
        }
        public string ReadDelimitedBy(char start, char end)
        {
            if (_reader.Peek() != start) throw new Exception($"Expected next character in stream to be '{start}' but was '{(char)_reader.Peek()}'.");

            _reader.Read();

            var value = ReadUntil(']');
            _reader.Read();

            return value;
        }

        public void Dispose()
        {
            _reader?.Dispose();
        }
    }
}