using System;
using System.Collections.Concurrent;
using board.engine;
using board.engine.Actions;
using board.engine.Movement;
using chess.engine.Game;
using chess.engine.Movement.Bishop;
using chess.engine.Movement.Knight;
using chess.engine.Movement.Rook;

namespace chess.engine.Movement
{
    public interface IFindAttackPaths
    {
        AttackPaths Attacking(BoardLocation at, Colours defendingPlayer = Colours.White);
    }

    public class FindAttackPaths : IFindAttackPaths
    {
        // TODO: Test the caching??
        private readonly ConcurrentDictionary<(BoardLocation, Colours), AttackPaths> _cache = new ConcurrentDictionary<(BoardLocation, Colours), AttackPaths>();
        public AttackPaths Attacking(BoardLocation at, Colours defendingPlayer = Colours.White)
        {
            var key = (at, defendingPlayer);
            if (_cache.TryGetValue(key, out var attacks))
            {
                return attacks;
            }


            var straightPaths = new RookPathGenerator().PathsFrom(at, (int) Colours.White);
            var diagonalPaths = new BishopPathGenerator().PathsFrom(at, (int)Colours.White);
            var knightPaths = new KnightPathGenerator().PathsFrom(at, (int)Colours.White);

            var pawnPaths = new Paths();

            var pawnPos1 = at.MoveForward(defendingPlayer)?.MoveLeft(defendingPlayer);
            if (pawnPos1 != null)
            {
                var path = new Path();
                path.Add(new BoardMove(at, pawnPos1, (int) DefaultActions.TakeOnly));
                pawnPaths.Add(path);
            }

            var pawnPos2 = at.MoveForward(defendingPlayer)?.MoveRight(defendingPlayer);
            if (pawnPos2 != null)
            {
                var path = new Path();
                path.Add(new BoardMove(at, pawnPos2, (int)DefaultActions.TakeOnly));
                pawnPaths.Add(path);
            }

            var attackPaths = new AttackPaths(straightPaths, diagonalPaths, knightPaths, pawnPaths);
            if (_cache.TryAdd(key, attackPaths))
            {
                return attackPaths;
            }

            throw new SystemException("Failed to add AttackPaths to cache");
        }

    }

    public class AttackPaths
    {
        public AttackPaths(Paths straight, Paths diagonal, Paths knight, Paths pawns)
        {
            Straight = straight;
            Diagonal = diagonal;
            Knight = knight;
            Pawns = pawns;
        }

        public Paths Straight { get; }
        public Paths Diagonal { get; set; }
        public Paths Knight { get; set; }
        public Paths Pawns { get; private set; }
    }

}