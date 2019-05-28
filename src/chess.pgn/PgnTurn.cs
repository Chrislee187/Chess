using System;
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
            var tokens = new Stack<string>(moveText.Split(new[] {' ', '\t', '.'}).Where(s => s.Trim().Any()).Reverse());

            var turns = new List<PgnTurn>();
            PgnMove white = null;
            PgnMove black = null;
            var annotationToken = false;
            var annotation = "";
            var turnIdx = 0;
            result = PgnGameResult.Unknown;

            while (tokens.Any())
            {
                // Number, '{}', Alpha, PgnGameResult
                var token = tokens.Pop();

                var tokenType = PgnTurnTokenParser.GetTokenType(token);

                if (annotationToken && tokenType != PgnTurnTokenTypes.AnnotationEnd)
                {
                    annotation = string.IsNullOrEmpty(annotation)
                        ? token
                        : $"{annotation} {token}";
                    continue;
                }

                switch (tokenType)
                {
                    case PgnTurnTokenTypes.AnnotationStart:
                        if (token.Length > 1) tokens.Push(token.Substring(1));
                        annotationToken = true;
                        break;

                    case PgnTurnTokenTypes.AnnotationEnd:
                        if (token.Length > 1)
                        {
                            tokens.Push("}");
                            tokens.Push(token.Substring(0,token.Length-1));
                        }
                        else
                        {
                            annotationToken = false;

                            if (white != null && black == null)
                            {
                                white.Annotation = annotation;
                            }
                            else if (black != null)
                            {
                                black.Annotation = annotation;
                            }
                            annotation = null;
                        }
                        break;
                    case PgnTurnTokenTypes.TurnStart:
                    {
                        turnIdx = token.ToInt();
                        if (white != null && black != null)
                        {
                            turns.Add(new PgnTurn(token.ToInt(), white, black));
                            white = black = null;
                            annotation = null;
                        }

                        break;
                    }
                    case PgnTurnTokenTypes.Notation when white == null:
                        white = new PgnMove(token, annotation);
                        break;
                    case PgnTurnTokenTypes.Notation:
                        black = new PgnMove(token, annotation);
                        break;
                    case PgnTurnTokenTypes.GameResult:
                        result = PgnTurnTokenParser.ParseResult(token);
                        turns.Add(new PgnTurn(turnIdx, white, black));
                        white = black = null;
                        annotation = null;
                        break;
                    default:
                        throw new Exception($"Unexpected tokenType '{tokenType}'");
                }

            }

            if (white != null)
            {
                if (turns.All(t => t.Turn != turnIdx))
                {
                    turns.Add(new PgnTurn(turnIdx, white, black));
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