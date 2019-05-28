using System;
using System.Collections.Generic;
using System.IO;

namespace chess.pgn.Parsing
{
    public class PgnUnparsedGame
    {
        public static IEnumerable<PgnUnparsedGame> Parse(StreamReader reader) => Parse(new TextParser(reader));
        public static IEnumerable<PgnUnparsedGame> Parse(string text) => Parse(new TextParser(text));

        private static IEnumerable<PgnUnparsedGame> Parse(TextParser textParser)
        {
            var games = new List<PgnUnparsedGame>();
            while (!textParser.EndOfStream)
            {
                textParser.SkipWhiteSpace();

                if (!textParser.IsNextChar('[')) throw new Exception($"No Tag-Pairs found!");

                var pairs = new List<string>();
                while (textParser.IsNextChar('['))
                {
                    var pair = textParser.ReadDelimitedBy('[', ']').Trim();

                    if (!string.IsNullOrEmpty(pair))
                    {
                        pairs.Add($"[{pair}]");
                        textParser.SkipWhiteSpace();
                    }
                    else
                    {
                        throw new Exception("Empty tag-pair found!");
                    }
                }

                textParser.SkipWhiteSpace();

                if (!textParser.IsNextChar('1')) throw new Exception($"No moves found!");

                var moves = textParser.ReadUntil('[').Trim();

                games.Add(new PgnUnparsedGame(string.Join("\n", pairs), moves));
            }

            return games;
        }

        private PgnUnparsedGame(string tagPairSection, string moveListSection)
        {
            TagPairSection = tagPairSection;
            MoveListSection = moveListSection;
        }

        public string TagPairSection { get; }
        public string MoveListSection { get; }

        public string ToPgnText()
        {
            return $"{TagPairSection}\n{MoveListSection}\n";
        }
    }

}