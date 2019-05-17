using board.engine;
using board.engine.Movement;
using chess.engine.Game;

namespace chess.engine.Movement.ChessPieces.King
{
    public class KingCastlePathGenerator : IPathGenerator
    {
        public Paths PathsFrom(BoardLocation location, int forPlayer)
        {
            var paths = new Paths();

            var playerIdx = (Colours) forPlayer;
            foreach (var dest in new[]
            {
                location.MoveRight(playerIdx,2),
                location.MoveLeft(playerIdx,2),
            })
            {
                if (dest != null)
                {
                    var side = dest.X > location.X
                            ? ChessMoveTypes.CastleKingSide
                            : ChessMoveTypes.CastleQueenSide;

                    paths.Add(new Path { BoardMove.Create(location, dest, (int) side) });
                }
            }

            return paths;
        }
    }
}