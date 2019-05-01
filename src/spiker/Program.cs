using System;
using System.Linq;
using System.Text;
using chess.engine.Board;
using chess.engine.Chess;
using chess.engine.Game;

namespace spiker
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Chess console Spikes host");

            var game = new ChessGame();

            var lastResult = "";

            while (game.InProgress)
            {
                Console.Clear();
                Console.WriteLine(new StringBoardBuilder().BuildSimpleTestBoard(game.Board));

                if (!string.IsNullOrEmpty(lastResult))
                {
                    Console.WriteLine($"!!! ERROR");
                    Console.WriteLine($"!!! {lastResult}");
                    Console.WriteLine($"!!!");
                }

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

        private static bool Quit()
        {
            return true;
        }
    }

    public class StringBoardBuilder
    {
        public string BuildSimpleTestBoard(BoardPiece[,] board)
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
                        var piece = boardPiece.Name == ChessPieceName.Knight ? "N" : boardPiece.Name.ToString().First().ToString();
                        sb.Append(boardPiece.Colour == Colours.White ? piece.ToUpper() : piece.ToLower());
                    }

                }
                sb.Append($" {rank + 1}");

                sb.AppendLine();
            }
            sb.AppendLine("  ABCDEFGH");

            return sb.ToString();
        }
        public string DynamicBoard(BoardPiece[,] board)
        {
            var squareWidth = 1;
            var squareHeight = 1;
            var sb = new StringBuilder();

            for (int rank = 7; rank >= 0; rank--)
            {
                for (int file = 0; file < 8; file++)
                {
                    var boardPiece = board[file, rank];

                    var piece = boardPiece == null ? "." : boardPiece.Name.ToString().First().ToString();

                    sb.Append(piece);
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }

    }
}
