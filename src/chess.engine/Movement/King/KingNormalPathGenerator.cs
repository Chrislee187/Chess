using System.Collections.Generic;
using chess.engine.Game;

namespace chess.engine.Movement.King
{
    public class KingNormalPathGenerator : IPathGenerator
    {
        public IEnumerable<Path> PathsFrom(BoardLocation location, Colours forPlayer)
        {
            var paths = new List<Path>();

            foreach (var dest in new[]
            {
                location.MoveForward(forPlayer),
                location.MoveForward(forPlayer)?.MoveRight(forPlayer),
                location.MoveRight(forPlayer),
                location.MoveBack(forPlayer)?.MoveRight(forPlayer),
                location.MoveBack(forPlayer),
                location.MoveBack(forPlayer)?.MoveLeft(forPlayer),
                location.MoveLeft(forPlayer),
                location.MoveLeft(forPlayer)?.MoveForward(forPlayer)
            })
            {
                if (dest != null)
                {
                    paths.Add(new Path { ChessMove.Create(location, dest, ChessMoveType.KingMove) });
                }
            }

            return paths;
        }

        public IEnumerable<Path> PathsFrom(string location, Colours forPlayer) => PathsFrom((BoardLocation)location, forPlayer);
    }
}