using board.engine;
using board.engine.Movement;
using chess.engine.Game;

namespace chess.engine.Movement.ChessPieces.Rook
{
    public class RookPathGenerator : StraightLinePathExtender, IPathGenerator
    {
        public Paths PathsFrom(BoardLocation location, int forPlayer)
        {
            var paths = new Paths();

            var platerIdx = (Colours)forPlayer;
            foreach (var path in new[]
            {
                ExtendedPathFrom(location, loc => loc.MoveForward(platerIdx)),
                ExtendedPathFrom(location, loc => loc.MoveBack(platerIdx)),
                ExtendedPathFrom(location, loc => loc.MoveLeft(platerIdx)),
                ExtendedPathFrom(location, loc => loc.MoveRight(platerIdx)),
            })
            {
                if (path != null)
                {
                    paths.Add(path);
                }
            }

            return paths;
        }
    }
}