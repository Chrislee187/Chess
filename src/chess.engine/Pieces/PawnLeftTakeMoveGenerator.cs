using System.Collections.Generic;

namespace chess.engine.Pieces
{
    public class PawnLeftTakeMoveGenerator : IMoveGenerator
    {
        public IEnumerable<Path> MovesFrom(BoardLocation location, Colours forPlayer)
        {

            var paths = new List<Path>();

            var takeType = location.Rank == Pawn.EnPassantRankFor(forPlayer)
                ? MoveType.TakeEnPassant
                : MoveType.TakeOnly;

            var oneSquareForward = location.MoveForward(forPlayer);
            var takeLeft = oneSquareForward.MoveLeft(forPlayer);
            if (takeLeft != null)
            {
                paths.Add(new Path
                {
                    Move.Create(location, takeLeft, takeType)
                });
            }

            return paths;
        }

        public IEnumerable<Path> MovesFrom(string location, Colours forPlayer) => MovesFrom((BoardLocation)location, forPlayer);
    }
}