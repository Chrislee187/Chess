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
        static void Main(string[] args)
        {
            var board = new ChessBoard();

            bool showError = false;
            MoveResult moveResult = null;
            while (!board.GameOver())
            {
                Console.WriteLine(MediumConsoleBoard.ToString(board));

                ShowError(showError, moveResult);

                Console.Write($"Player {board.WhoseTurn} to play: ");
                var cmd = Console.ReadLine();

                if (cmd.ToLower() == "quit")
                {
                    Environment.Exit(0);
                }

                moveResult = board.Move(cmd);
                showError = !moveResult.Succeeded;
                Console.Clear();
            }

            Console.WriteLine($"Game over, {board.GameState}");
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
