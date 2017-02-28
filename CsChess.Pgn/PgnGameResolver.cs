using CSharpChess.TheBoard;

namespace CsChess.Pgn
{
    public class PgnGameResolver
    {
        public ChessBoard Resolve(PgnGame pgnGame)
        {
            var board = new ChessBoard();

            foreach (var pgnTurnQuery in pgnGame.TurnQueries)
            {
                if (!pgnTurnQuery.White.QueryResolved)
                    pgnTurnQuery.White.ResolveQuery(board);

                if (!pgnTurnQuery.White.GameOver)
                    board.Move(pgnTurnQuery.White.ToString());


                if (!pgnTurnQuery.Black.QueryResolved)
                    pgnTurnQuery.Black.ResolveQuery(board);

                if (!pgnTurnQuery.Black.GameOver)
                    board.Move(pgnTurnQuery.Black.ToString());
            }

            return board;
        }
    }
}