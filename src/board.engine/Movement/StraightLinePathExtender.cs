using System;
using System.Linq;
using board.engine.Actions;

namespace board.engine.Movement
{
    public abstract class StraightLinePathExtender 
    {
        protected Path ExtendedPathFrom(BoardLocation location, Func<BoardLocation, BoardLocation> move)
        {
            var next = move(location);
            var path = new Path();
            while (next != null)
            {
                path.Add(new BoardMove(location, next, (int)DefaultActions.MoveOrTake));
                next = move(next);
            }

            if (!path.Any()) return null;

            return path;
        }
    }
}