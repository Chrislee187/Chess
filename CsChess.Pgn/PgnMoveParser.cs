using System;
using System.Collections.Generic;
using CSharpChess;
using CSharpChess.TheBoard;

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
        private static readonly IDictionary<char, Chess.PieceNames> PgnNameMap = new Dictionary<char, Chess.PieceNames>
        {
            {'P', Chess.PieceNames.Pawn },
            {'N', Chess.PieceNames.Knight },
            {'B', Chess.PieceNames.Bishop },
            {'R', Chess.PieceNames.Rook },
            {'Q', Chess.PieceNames.Queen },
            {'K', Chess.PieceNames.King },
            {'O', Chess.PieceNames.King },
        };

        public static bool TryParse(Chess.Colours turn, string move, ref PgnQuery pgnQuery)
        {
            pgnQuery.WithColour(turn);

            // TODO:
            if (MoveContainsPromotion(move))
            {
//                var promotedTo = GetPromotionPiece(move);
//
                var newMove = StripPromotion(move);
                pgnQuery.WithMoveType(MoveType.Promotion);

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
            if (move.ToUpper() == "O-O-O" || move.ToUpper() == "O-O-O")
            {
                CreateCastleMoveQuery(turn, move, pgnQuery);
                return true;
            }
            if (move.Length == 5)
            {
                MoveLength5(turn, move, pgnQuery);
                return true;
            }

            if (move.Contains("-"))
            {
                pgnQuery.WithResult(move);
                return true;
            }

            throw new ArgumentOutOfRangeException($"Unable to parse: {move}");
        }

        private static string StripPromotion(string move)
        {
            return move.Substring(0, move.IndexOf('='));
        }

        private static bool MoveContainsPromotion(string move)
        {
            return move.Contains("=");
        }

        private static bool MoveContainsCheck(string move)
        {
            return move.EndsWith("+");
        }

        private static void CreateCastleMoveQuery(Chess.Colours turn, string move, PgnQuery pgnQuery)
        {
            var dest = CalcKingDestinationForCastle(turn, move);
            pgnQuery.WithMoveType(MoveType.Castle);
            pgnQuery.WithFromFile('e');
            pgnQuery.WithFromRank(turn == Chess.Colours.White ? '1' : '8');
            pgnQuery.WithToFile(dest[0]);
            pgnQuery.WithToRank(dest[1]);
        }
        private static void MoveLength5(Chess.Colours turn, string move, PgnQuery pgnQuery)
        {
            /* Nbxd7 */

            if (char.IsUpper(move[0]))
            {
                pgnQuery.WithPiece(new ChessPiece(turn, GetPieceName(move[0])));
            }
            else
            {
                throw new ArgumentException($"First character not a piece: {move}", nameof(move));
            }
            pgnQuery.WithFromFile(move[1]);

            if (move[2].ToString().ToLower() == "x")
            {
                pgnQuery.WithMoveType(MoveType.Take);
            }
            else
            {
                throw new ArgumentException($"Not a take move: {move}", nameof(move));
            }

            pgnQuery.WithToFile(move[3]);
            pgnQuery.WithToRank(move[4]);
        }

        private static void MoveLength4(Chess.Colours turn, string move, PgnQuery pgnQuery)
        {
/*
                    Nbd7
                    Nxe4
                    Ra6+
                    cxb5
                 */
            pgnQuery.WithMoveType(MoveType.Move);

            if (char.IsUpper(move[0]))
            {
                pgnQuery.WithPiece(new ChessPiece(turn, GetPieceName(move[0])));
            }
            else
            {
                pgnQuery.WithPiece(new ChessPiece(turn, Chess.PieceNames.Pawn));
                pgnQuery.WithFromFile(move[0]);
            }

            if (move[1].ToString().ToLower() == "x")
            {
                pgnQuery.WithMoveType(MoveType.Take);
                pgnQuery.WithToFile(move[2]);
                pgnQuery.WithToRank(move[3]);
            }
            else if (move[2] == '=')
            {
                pgnQuery.WithMoveType(MoveType.Promotion);
                pgnQuery.WithToFile(move[0]);
                pgnQuery.WithToRank(move[1]);
                pgnQuery.WithPromotion(move[3]);
            }
            else
            {
                pgnQuery.WithFromFile(move[1]);
                pgnQuery.WithToFile(move[2]);
                pgnQuery.WithToRank(move[3]);
            }
        }

        private static void MoveLength3(Chess.Colours turn, string move, PgnQuery pgnQuery)
        {
            var pieceName = GetPieceName(move[0]);
            pgnQuery.WithPiece(new ChessPiece(turn, pieceName));
            if (move[0].ToString().ToUpper() == "O")
            {
                CreateCastleMoveQuery(turn, move, pgnQuery);
            }
            else
            {
                pgnQuery.WithMoveType(MoveType.Move);
                pgnQuery.WithToFile(move[1]);
                pgnQuery.WithToRank(move[2]);
            }
        }

        private static void MoveLength2(Chess.Colours turn, string move, PgnQuery pgnQuery)
        {
            pgnQuery.WithPiece(new ChessPiece(turn, Chess.PieceNames.Pawn));
            pgnQuery.WithMoveType(MoveType.Move);
            pgnQuery.WithFromFile(move[0]);
            pgnQuery.WithToFile(move[0]);
            pgnQuery.WithToRank(move[1]);
        }

        private static string CalcKingDestinationForCastle(Chess.Colours turn, string move)
        {
            if (move.ToUpper() == "O-O")
            {
                return "G" + ((turn == Chess.Colours.White) ? "1" : "8");
            }

            return "C" + ((turn == Chess.Colours.White) ? "1" : "8");
        }

        private static Chess.PieceNames GetPieceName(char chr)
        {
            Chess.PieceNames result;
            
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
                result = Chess.PieceNames.Pawn;
            }
            return result;
        }
    }
}