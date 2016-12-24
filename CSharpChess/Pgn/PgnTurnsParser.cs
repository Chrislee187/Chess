using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharpChess.Pgn
{
    public static class PgnTurnsParser
    {
        public static bool TryParse(string text, out IEnumerable<PgnTurnQuery> pgnTurns)
        {
            var turns = new List<PgnTurnQuery>();

            var moveIsFor = Chess.Colours.None;
            PgnMoveQuery white = null, black = null;
            string currentText = "";

            Action resetTurnState = () =>
            {
                moveIsFor = Chess.Colours.None;
                currentText = string.Empty;
                white = black = null;
            };

            var tokens = new Stack<string>(text.Split(' ').Reverse());
            int turnNumber = 0;
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

            if (moveIsFor != Chess.Colours.None)
            {
                turns.Add(new PgnTurnQuery(turnNumber, white, black, currentText));
            }
            pgnTurns = turns;
            return true;
        }

        private static void ExpectToKnowColour(Chess.Colours currentColour, string token)
        {
            if (currentColour == Chess.Colours.None)
                throw new ArgumentException($"Parse error: {token}, expected move but don't know for which colour");
        }

        private static int ParseTurnNumber(string token) 
            => int.Parse(token.Substring(0, token.IndexOf(".")));

        private static Chess.Colours ParseMoveColour(string token) 
            => token.Count(c => c == '.') == 1 
                    ? Chess.Colours.White 
                    : Chess.Colours.Black;

        private static PgnMoveQuery ParseMove(Chess.Colours currentColour, string token)
        {
            PgnMoveQuery moveQuery;
            if (!PgnMoveQuery.TryParse(currentColour, token, out moveQuery))
                throw new ArgumentException($"'{token}' could be parsed as a Pgn move.");
            return moveQuery;
        }

        private static bool IsNewTurn(string token)
        {
            return token.EndsWith(".");
        }

        private static bool ContainsStartOfComment(string token, char commentStartChar)
        {
            var idx = token.IndexOf(commentStartChar);

            return token.Contains(commentStartChar.ToString());
        }

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