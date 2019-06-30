using System.Linq;
using board.engine.Movement;
using chess.engine.Extensions;
using chess.engine.Game;
using chess.engine.Movement;
using NUnit.Framework;

namespace chess.engine.tests.Movement
{
    [TestFixture]
    public class FindAttackPathsTests
    {
        [Test]
        public void Attacking_returns_straight_paths()
        {
            var loc = "D4".ToBoardLocation();

            var finder = new FindAttackPaths();
            var attackPaths = finder.Attacking(loc);
            var paths = attackPaths.Straight;
            AssertExpectedPaths(paths, new[]
            {
                "D1", "D2", "D3",
                "D5", "D6", "D7", "D8",
                "E4", "F4", "G4", "H4",
                "A4", "B4", "C4"
            });
        }

        [Test]
        public void Attacking_returns_diagonal_paths()
        {
            var loc = "D4".ToBoardLocation();

            var finder = new FindAttackPaths();

            var attackPaths = finder.Attacking(loc);
            var paths = attackPaths.Diagonal;
            Assert.True(paths.Any());

            AssertExpectedPaths(paths, new[]
            {
                "E5", "F6", "G7", "H8",
                "C3", "B2", "A1",
                "C5", "B6", "A7",
                "E3", "F2", "G1"
            });
        }

        [Test]
        public void Attacking_returns_knight_paths()
        {
            var loc = "D4".ToBoardLocation();

            var finder = new FindAttackPaths();

            var attackPaths = finder.Attacking(loc);
            var paths = attackPaths.Knight;
            Assert.True(paths.Any());

            AssertExpectedPaths(paths, new[]
            {
                "E6", "F5", "F3", "E2", "C2", "B3", "B5", "C6"
            });
        }

        [Test]
        public void Attacking_returns_pawn_paths_for_white()
        {
            var loc = "D4".ToBoardLocation();

            var finder = new FindAttackPaths();

            var attackPaths = finder.Attacking(loc);
            var paths = attackPaths.Pawns;
            Assert.True(paths.Any());

            AssertExpectedPaths(paths, new[]
            {
                "C5", "E5"
            });
        }

        [Test]
        public void Attacking_returns_pawn_paths_for_black()
        {
            var loc = "D4".ToBoardLocation();

            var finder = new FindAttackPaths();

            var attackPaths = finder.Attacking(loc, Colours.Black);
            var paths = attackPaths.Pawns;
            Assert.True(paths.Any());

            AssertExpectedPaths(paths, new[]
            {
                "C3", "E3"
            });
        }

        private void AssertExpectedPaths(Paths paths, string[] moveTos)
        {
            Assert.That(moveTos.Count, Is.EqualTo(paths.FlattenMoves().Count()));

            foreach (var moveTo in moveTos)
            {
                AssertPathsContainsMoveTo(paths, moveTo);
            }
        }

        private void AssertPathsContainsMoveTo(Paths paths, string loc)
        {
            Assert.That(paths.ContainsMoveTo(loc.ToBoardLocation()), $"expected to find {loc} paths");
        }
    }
}