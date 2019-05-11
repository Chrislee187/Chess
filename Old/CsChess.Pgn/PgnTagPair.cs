using System;
using System.Linq;

namespace CsChess.Pgn
{
    public class PgnTagPair
    {
        public string Name { get; }
        public string Value { get; }

        private PgnTagPair(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public static PgnTagPair Parse(string pair)
        {
            var trimmed = pair.Trim();
            if(trimmed.First() != '[') throw new ArgumentException($"Expected '{trimmed}' to start with '['");

            // ReSharper disable once StringIndexOfIsCultureSpecific.1
            var delim = trimmed.IndexOf(" ");

            var name = trimmed.Substring(1, delim - 1).Trim();
            var value = trimmed.Substring(delim).Replace("\"", "").Trim();

            return new PgnTagPair(name, value);
        }

        public override string ToString()
        {
            return $"[{Name} \"{Value}\"]";
        }
    }
}