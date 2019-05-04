using chess.engine.Game;
using chess.engine.Movement;

namespace chess.engine.Chess.Movement.ChessPieces.King
{
    public class KingCastlePathGenerator : IPathGenerator
    {
        public Paths PathsFrom(BoardLocation location, Colours forPlayer)
        {
            var paths = new Paths();

            foreach (var dest in new[]
            {
                location.MoveRight(forPlayer,2),
                location.MoveLeft(forPlayer,2),
            })
            {
                if (dest != null)
                {
                    var side = dest.File > location.File
                            ? MoveType.CastleKingSide
                            : MoveType.CastleQueenSide;

                    paths.Add(new Path { BoardMove.Create(location, dest, side) });
                }
            }

            return paths;
        }

        public Paths PathsFrom(string location, Colours forPlayer) => PathsFrom((BoardLocation)location, forPlayer);
    }
}