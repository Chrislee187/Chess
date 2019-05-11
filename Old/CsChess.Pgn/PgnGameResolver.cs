using System;
using System.Linq;
using CSharpChess;
using CSharpChess.Extensions;

namespace CsChess.Pgn
{
    public class PgnGameResolver
    {
        private PgnGame _pgnGame;

        public Board Resolve(PgnGame pgnGame)
        {
            _pgnGame = pgnGame;
            var board = new Board();
            foreach (var pgnTurnQuery in pgnGame.TurnQueries)
            {
//                Console.WriteLine($"{board.ToAsciiBoard()}");
                ResolveMove(board, pgnTurnQuery.White);
                ResolveMove(board, pgnTurnQuery.Black);
//                Console.WriteLine($"Move: { pgnTurnQuery.White} - { pgnTurnQuery.Black}");
            }

            return board;
        }

        private void ResolveMove(Board board, PgnQuery pgnQuery)
        {
            try
            {
                if (!pgnQuery.QueryResolved)
                    pgnQuery.ResolveQuery(board);

                if (!pgnQuery.GameOver)
                {
                    var from = BoardLocation.At(pgnQuery.FromFile, pgnQuery.FromRank);
                    var to = BoardLocation.At(pgnQuery.ToFile, pgnQuery.ToRank);
                    var move = new Move(from, to, pgnQuery.MoveType, pgnQuery.PromotionPiece, pgnQuery.PgnText);
                    board.Move(move);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error resolving {pgnQuery}");
                Console.WriteLine($"{_pgnGame.White} vs {_pgnGame.Black} - {_pgnGame.Event} Round {_pgnGame.Round}");
                Console.WriteLine(board.ToAsciiBoard());
                Console.WriteLine(string.Join(" ", board.Moves.Select(m=> m.ToString())));
                throw;
            }
        }
    }
}