using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using chess.pgn.Parsing;

namespace chess.pgn
{
    [DebuggerDisplay("{DebuggerDisplayText}")]
    public class PgnGame
    {
#if DEBUG
        private string DebuggerDisplayText => $"{White} vs {Black} @ {Event} #{Round} {Result}";
# endif
        public static IEnumerable<PgnGame> ReadAllGamesFromFile(string filename) => ReadAllGamesFromString(File.ReadAllText(filename));

        public static IEnumerable<PgnGame> ReadAllGamesFromString(string pgnText)
            => PgnUnparsedGame
                .Parse(pgnText)
                .Select(Parse);

        private static PgnGame Parse(PgnUnparsedGame text)
        {
            var tagPairs = PgnTagPair.ParseMultiple(text.TagPairSection);
            var turns = PgnTurn.Parse(text.MoveListSection, out var pgnResult);
            
            var pgnGame = new PgnGame(text.ToPgnText(), text.MoveListSection, tagPairs, turns, pgnResult);
            return pgnGame;
        }

        // NOTE: (https://en.wikipedia.org/wiki/Portable_Game_Notation#Tag_pairs)
        // Strictly speaking Tag pairs have some rules about a standard set of 7 in a specific order
        // we expose the 7 here as public properties, however we are not caring to about the order, nor enforcing their existence
        public IEnumerable<PgnTagPair> TagPairs { get; }
        public IEnumerable<PgnTurn> Turns { get; }

        private string SafePairValue(string key)
        {
            var pair = TagPairs.FirstOrDefault(k => k.Name.Equals(key));
            if (pair != null)
                return pair.Value;

            return "";
        }

        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public string PgnText { get; }

        public string Event => SafePairValue("Event");
        public string Site => SafePairValue("Site");
        public PgnDate Date => PgnDate.Parse(SafePairValue("Date"));
        public string Round => SafePairValue("Round");
        public string White => SafePairValue("White");
        public string Black => SafePairValue("Black");
        public PgnGameResult Result { get; }
        public string MoveText { get; }


        private PgnGame(string pgnText, string moveText, IEnumerable<PgnTagPair> tagPairs,
            IEnumerable<PgnTurn> turns,
            PgnGameResult pgnResult)
        {
            PgnText = pgnText;
            TagPairs = tagPairs;
            Turns = turns;
            Result = pgnResult;
            MoveText = moveText;
        }
    }
}