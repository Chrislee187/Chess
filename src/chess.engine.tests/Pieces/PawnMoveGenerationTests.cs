using System;
using System.Collections.Generic;
using System.Linq;
using chess.engine.Game;
using chess.engine.Movement;
using chess.engine.Pieces.Pawn;
using NUnit.Framework;

namespace chess.engine.tests.Pieces
{
    public class PawnMoveGenerationTests
    {
        private IPathGenerator _pathGenerator;
        private Colours[] _colours = { Colours.Black, Colours.White };

        [SetUp]
        public void Setup()
        {
            _pathGenerator = new TestPawnPathAggrator();
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
                    var directionModifer = ChessMove.DirectionModifierFor(colour);

                    var pawnStartPosition = BoardLocation.At($"{file}{startRank}");
                    var allPaths = _pathGenerator.PathsFrom(pawnStartPosition, colour).ToList();

                    var oneSquareForward = pawnStartPosition.MoveForward(colour);
                    var oneSquareLeft = oneSquareForward.MoveLeft(colour);
                    if (file != ChessFile.A && oneSquareLeft != null)
                    {
                        var takeLeftPath = new Path
                        {
                            ChessMove.Create(pawnStartPosition,oneSquareLeft, ChessMoveType.TakeOnly)
                        };

                        AssertPathContains(allPaths, takeLeftPath, colour);
                    }

                    var oneSquareRight = oneSquareForward.MoveRight(colour);
                    if (file != ChessFile.H && oneSquareRight != null)
                    {
                        var takeRightPath = new Path
                        {
                            ChessMove.Create(pawnStartPosition, oneSquareRight, ChessMoveType.TakeOnly)
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
                var allPaths = _pathGenerator.PathsFrom(location, colour).ToList();

                var expectedMovePath = new Path
                {
                    ChessMove.Create(location, location.MoveForward(colour), ChessMoveType.MoveOnly)
                };

                var expectedTakePathLeft = new Path
                {
                    ChessMove.Create(location, location.MoveForward(colour).MoveLeft(colour), ChessMoveType.TakeOnly)
                };

                var expectedTakePathRight = new Path
                {
                    ChessMove.Create(location, location.MoveForward(colour).MoveRight(colour), ChessMoveType.TakeOnly)
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
                var directionModifer = ChessMove.DirectionModifierFor(colour);
                var enpassantRank = Pawn.EnPassantRankFor(colour);
                var location = BoardLocation.At($"D{enpassantRank}");
                var allPaths = _pathGenerator.PathsFrom(location, colour).ToList();

                var expectedEnPassantTakePathLeft = new Path
                {
                    ChessMove.Create($"D{enpassantRank}", $"C{enpassantRank + directionModifer}", ChessMoveType.TakeEnPassant)
                };

                var expectedEnPassantTakePathRight = new Path
                {
                    ChessMove.Create($"D{enpassantRank}", $"E{enpassantRank + directionModifer}", ChessMoveType.TakeEnPassant)
                };

                AssertPathContains(allPaths, expectedEnPassantTakePathLeft, colour);
                AssertPathContains(allPaths, expectedEnPassantTakePathRight, colour);
            }
        }

        [Test]
        public void MovesFrom_is_not_responsible_for_enpassant() 
            => Assert.Pass("En-passant is a normal take move for a pawn, so the physical locations are not special, just the 'Take' conditions");

        private void AssertPathContains(IEnumerable<Path> paths, Path path, Colours colour) 
            => Assert.That(paths.Contains(path), $"{path} not found for {colour}, check ChessMoveType!");

        private Path CreateExpectedPawnStartPath(ChessFile file, int startRank, int directionModifier)
        {
            var oneSquareForward = $"{file}{startRank + directionModifier}";
            var twoSquaresForward = $"{file}{startRank + (directionModifier * 2)}";
            return new Path
            {
                ChessMove.Create($"{file}{startRank}", oneSquareForward, ChessMoveType.MoveOnly),
                ChessMove.Create($"{file}{startRank}", twoSquaresForward, ChessMoveType.MoveOnly)
            };
        }

        class TestPawnPathAggrator : IPathGenerator
        {
            // TODO: Don't use this aggregator, split the tests up
            public IEnumerable<Path> PathsFrom(BoardLocation location, Colours forPlayer)
            {
                return new PawnNormalAndStartingPathGenerator().PathsFrom(location, forPlayer)
                    .Concat(new PawnLeftTakePathGenerator().PathsFrom(location, forPlayer))
                    .Concat(new PawnRightTakePathGenerator().PathsFrom(location, forPlayer));

            }

            public IEnumerable<Path> PathsFrom(string location, Colours forPlayer) =>
                PathsFrom((BoardLocation) location, forPlayer);
        }
    }
}