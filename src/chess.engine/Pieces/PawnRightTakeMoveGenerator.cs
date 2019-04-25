using System.Collections.Generic;

namespace chess.engine.Pieces
{
    public class PawnRightTakeMoveGenerator : IMoveGenerator
    {

        public IEnumerable<Path> MovesFrom(string location, Colours forPlayer)
            => MovesFrom(BoardLocation.At(location), forPlayer);

        public IEnumerable<Path> MovesFrom(BoardLocation location, Colours forPlayer)
        {
            Guard.ArgumentException(
                () => location.Rank == Move.EndRankFor(forPlayer), 
                $"{ChessPieceName.Pawn} is invalid at {location}.");

            var paths = new List<Path>();

            var takeType = location.Rank == Pawn.EnPassantRankFor(forPlayer)
                ? MoveType.TakeEnPassant
                : MoveType.TakeOnly;

            var oneSquareForward = location.MoveForward(forPlayer);


            var takeRight = oneSquareForward.MoveRight(forPlayer);
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