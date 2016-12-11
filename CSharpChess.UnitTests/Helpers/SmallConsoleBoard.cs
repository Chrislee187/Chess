using System;
using System.Collections.Generic;
using System.Linq;
using CSharpChess.TheBoard;
using CSharpChess.Threat;

namespace CSharpChess.UnitTests.Helpers
{
    public class SmallConsoleBoard
    {
        // TODO: Refactor to use same pattern as Medium
        private const bool UseColours = false;
        private const bool ShowThreat = true;
        public static void Write(ChessBoard board, ThreatAnalyser threats = null)
        {
            var consoleBoard = CreateConsoleBoard(board, threats);

            foreach (var rank in Chess.Board.Ranks.Reverse())
            {
                foreach (var file in Chess.Board.Files)
                {
                    consoleBoard[BoardLocation.At((int) file, rank)]();
                }
                Console.Write("\n");
            }

        }

        private static Dictionary<BoardLocation, Action> CreateConsoleBoard(ChessBoard board, ThreatAnalyser threats)
        {
            var t = threats ?? new ThreatAnalyser(board);

            var consoleBoard = new Dictionary<BoardLocation, Action>();

            foreach (var boardPiece in board.Pieces)
            {
                var hasThreats = t.For(boardPiece.Location).Threats.Any();
                Action write = () =>
                {
                    if (ShowThreat && hasThreats)
                    {
                        if (UseColours)
#pragma warning disable 162
                        {
                            Console.BackgroundColor = ConsoleColor.Red;
                        }
#pragma warning restore 162
                    }

                    if (ShowThreat && hasThreats)
                    {
                        Console.Write("X");
                    }
                    else
                    {
                        Console.Write(OneCharBoard.ToChar(boardPiece));
                    }

                    if (UseColours)
#pragma warning disable 162
                    {
                        Console.ResetColor();
                    }
#pragma warning restore 162
                };

                consoleBoard.Add(boardPiece.Location, write);
            }
            return consoleBoard;
        }
    }
}