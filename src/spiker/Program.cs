using System;
using System.Linq;
using System.Text;
using chess.engine;
using chess.engine.Board;
using chess.engine.Chess;
using chess.engine.Entities;
using chess.engine.Game;
using Microsoft.Extensions.Logging;

namespace spiker
{
    class Program
    {
        static void Main(string[] args)
        {
            var logger = AppContainer.GetService<ILogger<ChessGame>>();
            var engineProvider = AppContainer.GetService<IBoardEngineProvider<ChessPieceEntity>>();
            var game = new ChessGame(logger, engineProvider);

            var lastResult = "";

            while (game.InProgress)
            {
                Console.Clear();
                Console.WriteLine("Chess console Spikes host");

                var board = new StringBoardBuilder().BuildSimpleTestBoard(game.Board);

                var lines = board.Split('\n');
                WriteLinesAt(1, 2, lines);
                Console.CursorTop = 2;
                Console.CursorLeft = lines.Max(b => b.Length) + 1;
                Console.WriteLine("MENU GOES HERE");

                Console.CursorTop = lines.Length + 3;
                Console.CursorLeft = 0;
                if (string.IsNullOrEmpty(lastResult))
                {
                    Console.WriteLine("OTHER INFORMATION OUTPUT GOES HERE");
                }
                else
                {
                    Console.WriteLine($"!!! ERROR");
                    Console.WriteLine($"!!! {lastResult}");
                    Console.WriteLine($"!!!");
                }

                Console.CursorTop = lines.Length + 1;
                Console.CursorLeft = 0;

                Console.Write($"Enter move for player {game.CurrentPlayer} : ");

                var input = Console.ReadLine();

                if (input == "quit")
                {
                    if(Quit())
                    {
                        return;
                    }
                } else if (!string.IsNullOrEmpty(input))
                {
                    lastResult = game.Move(input);
                }
            }
        }

        static void WriteLinesAt(int left, int top, params string[] output)
        {
            foreach (var line in output)
            {
                Console.CursorLeft = left;
                Console.CursorTop = top++;
                Console.Write(line);
            }
        }
        private static bool Quit()
        {
            return true;
        }
    }

    public class StringBoardBuilder
    {
        public string BuildSimpleTestBoard(LocatedItem<ChessPieceEntity>[,] board)
        {
            var sb = new StringBuilder();
            sb.AppendLine("  ABCDEFGH");
            for (int rank = 7; rank >= 0; rank--)
            {
                sb.Append($"{rank+1} ");
                for (int file = 0; file < 8; file++)
                {
                    var boardPiece = board[file, rank];

                    if (boardPiece == null)
                    {
                        sb.Append(".");
                    }
                    else
                    {
                        var entity = boardPiece.Item;
                        var piece = entity.Piece == ChessPieceName.Knight ? "N" : entity.Piece.ToString().First().ToString();
                        sb.Append(entity.Player == Colours.White ? piece.ToUpper() : piece.ToLower());
                    }
                }
                sb.Append($" {rank + 1}");

                sb.AppendLine();
            }
            sb.AppendLine("  ABCDEFGH");

            return sb.ToString();
        }
    }
}
