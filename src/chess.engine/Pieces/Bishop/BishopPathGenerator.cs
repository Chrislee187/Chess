using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Authentication.ExtendedProtection;
using chess.engine.Game;
using chess.engine.Movement;

namespace chess.engine.Pieces.King
{
    public class BishopPathGenerator : IPathGenerator
    {
        public IEnumerable<Path> PathsFrom(BoardLocation location, Colours forPlayer)
        {
            var paths = new List<Path>();

            BoardLocation ForwardRight(BoardLocation start) => start.MoveForward(forPlayer)?.MoveRight(forPlayer);
            BoardLocation ForwardLeft(BoardLocation start) => start.MoveForward(forPlayer)?.MoveLeft(forPlayer);
            BoardLocation BackRight(BoardLocation start) => start.MoveBack(forPlayer)?.MoveRight(forPlayer);
            BoardLocation BackLeft(BoardLocation start) => start.MoveBack(forPlayer)?.MoveLeft(forPlayer);

            foreach (var path in new[]
            {
                ExtendedPathFrom(location, ForwardRight),
                ExtendedPathFrom(location, ForwardLeft),
                ExtendedPathFrom(location, BackRight),
                ExtendedPathFrom(location, BackLeft)
            })
            {
                if (path != null)
                {
                    paths.Add(path);
                }
            }

            return paths;
        }

        private Path ExtendedPathFrom(BoardLocation location, Func<BoardLocation, BoardLocation> move)
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

        public IEnumerable<Path> PathsFrom(string location, Colours forPlayer) => PathsFrom((BoardLocation)location, forPlayer);
    }
}