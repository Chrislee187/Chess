using System;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using ConsoleStuff;
using ConsoleStuff.Tests;
using CSharpChess;
using CSharpChess.System.Extensions;
using CSharpChess.TheBoard;

namespace CsChess
{
    class Program
    {
        private static bool _debug;
        static void Main(string[] args)
        {
            var board = new ChessBoard();

            bool showError = false;
            MoveResult moveResult = null;

            while (!board.GameOver())
            {
                DrawBoard(board, moveResult, showError);
                var cmd = GetCommand(board);

                if (string.IsNullOrEmpty(cmd))
                {
                    // Do nothing and allow redraw
                }
                else if (cmd.ToLower() == "debug")
                {
                    _debug = !_debug;
                }
                else if (cmd.ToLower() != "quit")
                {
                    try
                    {
                        moveResult = board.Move(cmd);
                    }
                    catch (Exception e)
                    {
                        var debugInfo = _debug
                            ? e.StackTrace
                            : "";
                        moveResult = MoveResult.Failure(e.Message + debugInfo, ChessMove.Null);
                    }
                    showError = !moveResult.Succeeded;
                }
                else
                {
                    Console.WriteLine("Exiting...");
                    Environment.Exit(0);
                }
            }

            Console.WriteLine($"Game over, {board.GameState}");
        }

        private static string GetCommand(ChessBoard board)
        {
            Console.Write($"Player {board.WhoseTurn} to play: ");
            var cmd = Console.ReadLine();
            return cmd;
        }

        private static void DrawBoard(ChessBoard board, MoveResult moveResult, bool showError)
        {
            Console.Clear();
            Console.WriteLine(MediumConsoleBoard.ToString(board));
            ShowError(showError, moveResult);
        }

        private static void ShowError(bool showError, MoveResult moveResult)
        {
            if (showError)
            {
                using (new ConsoleColour(ConsoleColor.Red, ConsoleColor.White))
                {
                    Console.WriteLine(moveResult.Message);
                }
            }
        }
    }
}
