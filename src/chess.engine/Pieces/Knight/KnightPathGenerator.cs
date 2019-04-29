using System.Collections.Generic;
using chess.engine.Game;
using chess.engine.Movement;

namespace chess.engine.Pieces.Knight
{
    public class KnightPathGenerator : IPathGenerator
    {
        public IEnumerable<Path> PathsFrom(BoardLocation location, Colours forPlayer)
        {
            var paths = new List<Path>();

            foreach (var dest in new[]
            {
                location.KnightVerticalMove(forPlayer, true, true),
                location.KnightVerticalMove(forPlayer, true, false),
                location.KnightVerticalMove(forPlayer, false, true),
                location.KnightVerticalMove(forPlayer, false, false),

                location.KnightHorizontalMove(forPlayer, true, true),
                location.KnightHorizontalMove(forPlayer, true, false),
                location.KnightHorizontalMove(forPlayer, false, true),
                location.KnightHorizontalMove(forPlayer, false, false),
            })
            {
                if (dest != null)
                {
                    paths.Add(new Path { ChessMove.Create(location, dest, ChessMoveType.MoveOrTake) });
                }
            }

            return paths;
        }

        public IEnumerable<Path> PathsFrom(string location, Colours forPlayer) => PathsFrom((BoardLocation)location, forPlayer);
    }
}