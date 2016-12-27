using System;
using ConsoleStuff;
using ConsoleStuff.Panels;
using ConsoleStuff.Tests;
using ConsoleStuff.Tests.Commands;
using CSharpChess.System.Extensions;
using CSharpChess.TheBoard;

namespace CsChess
{
    class Program
    {
        private static readonly ConsoleCellColour ErrorTextColour = new ConsoleCellColour(ConsoleColor.White, ConsoleColor.Red);
        private const int ScreenWidth = 100;
        private static bool _debug;

        public Program()
        {
        }

        static void Main(string[] args)
        {
            var board = new ChessBoard();

            MoveResult moveResult = null;
            bool first = true;

            Action<ConsolePanel> sizeConsoleWindow = (screen) =>
            {
                if (Console.WindowWidth < screen.Width) Console.WindowWidth = screen.Width;
                if (Console.WindowHeight < screen.Height) Console.WindowHeight = screen.Height + 1;
                Console.CursorTop = 0;
            };

            var options = new BoardOptions();

            var commandMenu = BuildMenu(options);

            while (true)
            {
                var screen = DrawScreen(board, commandMenu, options, moveResult);
                if (first)
                {
                    sizeConsoleWindow(screen);
                    first = false;
                }
                Console.CursorTop = screen.Height -1;

                var cmd = GetCommand(board);

                if (string.IsNullOrEmpty(cmd))
                {
                    // Do nothing and allow redraw
                }
                else if (!commandMenu.Execute(cmd))
                {
                    try
                    {
                        moveResult = board.Move(cmd);
                    }
                    catch (Exception e)
                    {
                        moveResult = InvalidMoveOrCommand(cmd, e);
                    }
                }
            }

            Console.WriteLine($"Game over, {board.GameState}");
        }

        private static CommandMenu BuildMenu(BoardOptions options)
        {
            CommandMenu _commandMenu = new CommandMenuBuilder()
                .WithItem("debug", (cargs) => _debug = !_debug)
                .WithItem("quit", (s) => Environment.Exit(0))
                .WithItem("colour", (s) => options.ColouredSquares = !options.ColouredSquares, "Toggle coloured board")
                .WithItem("locs", (s) => options.ShowRanksAndFiles = !options.ShowRanksAndFiles, "Toggle show ranks and files")
                .Build();
            return _commandMenu;
        }

        private static MoveResult InvalidMoveOrCommand(string cmd, Exception e)
        {
            var debugInfo = _debug
                ? e.StackTrace
                : "";
            return MoveResult.Failure($"Invalid Move or Command: {cmd}\n" + e.Message + debugInfo, ChessMove.Null);
        }

        private static string GetCommand(ChessBoard board)
        {
            if (board.GameOver())
            {
                Console.WriteLine($"Game over: {board.GameState}");
                Console.ReadLine();
                return "quit";
            }
            Console.Write($"{board.WhoseTurn} to play: ");
            var cmd = Console.ReadLine();
            return cmd;
        }
        private static ConsolePanel DrawScreen(ChessBoard board, CommandMenu commandMenu, BoardOptions options, MoveResult moveResult)
        {
            Console.Clear();
            var boardPanel = new MediumConsoleBoard(board).Build(options);
            var screen = new ConsolePanel(ScreenWidth, boardPanel.Height);

            var y = AddTitlePanel(screen, boardPanel.Width);
            AddErrorPanel(screen, moveResult, boardPanel.Width+ 3, y+ 2);
            AddMenuPanel(commandMenu, options, screen, boardPanel);
            {
                // Check current view states to show
                //  Movelist
                //  About
            }

            screen.PrintAt(1, 1, boardPanel);
            
            if (options.ColouredSquares)
            {
                screen.ToColouredConsole()();
            }
            else
            {
                Console.WriteLine(screen.ToString());
            }
            Console.CursorLeft = boardPanel.Width + 2;

            return screen;
        }

        private static int AddTitlePanel(ConsolePanel screen, int boardPanelWidth)
        {
            var y = 1;
            var x = (ScreenWidth - boardPanelWidth + 2) / 3;
            screen.PrintAt(boardPanelWidth +2 + x, y, "---=== CsChess V0.1 ===---");
            return y;
        }

        private static void AddMenuPanel(CommandMenu commandMenu, BoardOptions options, ConsolePanel screen,
            ConsolePanel boardPanel)
        {
            if (options.ShowMenu)
            {
                var menuPanel = CreateMenuPanel(commandMenu);
                if (menuPanel != null)
                {
                    screen.PrintAt(boardPanel.Width + 3, boardPanel.Height - (menuPanel.Height + 1), menuPanel);
                }
            }
        }

        private static void AddErrorPanel(ConsolePanel screen, MoveResult moveResult, int x, int y)
        {
            if (moveResult != null && !moveResult.Succeeded)
            {
                var errorPanel = CreateErrorPanel(moveResult);
                if (errorPanel != null)
                {
                    screen.PrintAt(x, y, errorPanel);
                }
            }
        }

        private static ConsolePanel CreateMenuPanel(CommandMenu commandMenu)
        {
            return new TextConsolePanel(commandMenu.HelpText());
        }

        private static ConsolePanel CreateErrorPanel(MoveResult moveResult)
        {

            var textConsolePanel = new TextConsolePanel(moveResult.Message, 60, ErrorTextColour);
            var borderPanel = new ConsolePanel(textConsolePanel.Width + 4, textConsolePanel.Height + 4);
            borderPanel.Fill('*', ErrorTextColour);
            borderPanel.PrintAt(2, 1, "ERROR");
            borderPanel.PrintAt(3, 3, textConsolePanel);

            return borderPanel;
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
