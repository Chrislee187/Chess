using System.Collections.Generic;
using chess.engine.Game;
using chess.engine.Movement;

namespace chess.engine.Pieces.Pawn
{
    public class PawnRightTakePathGenerator : IPathGenerator
    {


        public IEnumerable<Path> PathsFrom(BoardLocation location, Colours forPlayer)
        {
            Guard.ArgumentException(
                () => location.Rank == ChessMove.EndRankFor(forPlayer),
                $"{ChessPieceName.Pawn} is invalid at {location}.");

            var paths = new List<Path>();

            var takeType = location.Rank == Pawn.EnPassantRankFor(forPlayer)
                ? ChessMoveType.TakeEnPassant
                : ChessMoveType.TakeOnly;

            var oneSquareForward = location.MoveForward(forPlayer);


            var takeRight = oneSquareForward.MoveRight(forPlayer);
            if (takeRight != null)
            {
                var move = ChessMove.Create(location, takeRight, takeType);
                paths.Add(new Path
                {
                    move
                });
            }

            return paths;
        }

        public IEnumerable<Path> PathsFrom(string location, Colours forPlayer) =>
            PathsFrom(BoardLocation.At(location), forPlayer);
    }
}