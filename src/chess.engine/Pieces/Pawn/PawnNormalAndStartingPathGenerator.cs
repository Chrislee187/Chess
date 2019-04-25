using System.Collections.Generic;
using chess.engine.Game;
using chess.engine.Movement;

namespace chess.engine.Pieces.Pawn
{
    public class PawnNormalAndStartingPathGenerator : IPathGenerator
    {
        public IEnumerable<Path> PathsFrom(BoardLocation location, Colours forPlayer)
        {
            var paths = new List<Path>();

            var oneSquareForward = location.MoveForward(forPlayer);
            var path = new Path
            {
                ChessMove.CreateMoveOnly(location, oneSquareForward)
            };

            if (location.Rank == Pawn.StartRankFor(forPlayer))
            {
                path.Add(ChessMove.CreateMoveOnly(location, location.MoveForward(forPlayer, 2)));
            }

            paths.Add(path);

            return paths;
        }

        public IEnumerable<Path> PathsFrom(string location, Colours forPlayer) => PathsFrom((BoardLocation) location, forPlayer);
    }
}