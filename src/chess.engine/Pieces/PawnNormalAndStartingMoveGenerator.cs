using System.Collections.Generic;

namespace chess.engine.Pieces
{
    public class PawnNormalAndStartingMoveGenerator : IMoveGenerator
    {
        public IEnumerable<Path> MovesFrom(BoardLocation location, Colours forPlayer)
        {
            var paths = new List<Path>();

            var oneSquareForward = location.MoveForward(forPlayer);
            var path = new Path
            {
                Move.CreateMoveOnly(location, oneSquareForward)
            };

            if (location.Rank == Pawn.StartRankFor(forPlayer))
            {
                path.Add(Move.CreateMoveOnly(location, location.MoveForward(forPlayer, 2)));
            }

            paths.Add(path);

            return paths;
        }

        public IEnumerable<Path> MovesFrom(string location, Colours forPlayer) => MovesFrom((BoardLocation) location, forPlayer);
    }
}