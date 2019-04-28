using System.Collections.Generic;
using chess.engine.Game;
using chess.engine.Movement;

namespace chess.engine.Pieces.King
{
    public class KingNormalPathGenerator : IPathGenerator
    {
        public IEnumerable<Path> PathsFrom(BoardLocation location, Colours forPlayer)
        {
            var paths = new List<Path>();

            var n = location.MoveForward(forPlayer);
            var ne = location.MoveForward(forPlayer)?.MoveRight(forPlayer);
            var e = location.MoveRight(forPlayer);
            var se = location.MoveBack(forPlayer)?.MoveRight(forPlayer);
            var s = location.MoveBack(forPlayer);
            var sw = location.MoveBack(forPlayer)?.MoveLeft(forPlayer);
            var w = location.MoveLeft(forPlayer);
            var nw = location.MoveLeft(forPlayer)?.MoveForward(forPlayer);

            foreach (var dest in new BoardLocation[]{n, ne, e, se, s, sw, w ,nw})
            {
                if (dest != null)
                {
                    paths.Add(new Path{ChessMove.Create(location, dest, ChessMoveType.KingMove)});
                }
            }

            return paths;
        }

        public IEnumerable<Path> PathsFrom(string location, Colours forPlayer) => PathsFrom((BoardLocation) location, forPlayer);
    }
}