using System.Collections.Generic;

namespace chess.engine.Pieces
{
    public class PawnMoveGenerator : IMoveGenerator
    {

        public IEnumerable<Path> MovesFrom(string location, Colours playerToMove)
            => MovesFrom(BoardLocation.At(location), playerToMove);

        public IEnumerable<Path> MovesFrom(BoardLocation location, Colours playerToMove)
        {
            Guard.ArgumentException(
                () => location.Rank == Move.EndRankFor(playerToMove), 
                $"{PieceName.Pawn} is invalid at {location}.");

            var paths = new List<Path>();

            var path = new Path
            {
                Move.CreateMoveOnly(location, location.MoveForward(playerToMove))
            };

            if (location.Rank == Pawn.StartRankFor(playerToMove))
            {
                path.Add(Move.CreateMoveOnly(location, location.MoveForward(playerToMove, 2)));
            }

            paths.Add(path);

            var takeType = location.Rank == Pawn.EnPassantRankFor(playerToMove)
                ? MoveType.TakeEnPassant
                : MoveType.TakeOnly;

            var takeLeft = location.MoveForward(playerToMove).MoveLeft(playerToMove);
            if (takeLeft != null)
            {
                paths.Add(new Path
                {
                    Move.Create(location, takeLeft, takeType)
                });
            }

            var takeRight = location.MoveForward(playerToMove).MoveRight(playerToMove);
            if (takeRight != null)
            {
                paths.Add(new Path
                {
                    Move.Create(location, takeRight, takeType)
                });
            }

            return paths;
        }

    }
}