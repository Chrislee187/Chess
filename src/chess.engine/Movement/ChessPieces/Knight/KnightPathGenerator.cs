using chess.engine.Game;

namespace chess.engine.Movement.ChessPieces.Knight
{
    public class KnightPathGenerator : IPathGenerator
    {
        public Paths PathsFrom(BoardLocation location, Colours forPlayer)
        {
            var paths = new Paths();

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
                    paths.Add(new Path { BoardMove.Create(location, dest, MoveType.MoveOrTake) });
                }
            }

            return paths;
        }

        public Paths PathsFrom(string location, Colours forPlayer) => PathsFrom((BoardLocation)location, forPlayer);
    }
}