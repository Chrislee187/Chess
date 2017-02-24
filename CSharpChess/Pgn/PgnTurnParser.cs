using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharpChess.Pgn
{
    public static class PgnTurnParser
    {
        public static bool TryParse(string text, out IEnumerable<PgnTurnQuery> pgnTurns)
        {
            var turns = new List<PgnTurnQuery>();

            var moveIsFor = Chess.Colours.None;
            PgnQuery white = null, black = null;
            string currentText = "";
            int turnNumber = 0;

            Action resetTurnState = () =>
            {
                moveIsFor = Chess.Colours.None;
                currentText = string.Empty;
                white = black = null;
            };

            var noLineEndings = text.Replace("\r\n", " ").Replace("\r", " ").Replace("\n", " ");
            var tokens = new Stack<string>(noLineEndings.Split(' ').Where(s => s.Trim().Any()).Reverse());
            while (tokens.Any())
            {
                var token = tokens.Pop();
                currentText = $"{currentText}{token} ";

                if (ContainsStartOfComment(token, ';')) IgnoreTillEndComment(token, tokens, ';', '\n');
                else if (ContainsStartOfComment(token, '{')) IgnoreTillEndComment(token, tokens,'{', '}');
                else if (IsNewTurn(token))
                {
                    if (moveIsFor == Chess.Colours.None)
                    {
                        if (token.Last() != '.')
                        {
                            var idx = token.Split('.');
                            tokens.Push(idx[1]);
                            token = $"{idx[0]}.";
                        }
                        turnNumber = ParseTurnNumber(token);
                        moveIsFor = ParseMoveColour(token);
                    }
                    else
                    {
                        tokens.Push(token);

                        turns.Add(new PgnTurnQuery(turnNumber, white, black, currentText));
                        resetTurnState();
                    }
                }
                else
                {
                    if (!IsEndGameToken(token))
                    {
                        ExpectToKnowColour(moveIsFor, token);

                        var moveQuery = ParseMove(moveIsFor, token);

                        if (moveIsFor == Chess.Colours.White)
                        {
                            white = moveQuery;
                            moveIsFor = Chess.Colours.Black;
                        }
                        else
                        {
                            black = moveQuery;
                            turns.Add(new PgnTurnQuery(turnNumber, white, black, currentText));
                            resetTurnState();
                        }
                    }
                }
            }

            if (moveIsFor != Chess.Colours.None)
            {
                turns.Add(new PgnTurnQuery(turnNumber, white, black, currentText));
            }
            pgnTurns = turns;
            return true;
        }

        private static bool IsEndGameToken(string token)
        {
            return token == "1/2-1/2"
                || token == "1-0"
                || token == "0-1";
        }

        // ReSharper disable once UnusedParameter.Local
        private static void ExpectToKnowColour(Chess.Colours currentColour, string token)
        {
            if (currentColour == Chess.Colours.None)
                throw new ArgumentException($"Parse error: {token}, expected move but don't know for which colour");
        }

        private static int ParseTurnNumber(string token)
        {
            int turnNumber;
            var substring = token.Substring(0, token.IndexOf(".", StringComparison.Ordinal));
            if(!int.TryParse(substring, out turnNumber))
            {
                throw new ArgumentException($"Couldn't parse '{substring}' as a turn number.");
            }
            return turnNumber;
        }

        private static Chess.Colours ParseMoveColour(string token) 
            => token.Count(c => c == '.') == 1 
                    ? Chess.Colours.White 
                    : Chess.Colours.Black;

        private static PgnQuery ParseMove(Chess.Colours currentColour, string token)
        {
            var pgnQuery = new PgnQuery();
            if (!PgnMoveParser.TryParse(currentColour, token, ref pgnQuery))
                throw new ArgumentException($"'{token}' could be parsed as a Pgn move.");
            return pgnQuery;
        }

        private static bool IsNewTurn(string token)
        {
            var idx = token.IndexOf(".");

            if (idx == -1) return false;
            int num;
            var enumerable = token.Take(idx).ToArray();
            var s = new string(enumerable);
            return (int.TryParse(s, out num));
        }

        private static bool ContainsStartOfComment(string token, char commentStartChar) 
            => token.Contains(commentStartChar.ToString());

        private static void IgnoreTillEndComment(string token, Stack<string> tokens, char commentStart, char commentEnd)
        {
            var idx = token.IndexOf(commentStart);
            if (idx == -1) return;

            var preToken = token.Substring(0, idx);
            var comment = "";
            while (tokens.Any() && !tokens.Peek().Contains(commentEnd))
            {
                comment = $"{comment} {tokens.Pop()}";
            }

            if (tokens.Any())
            {
                var lastCommentToken = tokens.Pop();
                var split = lastCommentToken.Split(commentEnd).Where(s => !string.IsNullOrEmpty(s)).ToList();
                if (split.Count > 1)
                {
                    comment = $"{comment} {split[0]}";
                    var postToken = split.Skip(1).Aggregate("", (a, i) => $"{a} {i}");
                    tokens.Push(postToken);

                    if (!string.IsNullOrEmpty(preToken))
                    {
                        tokens.Push(preToken);
                    }
                }
            }
        }
    }
}