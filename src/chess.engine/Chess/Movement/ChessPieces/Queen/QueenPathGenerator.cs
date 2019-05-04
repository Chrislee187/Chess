using chess.engine.Chess.Movement.ChessPieces.Bishop;
using chess.engine.Chess.Movement.ChessPieces.Rook;
using chess.engine.Game;
using chess.engine.Movement;

namespace chess.engine.Chess.Movement.ChessPieces.Queen
{
    public class QueenPathGenerator : IPathGenerator
    {
        public Paths PathsFrom(BoardLocation location, Colours forPlayer)
        {
            var paths = new Paths();
            paths.AddRange(new RookPathGenerator().PathsFrom(location, forPlayer));
            paths.AddRange(new BishopPathGenerator().PathsFrom(location, forPlayer));

            return paths;
        }

        public Paths PathsFrom(string location, Colours forPlayer) => PathsFrom((BoardLocation)location, forPlayer);
    }
}