using System;
using System.Collections.Generic;
using System.Linq;
using CSharpChess.System.Extensions;

namespace CSharpChess.Pgn
{
    public class PgnTurnsParser
    {
        public static bool TryParse(string text, out IEnumerable<PgnTurnQuery> pgnTurns)
        {
            var turns = new List<PgnTurnQuery>();

            var tokens = new Stack<string>(text.Split(' ').Reverse());
            var currentColour = Chess.Colours.None;
            PgnMoveQuery white = null, black = null;
            int turnNumber = 0;
            string currentText = "";
            while (tokens.Any())
            {
                var token = tokens.Pop();
                currentText = $"{currentText}{token} ";

                if (token.StartsWith(";"))
                {
                    IgnoreEndOfLineComment(token, tokens);
                }
                else if (token.EndsWith("."))
                {
                    var dotCount = token.Count(c => c == '.');
                    if (currentColour == Chess.Colours.None)
                    {
                        turnNumber = int.Parse(token.Substring(0, token.IndexOf(".")));
                        currentColour = dotCount == 1 ? Chess.Colours.White : Chess.Colours.Black;
                    }
                    else
                    {
                        tokens.Push(token);

                        turns.Add(new PgnTurnQuery(turnNumber, white, black, currentText));
                        currentColour = Chess.Colours.None;
                    }
                }
                else
                {
                    if(currentColour == Chess.Colours.None) throw new ArgumentException();

                    PgnMoveQuery moveQuery;
                    var move = PgnMoveQuery.TryParse(currentColour, token, out moveQuery);

                    if (currentColour == Chess.Colours.White)
                    {
                        white = moveQuery;
                        currentColour = Chess.Colours.Black;
                    }
                    else
                    {
                        black = moveQuery;

                        turns.Add(new PgnTurnQuery(turnNumber, white, black, currentText));
                        currentColour = Chess.Colours.None;
                    }
                }
            }
            if (currentColour != Chess.Colours.None)
            {
                turns.Add(new PgnTurnQuery(turnNumber, white, black, currentText));
            }
            pgnTurns = turns;
            return true;
        }

        private static void IgnoreEndOfLineComment(string token, Stack<string> tokens)
        {
            var comment = token;
            while (tokens.Any() && !tokens.Peek().Contains("\n"))
            {
                comment = $"{comment} {tokens.Pop()}";
            }

            if (tokens.Any())
            {
                var lastCommentToken = tokens.Pop();
                var split = lastCommentToken.Split('\n');
                comment = $"{comment} {split[0]}";
                if (split.Length > 1)
                {
                    tokens.Push(split.Skip(1).Aggregate("", (a, i) => $"{a} {i}"));
                }
            }
        }
    }
}