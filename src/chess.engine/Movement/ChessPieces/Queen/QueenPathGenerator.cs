using board.engine;
using board.engine.Movement;
using chess.engine.Movement.ChessPieces.Bishop;
using chess.engine.Movement.ChessPieces.Rook;

namespace chess.engine.Movement.ChessPieces.Queen
{
    public class QueenPathGenerator : IPathGenerator
    {
        public Paths PathsFrom(BoardLocation location, int forPlayer)
        {
            var paths = new Paths();
            paths.AddRange(new RookPathGenerator().PathsFrom(location, forPlayer));
            paths.AddRange(new BishopPathGenerator().PathsFrom(location, forPlayer));

            return paths;
        }
   }
}