using System;
using System.Text;
using System.Threading.Tasks;
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

                if (showError)
                {
                    Console.WriteLine(moveResult.Message);
                }

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
    }
}
