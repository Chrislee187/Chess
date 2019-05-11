using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using CSharpChess.Extensions;
using CSharpChess.System;

namespace Chess.Common.Tests.Pgn
{
    public class PgnGame
    {
        public IReadOnlyDictionary<string,string> TagPairs { get; }
        public IEnumerable<PgnTurnQuery> TurnQueries { get; }

        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public string PgnText { get; }

        public string Event => TagPairs["Event"];
        public string Site => TagPairs["Site"];
        public PgnDate Date =>  PgnDate.Parse(TagPairs["Date"]);
        public int Round => TagPairs["Round"].ToInt();
        public string White => TagPairs["White"];
        public string Black => TagPairs["Black"];
        public ChessGameResult Result => PgnResult.Parse(TagPairs["Result"]);


        private PgnGame(string pgnText, IReadOnlyDictionary<string, string> tagPairs, IEnumerable<PgnTurnQuery> turnQueries)
        {
            PgnText = pgnText;
            TagPairs = tagPairs;
            TurnQueries = turnQueries;
        }

        public static IEnumerable<PgnGame> Parse(string gameText)
        {
            var trimmed = gameText.Trim();

            var linesTrimmed = new Regex("^ +", RegexOptions.Multiline).Replace(trimmed, "");

            var chunks = linesTrimmed.Split(new [] { $"{Environment.NewLine}{Environment.NewLine}"}, StringSplitOptions.RemoveEmptyEntries);

            var stk = new Stack<string>(chunks);

            if (stk.Count() % 2 != 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            var list = new List<PgnGame>();

            while (stk.Any())
            {
                var moves = stk.Pop();
                var tags = stk.Pop();

                var singleGame = tags + Environment.NewLine.Repeat(2) + moves;
                var pgnGame = ParseSingleGame(singleGame, singleGame);
                list.Add(pgnGame);
            }

            return list;
        }

        private static PgnGame ParseSingleGame(string gameText, string remainder)
        {
            Dictionary<string, string> tagPairs;
            remainder = ParseTagPairs(remainder, out tagPairs);

            IEnumerable<PgnTurnQuery> pgnTurnQueries;
            var parsed = PgnTurnParser.TryParse(remainder, out pgnTurnQueries);

            if (!parsed) throw new InvalidOperationException("Turn parsing failed");

            var pgnGame = new PgnGame(gameText, tagPairs, pgnTurnQueries);
            return pgnGame;
        }

        private static string ParseTagPairs(string remainder, out Dictionary<string, string> tagPairs)
        {
            var pairs = remainder.Split(']');
            var tps = new Dictionary<string, string>();

            if (pairs.Length > 1)
            {
                foreach (var pair in pairs)
                {
                    if (pair.Trim().First() == '[')
                    {
                        var pgnTagPair = PgnTagPair.Parse(pair);
                        tps.Add(pgnTagPair.Name, pgnTagPair.Value);
                    }
                    else
                    {
                        break;
                    }
                }
            }
            tagPairs = tps;
            return pairs.Last();
        }

        public override string ToString()
        {
            return $"{White} vs {Black} @ {Event} #{Round} {Result}";
        }
    }
}