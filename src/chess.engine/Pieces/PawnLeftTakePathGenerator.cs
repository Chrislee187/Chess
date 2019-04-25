using System.Collections.Generic;

namespace chess.engine.Pieces
{
    public class PawnLeftTakePathGenerator : IPathGenerator
    {
        public IEnumerable<Path> PathsFrom(BoardLocation location, Colours forPlayer)
        {

            var paths = new List<Path>();

            var takeType = location.Rank == Pawn.EnPassantRankFor(forPlayer)
                ? ChessMoveType.TakeEnPassant
                : ChessMoveType.TakeOnly;

            var oneSquareForward = location.MoveForward(forPlayer);
            var takeLeft = oneSquareForward.MoveLeft(forPlayer);
            if (takeLeft != null)
            {
                paths.Add(new Path
                {
                    ChessMove.Create(location, takeLeft, takeType)
                });
            }

            return paths;
        }

        public IEnumerable<Path> PathsFrom(string location, Colours forPlayer) => PathsFrom((BoardLocation)location, forPlayer);
    }
}