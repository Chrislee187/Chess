using System;
using System.Collections.Generic;
using System.Linq;
using chess.engine.Pieces;
using NUnit.Framework;

namespace chess.engine.tests.Pieces
{
    public class PawnMoveGeneratorTests
    {
        private IMoveGenerator _moveGenerator;
        private Colours[] _colours = { Colours.Black, Colours.White };

        [SetUp]
        public void Setup()
        {
            _moveGenerator = new TestPawnMoveAggrator();
        }

        [Test]
        public void MovesFrom_returns_possible_moves_from_starting_position()
        {
            // TODO: ? Split this in to more discrete tests?
            _colours = new []{Colours.Black, Colours.White};
            foreach (ChessFile file in Enum.GetValues(typeof(ChessFile)))
            {
                foreach (Colours colour in Enum.GetValues(typeof(Colours)))
                {
                    var startRank = Pawn.StartRankFor(colour);
                    var directionModifer = Move.DirectionModifierFor(colour);

                    var pawnStartPosition = BoardLocation.At($"{file}{startRank}");
                    var allPaths = _moveGenerator.MovesFrom(pawnStartPosition, colour).ToList();

                    var oneSquareForward = pawnStartPosition.MoveForward(colour);
                    var oneSquareLeft = oneSquareForward.MoveLeft(colour);
                    if (file != ChessFile.A && oneSquareLeft != null)
                    {
                        var takeLeftPath = new Path
                        {
                            Move.Create(pawnStartPosition,oneSquareLeft, MoveType.TakeOnly)
                        };

                        AssertPathContains(allPaths, takeLeftPath, colour);
                    }

                    var oneSquareRight = oneSquareForward.MoveRight(colour);
                    if (file != ChessFile.H && oneSquareRight != null)
                    {
                        var takeRightPath = new Path
                        {
                            Move.Create(pawnStartPosition, oneSquareRight, MoveType.TakeOnly)
                        };

                        AssertPathContains(allPaths, takeRightPath, colour);
                    }

                    var movePaths = CreateExpectedPawnStartPath(file, startRank, directionModifer);
                    AssertPathContains(allPaths, movePaths, colour);
                }
            }
        }

        [Test]
        public void MovesFrom_returns_possible_moves_and_takes_for_non_starting_position()
        {
            foreach (var colour in _colours)
            {
                var location = BoardLocation.At($"D3");
                var allPaths = _moveGenerator.MovesFrom(location, colour).ToList();

                var expectedMovePath = new Path
                {
                    Move.Create(location, location.MoveForward(colour), MoveType.MoveOnly)
                };

                var expectedTakePathLeft = new Path
                {
                    Move.Create(location, location.MoveForward(colour).MoveLeft(colour), MoveType.TakeOnly)
                };

                var expectedTakePathRight = new Path
                {
                    Move.Create(location, location.MoveForward(colour).MoveRight(colour), MoveType.TakeOnly)
                };

                AssertPathContains(allPaths, expectedMovePath, colour);
                AssertPathContains(allPaths, expectedTakePathLeft, colour);
                AssertPathContains(allPaths, expectedTakePathRight, colour);
            }
        }

        [Test]
        public void MovesFrom_returns_enpassant_takes_when_appropriate()
        {
            foreach (var colour in _colours)
            {
                var directionModifer = Move.DirectionModifierFor(colour);
                var enpassantRank = Pawn.EnPassantRankFor(colour);
                var location = BoardLocation.At($"D{enpassantRank}");
                var allPaths = _moveGenerator.MovesFrom(location, colour).ToList();

                var expectedEnPassantTakePathLeft = new Path
                {
                    Move.Create($"D{enpassantRank}", $"C{enpassantRank + directionModifer}", MoveType.TakeEnPassant)
                };

                var expectedEnPassantTakePathRight = new Path
                {
                    Move.Create($"D{enpassantRank}", $"E{enpassantRank + directionModifer}", MoveType.TakeEnPassant)
                };

                AssertPathContains(allPaths, expectedEnPassantTakePathLeft, colour);
                AssertPathContains(allPaths, expectedEnPassantTakePathRight, colour);
            }
        }

        [Test]
        public void MovesFrom_is_not_responsible_for_enpassant() 
            => Assert.Pass("En-passant is a normal take move for a pawn, so the physical locations are not special, just the 'Take' conditions");

        private void AssertPathContains(IEnumerable<Path> paths, Path path, Colours colour) 
            => Assert.That(paths.Contains(path), $"{path} not found for {colour}, check MoveType!");

        private Path CreateExpectedPawnStartPath(ChessFile file, int startRank, int directionModifier)
        {
            var oneSquareForward = $"{file}{startRank + directionModifier}";
            var twoSquaresForward = $"{file}{startRank + (directionModifier * 2)}";
            return new Path
            {
                Move.Create($"{file}{startRank}", oneSquareForward, MoveType.MoveOnly),
                Move.Create($"{file}{startRank}", twoSquaresForward, MoveType.MoveOnly)
            };
        }

        class TestPawnMoveAggrator : IMoveGenerator
        {
            // TODO: Don't use this aggregator, split the tests up
            public IEnumerable<Path> MovesFrom(BoardLocation location, Colours forPlayer)
            {
                return new PawnNormalAndStartingMoveGenerator().MovesFrom(location, forPlayer)
                    .Concat(new PawnLeftTakeMoveGenerator().MovesFrom(location, forPlayer))
                    .Concat(new PawnRightTakeMoveGenerator().MovesFrom(location, forPlayer));

            }

            public IEnumerable<Path> MovesFrom(string location, Colours forPlayer) =>
                MovesFrom((BoardLocation) location, forPlayer);
        }
    }
}