using System.Collections.Generic;
using chess.engine.Game;
using chess.engine.Movement;

namespace chess.engine.Pieces.King
{
    public class KingCastlePathGenerator : IPathGenerator
    {
        public IEnumerable<Path> PathsFrom(BoardLocation location, Colours forPlayer)
        {
            var paths = new List<Path>();

            foreach (var dest in new[]
            {
                location.MoveRight(forPlayer,2),
                location.MoveLeft(forPlayer,2),
            })
            {
                if (dest != null)
                {
                    var side = dest.File > location.File
                            ? ChessMoveType.CastleKingSide
                            : ChessMoveType.CastleQueenSide;

                    paths.Add(new Path { ChessMove.Create(location, dest, side) });
                }
            }

            return paths;
        }

        public IEnumerable<Path> PathsFrom(string location, Colours forPlayer) => PathsFrom((BoardLocation)location, forPlayer);
    }
}