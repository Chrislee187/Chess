using System;
using System.Linq;
using CSharpChess;
using CSharpChess.System;
using CSharpChess.TheBoard;

namespace CsChess.Pgn
{
    public class PgnGameResolver
    {
        private PgnGame _pgnGame;

        public ChessBoard Resolve(PgnGame pgnGame)
        {
            _pgnGame = pgnGame;
            var board = new ChessBoard();

            foreach (var pgnTurnQuery in pgnGame.TurnQueries)
            {
                ResolveMove(board, pgnTurnQuery.White);
                ResolveMove(board, pgnTurnQuery.Black);
            }

            return board;
        }

        private void ResolveMove(ChessBoard board, PgnQuery pgnQuery)
        {
            try
            {
                if (!pgnQuery.QueryResolved)
                    pgnQuery.ResolveQuery(board);

                if (!pgnQuery.GameOver)
                    board.Move(pgnQuery.ToMove());
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error resolving {pgnQuery}");
                Console.WriteLine($"{_pgnGame.White} vs {_pgnGame.Black} - {_pgnGame.Event} Round {_pgnGame.Round}");
                Console.WriteLine(board.ToAsciiBoard());
                Console.WriteLine(string.Join(" ", board.Moves.Select(m=> m.ToString())));
                Console.WriteLine(e);
                throw;
            }
        }
    }
}