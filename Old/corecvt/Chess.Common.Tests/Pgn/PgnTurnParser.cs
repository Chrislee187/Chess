using System;
using System.Collections.Generic;
using System.Linq;
using CSharpChess.System;

namespace Chess.Common.Tests.Pgn
{
    public static class PgnTurnParser
    {
        public static bool TryParse(string text, out IEnumerable<PgnTurnQuery> pgnTurns)
        {
            var turns = new List<PgnTurnQuery>();

            var moveIsFor = Colours.None;
            PgnQuery white = null, black = null;
            string currentText = "";
            int turnNumber = 0;

            Action resetTurnState = () =>
            {
                moveIsFor = Colours.None;
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
                    if (moveIsFor == Colours.None)
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
                    if (IsEndGameToken(token))
                    {
                        if (moveIsFor == Colours.White)
                        {
                            white.WithResult(token);
                            black = CreateEndGameQuery(token);
                        }
                        else if (moveIsFor == Colours.Black)
                        {
                            black = CreateEndGameQuery(token);
                            moveIsFor = Colours.None;
                        }
                        else
                        {
                            white = CreateEndGameQuery(token);
                            black = CreateEndGameQuery(token);
                        }

                        var pgnTurnQuery = new PgnTurnQuery(turnNumber, white, black, currentText);

                        turns.Add(pgnTurnQuery);
                    }
                    else
                    {
                        ExpectToKnowColour(moveIsFor, token);

                        var moveQuery = ParseMove(moveIsFor, token);

                        if (moveIsFor == Colours.White)
                        {
                            white = moveQuery;
                            moveIsFor = Colours.Black;
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

            if (moveIsFor != Colours.None)
            {
                turns.Add(new PgnTurnQuery(turnNumber, white, black, currentText));
            }
            pgnTurns = turns;
            return true;
        }

        private static PgnQuery CreateEndGameQuery(string token)
        {
            var b = new PgnQuery();
            b.WithResult(token);
            return b;
        }

        private static bool IsEndGameToken(string token)
        {
            return token == "1/2-1/2"
                || token == "1-0"
                || token == "0-1"
                || token == "*";
        }

        // ReSharper disable once UnusedParameter.Local
        // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
        private static void ExpectToKnowColour(Colours currentColour, string token)
        {
            if (currentColour == Colours.None)
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

        private static Colours ParseMoveColour(string token) 
            => token.Count(c => c == '.') == 1 
                    ? Colours.White 
                    : Colours.Black;

        private static PgnQuery ParseMove(Colours currentColour, string token)
        {
            var pgnQuery = new PgnQuery();
            if (!PgnMoveParser.TryParse(currentColour, token, ref pgnQuery))
                throw new ArgumentException($"'{token}' could be parsed as a Pgn move.");
            return pgnQuery;
        }

        private static bool IsNewTurn(string token)
        {
            var idx = token.IndexOf(".", StringComparison.Ordinal);

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