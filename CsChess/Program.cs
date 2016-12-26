using System;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using ConsoleStuff;
using ConsoleStuff.Panels;
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
            bool first = true;

            Action<ConsolePanel> sizeConsoleWindow = (screen) =>
            {
                if (Console.WindowWidth < screen.Width) Console.WindowWidth = screen.Width;
                if (Console.WindowHeight < screen.Height) Console.WindowHeight = screen.Height + 3;
                Console.CursorLeft = 0;
                Console.CursorTop = 0;
            };

            while (!board.GameOver())
            {
                var screen = DrawBoard(board, moveResult, showError);
                if (first)
                {
                    sizeConsoleWindow(screen);
                    first = false;
                }
                Console.CursorLeft = 0;
                Console.CursorTop = screen.Height + 2;

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
        private static ConsoleCellColour ErrorTextColour = new ConsoleCellColour(ConsoleColor.White, ConsoleColor.Red);
        private static ConsolePanel DrawBoard(ChessBoard board, MoveResult moveResult, bool showError)
        {
            Console.Clear();

            var boardPanel = new MediumConsoleBoard(board).Build();

            var errorPanel = new ConsolePanel(1,1);
            if (showError && !moveResult.Succeeded)
            {
                errorPanel = new TextConsolePanel(moveResult.Message, ErrorTextColour);
            }

            var screenWidth = boardPanel.Width + 4 + errorPanel.Width;
            var screen = new ConsolePanel(screenWidth, boardPanel.Height);

            screen.PrintAt(1, 1, boardPanel);
            screen.PrintAt(boardPanel.Width+3, 3, errorPanel);

           screen.ToColouredConsole()();

            return screen;
//            Console.WriteLine(screen.ToString());
            //ShowError(showError, moveResult);
        }

        private static void ShowError(bool showError, MoveResult moveResult)
        {
            if (showError)
            {
                using (new ChangeConsoleColour(ConsoleColor.Red, ConsoleColor.White))
                {
                    Console.WriteLine(moveResult.Message);
                }
            }
        }
    }
}
