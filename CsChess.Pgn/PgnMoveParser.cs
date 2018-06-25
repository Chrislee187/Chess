using System;
using System.Collections.Generic;
using System.Linq;
using CSharpChess;
using CSharpChess.Movement;
using CSharpChess.System;


namespace CsChess.Pgn
{
    /*
     * Pgn notation requires current board state to determine the actual moveType
     * as it is not explicit about the piece being moved only what happened and
     * where the destination was.
     * 
     * We will parse the raw text to create a pgnMoveQuery to apply against the board
     * that can be used to determine the missing details
     * 
     */

    public static class PgnMoveParser
    {
        public static bool TryParse(Colours turn, string move, ref PgnQuery pgnQuery)
        {
            pgnQuery.WithColour(turn);
            pgnQuery.WithPgn(move);
            if (move.ToUpper() == "O-O" || move.ToUpper() == "O-O-O")
            {
                CreateCastleMoveQuery(turn, move, pgnQuery);
                return true;
            }

            if (MoveContainsGameResult(move))
            {
                pgnQuery.WithResult(move);
                return true;
            }

            if (MoveContainsPromotion(move))
            {
                var newMove = StripPromotion(move);
                var promotionPiece = move.Substring(move.IndexOf("=", StringComparison.Ordinal)+1, 1);
                pgnQuery.WithMoveType(MoveType.Promotion);
                pgnQuery.WithPromotion(promotionPiece[0]);

                return TryParse(turn, newMove, ref pgnQuery);
            }

            if (MoveContainsCheck(move))
            {
                move = move.Substring(0, move.Length - 1);
                if (!TryParse(turn, move, ref pgnQuery)) return false;

                // TODO: If the sub-parse returns 'Check' probably checkmate
                pgnQuery.WithMoveType(MoveType.Check);
                return true;
            }

            if (move.Length == 2)
            {
                MoveLength2(turn, move, pgnQuery);
                return true;
            }

            if (move.Length == 3)
            {
                MoveLength3(turn, move, pgnQuery);
                return true;
            }
            if (move.Length == 4)
            {
                MoveLength4(turn, move, pgnQuery);
                return true;
            }
            if (move.Length == 5)
            {
                MoveLength5(turn, move, pgnQuery);
                return true;
            }

            throw new ArgumentOutOfRangeException($"Unable to parse: {move}");
        }

        private static void CreateCastleMoveQuery(Colours turn, string move, PgnQuery pgnQuery)
        {
            var dest = CalcKingDestinationForCastle(turn, move);
            pgnQuery.WithMoveType(MoveType.Castle);
            pgnQuery.WithPiece(new ChessPiece(turn, PieceNames.King));
            pgnQuery.WithFromFile('e');
            pgnQuery.WithFromRank(turn == Colours.White ? '1' : '8');
            SetToFileFromMoveAt(pgnQuery, dest, 0);
        }
        private static void MoveLength5(Colours turn, string move, PgnQuery pgnQuery)
        {
            /* Nbxd7 */

            if (CharIsNamedPiece(move[0]))
            {
                pgnQuery.WithPiece(new ChessPiece(turn, GetPieceName(move[0])));
            }
            else
            {
                throw new ArgumentException($"First character not a piece: {move}", nameof(move));
            }
            if (char.IsNumber(move[1]))
            {
                pgnQuery.WithFromRank(move[1]);
            }
            else
            {
                pgnQuery.WithFromFile(move[1]);
            }

            if (MoveContainsTake(move, 2))
            {
                pgnQuery.WithMoveType(MoveType.Take);
            }
            else
            {
                throw new ArgumentException($"Not a take move: {move}", nameof(move));
            }
            SetToFileFromMoveAt(pgnQuery, move, 3);
        }

        private static void MoveLength4(Colours turn, string move, PgnQuery pgnQuery)
        {
            pgnQuery.WithMoveType(MoveType.Move);

            if (CharIsNamedPiece(move[0]))
            {
                SetPieceFromMoveAt(pgnQuery, turn, move);
            }
            else
            {
                pgnQuery.WithPiece(new ChessPiece(turn, PieceNames.Pawn));
                pgnQuery.WithFromFile(move[0]);
            }

            if (MoveContainsTake(move, 1))
            {
                pgnQuery.WithMoveType(MoveType.Take);
                SetToFileFromMoveAt(pgnQuery, move, 2);
            }
            else if (MoveContainsPromotion(move, 2))
            {
                pgnQuery.WithMoveType(MoveType.Promotion);
                SetToFileFromMoveAt(pgnQuery, move, 0);
                pgnQuery.WithPromotion(move[3]);
            }
            else
            {
                if (char.IsNumber(move[1]))
                {
                    pgnQuery.WithFromRank(move[1]);
                }
                else
                {
                    pgnQuery.WithFromFile(move[1]);
                }

                SetToFileFromMoveAt(pgnQuery, move, 2);
            }
        }

        private static void MoveLength3(Colours turn, string move, PgnQuery pgnQuery)
        {
            SetPieceFromMoveAt(pgnQuery, turn, move);
            if (IsCastleMove(move))
            {
                CreateCastleMoveQuery(turn, move, pgnQuery);
            }
            else
            {
                SetToFileFromMoveAt(pgnQuery, move, 1);
            }
        }

        private static void MoveLength2(Colours turn, string move, PgnQuery pgnQuery)
        {
            pgnQuery.WithPiece(new ChessPiece(turn, PieceNames.Pawn));
            pgnQuery.WithMoveType(MoveType.Move);
            pgnQuery.WithFromFile(move[0]);
            SetToFileFromMoveAt(pgnQuery, move, 0);
        }

        private static string CalcKingDestinationForCastle(Colours turn, string move)
        {
            if (move.ToUpper() == "O-O")
            {
                return "G" + ((turn == Colours.White) ? "1" : "8");
            }

            return "C" + ((turn == Colours.White) ? "1" : "8");
        }

        private static PieceNames GetPieceName(char chr)
        {
            PieceNames result;
            
            if (char.IsUpper(chr))
            {
                var pieceChar = char.ToUpper(chr);
                if (!PgnNameMap.ContainsKey(pieceChar))
                {
                    throw new NotImplementedException($"'{pieceChar}' is not a valid SAN piece.");
                }
                result = PgnNameMap[pieceChar];
            }
            else
            {
                result = PieceNames.Pawn;
            }
            return result;
        }
        private static readonly IDictionary<char, PieceNames> PgnNameMap = new Dictionary<char, PieceNames>
        {
            {'P', PieceNames.Pawn },
            {'N', PieceNames.Knight },
            {'B', PieceNames.Bishop },
            {'R', PieceNames.Rook },
            {'Q', PieceNames.Queen },
            {'K', PieceNames.King },
            {'O', PieceNames.King },
        };

        private static string StripPromotion(string move)
            => move.Substring(0, move.IndexOf('='));

        private static bool CharIsNamedPiece(char c)
           => char.IsUpper(c);


        private static bool IsCastleMove(string move, int checkIndexedChar = -1)
            => checkIndexedChar == -1
                ? move.ToUpper().Contains("O-O")
                : char.ToUpper(move[checkIndexedChar]) == 'O';

        private static bool MoveContainsGameResult(string move)
           => move.Contains("-") && move.ToUpper().First() != 'O' || move.First() == '*';

        private static bool MoveContainsPromotion(string move, int checkIndexedChar = -1)
            => checkIndexedChar == -1
                ? move.Contains("=")
                : char.ToLower(move[checkIndexedChar]) == '=';

        private static bool MoveContainsCheck(string move)
            => move.EndsWith("+");

        private static bool MoveContainsTake(string move, int checkIndexedChar = -1)
            => checkIndexedChar == -1 
                ? move.Contains("x") 
                : char.ToLower(move[checkIndexedChar]) == 'x';

        private static void SetPieceFromMoveAt(PgnQuery pgnQuery, Colours turn, string move)
           => pgnQuery.WithPiece(new ChessPiece(turn, GetPieceName(move[0])));

        private static void SetToFileFromMoveAt(PgnQuery pgnQuery, string move, int index)
        {
            pgnQuery.WithToFile(move[index]);
            pgnQuery.WithToRank(move[index + 1]);
        }

    }
}