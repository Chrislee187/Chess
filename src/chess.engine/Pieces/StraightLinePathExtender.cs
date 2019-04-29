using System;
using System.Linq;
using chess.engine.Game;
using chess.engine.Movement;

namespace chess.engine.Pieces
{
    public abstract class StraightLinePathExtender 
    {
        protected Path ExtendedPathFrom(BoardLocation location, Func<BoardLocation, BoardLocation> move)
        {
            var next = move(location);
            var path = new Path();
            while (next != null)
            {
                path.Add(ChessMove.CreateMoveOrTake(location, next));
                next = move(next);
            }

            if (!path.Any()) return null;

            return path;
        }
    }
}