using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace chess.pgn
{
    [DebuggerDisplay("{DebuggerDisplayText}")]
    public class PgnTurn
    {
#if DEBUG
        private string DebuggerDisplayText => $"{Turn}. {White?.San ?? "..."} {Black?.San ?? "..."}";
#endif

        public static IEnumerable<PgnTurn> Parse(string text, out PgnGameResult result)
        {
            var moveText = text.Replace("\r", " ").Replace("\n", " ");
            var tokens = new Stack<string>(moveText.Split(new[] { ' ', '.' }).Where(s => s.Trim().Any()).Reverse());

            var turns = new List<PgnTurn>();
            PgnMove white = null;
            PgnMove black = null;
            var commentToken = false;
            var comment = "";
            var turnIdx = 0;
            result = PgnGameResult.Unknown;

            var tokenParser = new PgnTurnTokenParser(); 

            while (tokens.Any())
            {
                // Number, '{}', Alpha, PgnGameResult
                var token = tokens.Pop();

                var tokenType = tokenParser.GetTokenType(token);

                if (tokenType == PgnTurnTokenTypes.CommentEnd)
                {
                    commentToken = false;
                }
                else if (tokenType == PgnTurnTokenTypes.CommentStart)
                {
                    if (token.Length > 1) tokens.Push(token.Substring(1));
                    commentToken = true;
                }
                else if (commentToken)
                {
                    comment = comment == string.Empty
                        ? token
                        : $"{comment} {token}";
                }
                else if (tokenType == PgnTurnTokenTypes.TurnStart)
                {
                    turnIdx = token.ToInt();
                    if (turnIdx != 1)
                    {
                        turns.Add(new PgnTurn(turnIdx - 1, white, black));
                        white = null;
                        black = null;
                        comment = "";
                    }
                }
                else
                {
                    if (tokenType == PgnTurnTokenTypes.Notation)
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
                        result = PgnTurnTokenParser.ParseResult(token);
                        turns.Add(new PgnTurn(turnIdx, white, black));
                        white = null;
                        black = null;
                        comment = "";
                    }
                }
            }

            return turns;
        }


        public int Turn { get; }
        public PgnMove White { get; }
        public PgnMove Black { get; }

        public PgnTurn(int turn, PgnMove white, PgnMove black)
        {
            Turn = turn;
            White = white;
            Black = black;
        }

    }
}