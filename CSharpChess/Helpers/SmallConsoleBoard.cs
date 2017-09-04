using System;
using System.Collections.Generic;
using System.Linq;
using CSharpChess.System;
using CSharpChess.System.Extensions;
using CSharpChess.TheBoard;

namespace CSharpChess.Helpers
{
    public class SmallConsoleBoard
    {
        // TODO: Refactor to use same pattern as Medium
        private const bool UseColours = false;
        private const bool ShowThreat = true;
        public static void Write(ChessBoard board)
        {
            var consoleBoard = CreateConsoleBoard(board);

            foreach (var rank in Chess.Ranks.Reverse())
            {
                foreach (var file in Chess.Files)
                {
                    consoleBoard[BoardLocation.At((int) file, rank)]();
                }
                Console.Write("\n");
            }

        }

        private static Dictionary<BoardLocation, Action> CreateConsoleBoard(ChessBoard board)
        {
            var consoleBoard = new Dictionary<BoardLocation, Action>();

            foreach (var boardPiece in board.Pieces)
            {
                var hasThreats = false; //boardPiece.MoveFactory.All(board, boardPiece.Location).Any();
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
                        Console.Write(OneCharBoard.ToChar(boardPiece.Piece));
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