using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace chess.pgn
{
    [DebuggerDisplay("{DebuggerDisplayText}")]
    public class PgnGame
    {
        private string DebuggerDisplayText => $"{White} vs {Black} @ {Event} #{Round} {Result}";

        // NOTE: (https://en.wikipedia.org/wiki/Portable_Game_Notation#Tag_pairs)
        // Strictly speaking Tag pairs have some rules about a standard set of 7 in a specific order
        // we expose the 7 here as public properties, however we are not caring to about the order, nor enforcing their existence
        public IReadOnlyDictionary<string, string> TagPairs { get; }
        public IEnumerable<PgnTurn> Turns { get; }

        private string SafePairValue(string key)
        {
            if (TagPairs.ContainsKey(key))
                return TagPairs[key];

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


        private PgnGame(string pgnText, string moveText, IReadOnlyDictionary<string, string> tagPairs,
            IEnumerable<PgnTurn> turns,
            PgnGameResult pgnResult)
        {
            PgnText = pgnText;
            TagPairs = tagPairs;
            Turns = turns;
            Result = pgnResult;
            MoveText = moveText;
        }


        public static PgnGame Parse(string gameText)
        {
            // NOTE: Known Issue: Because we split the turn numbers up based on the '.' char after the number we also lose any '.' chars
            // in any comments
            var remainder = ParseTagPairs(gameText, out var tagPairs).Trim();
            var moveText = remainder.Replace("\r", " ").Replace("\n", " ");
            var tokens = new Stack<string>(moveText.Split(new []{' ', '.'}).Where(s => s.Trim().Any()).Reverse());

            var turns = new List<PgnTurn>();
            PgnMove white = null;
            PgnMove black = null;
            var commentToken = false;
            var comment = "";
            var turnIdx = 0;
            PgnGameResult pgnResult = PgnGameResult.Unknown;
            while (tokens.Any())
            {
                // Number, '{}', Alpha, PgnGameResult
                var token = tokens.Pop();

                if (token == "}")
                {
                    commentToken = false;
                }

                if (commentToken)
                {
                    comment = comment == string.Empty 
                        ? token 
                        : $"{comment} {token}";
                }
                else if (token.StartsWith("{"))
                {
                    if(token.Length > 1) tokens.Push(token.Substring(1));
                    commentToken = true;
                }
                else if (token.ToInt() > 0)
                {
                    turnIdx = token.ToInt();
                    if (turnIdx != 1)
                    {
                        turns.Add(new PgnTurn(turnIdx-1, white, black));
                        white = null;
                        black = null;
                        comment = "";
                    }
                }
                else
                {
                    pgnResult = ParseResult(token);
                    if (pgnResult == PgnGameResult.Unknown)
                    {
                        if (white == null)
                        {
                            white = new PgnMove(token, comment);
                        }
                        else
                        {
                            black = new PgnMove(token, comment);
                        }
                    }
                    else
                    {
                        turns.Add(new PgnTurn(turnIdx, white, black));
                        white = null;
                        black = null;
                        comment = "";
                    }
                }
            }

            var pgnGame = new PgnGame(gameText, moveText, tagPairs, turns, pgnResult);
            return pgnGame;
        }

        private static PgnGameResult ParseResult(string tagPair)
        {
            if (tagPair == "1/2-1/2") return PgnGameResult.Draw;
            if (tagPair == "1-0") return PgnGameResult.WhiteWins;
            if (tagPair == "0-1") return PgnGameResult.BlackWins;
            if (tagPair == "*") return PgnGameResult.Unknown;

            return PgnGameResult.Unknown;
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
    }
}