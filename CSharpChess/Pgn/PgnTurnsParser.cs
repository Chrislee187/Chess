using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharpChess.Pgn
{
    public class PgnTurnsParser
    {
        public static bool TryParse(string text, out IEnumerable<PgnTurnQuery> pgnTurns)
        {
            /*
             * while not end of string
             *  read turn number
             *      read / create white move
             *      read / create black move
             */
            var turns = new List<PgnTurnQuery>();
            var remaining = text;

            Tuple<PgnMoveQuery, PgnMoveQuery> pgnQuery = new Tuple<PgnMoveQuery, PgnMoveQuery>(null, null);
            while (!string.IsNullOrEmpty(remaining))
            {
                int turnNumber;
                remaining = ReadTurnNumber(remaining, out turnNumber);

                Chess.Colours turn;
                remaining = ReadWhoseTurn(remaining, out turn);

                remaining = ReadPgnMove(remaining, turn, out pgnQuery);

                turns.Add(new PgnTurnQuery(turnNumber, pgnQuery.Item1, pgnQuery.Item2, text));
            }
            pgnTurns = turns;
            return true;
        }

        private static string ReadPgnMove(string remaining, Chess.Colours turn, out Tuple<PgnMoveQuery, PgnMoveQuery> pgnQuery)
        {
            var moves = remaining.Split(' ');
            PgnMoveQuery white = null, black = null;

            int movesIdx = 0;
            PgnMoveQuery moveQuery;
            if (!PgnMoveQuery.TryParse(turn, moves[movesIdx], out moveQuery))
            {
                throw new ArgumentException($"Cannot parse {moves[movesIdx]} as a white move!");
            }

            white = moveQuery;
            if (moves.Length == 2)
            {
                if (!PgnMoveQuery.TryParse(Chess.Colours.Black, moves[movesIdx], out black))
                {
                    throw new ArgumentException($"Cannot parse {moves[movesIdx]} as a black move!");
                }
            }
            else
            {
                if (turn == Chess.Colours.Black)
                {
                    white = null;
                    black = moveQuery;
                }
            }
            pgnQuery = new Tuple<PgnMoveQuery, PgnMoveQuery>(white, black);

            return moves.Skip(movesIdx + 1).Aggregate("", (s, a) => $"{s} {a}");
        }

        private static string ReadWhoseTurn(string remaining, out Chess.Colours turn)
        {
            var idx = 0;

            while (remaining[idx++] == '.')
            {}

            if (idx == 2)
            {
                turn = Chess.Colours.White;
            }
            else if (idx == 4)
            {
                turn = Chess.Colours.Black;
            }
            else
            {
                throw new ArgumentException($"Turn could not be determined from '{remaining}'.");
            }

            return remaining.Substring(idx).Trim();
        }

        private static string ReadTurnNumber(string text, out int turnNumber)
        {
            var delim = text.IndexOf(".");

            if (delim < 0)
                throw new ArgumentException($"Turn delimiter (.) not found in '{text}'.", nameof(text));

            var numb = text.Substring(0, delim);

            if(!int.TryParse(numb, out turnNumber))
                throw new ArgumentException($"'{numb}' is not a valid turn number.");

            return text.Substring(delim).Trim();
        }
    }
}