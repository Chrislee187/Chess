using System;
using System.Collections.Generic;
using System.Linq;
using chess.engine.Pieces;
using NUnit.Framework;

namespace chess.engine.tests.Pieces
{
    public class PawnTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase(ChessFile.B)]
        [TestCase(ChessFile.C)]
        [TestCase(ChessFile.D)]
        [TestCase(ChessFile.E)]
        [TestCase(ChessFile.F)]
        [TestCase(ChessFile.G)]
        public void MovesFrom_returns_possible_starting_moves_for_non_edge_pieces(ChessFile file)
        {
            foreach (var colour in new []{Colours.Black, Colours.White})
            {
                var startRank = Pawn.StartRankFor(colour);
                var directionModifer = Move.DirectionModifierFor(colour);
                var allMoves = Pawn.MovesFrom($"{file}{startRank}", colour);

                Assert.That(allMoves.Count, Is.EqualTo(4));

                var expectedMoves = CreateExpectedPawnStartMoves(file, startRank, directionModifer);

                var moves = allMoves.Where(m => m.MoveType == MoveType.MoveOnly).ToList();
                AssertExpectedStartMoves(moves, expectedMoves, colour);

                var takes = allMoves.Where(m => m.MoveType == MoveType.TakeOnly).ToList();
                var take1 = Move.Create($"{file}{startRank}", $"{file - 1}{startRank + directionModifer}");
                var take2 = Move.Create($"{file}{startRank}", $"{file + 1}{startRank + directionModifer}");

                Assert.True(takes.Contains(take1), $"{take1} not found for {colour}");
                Assert.True(takes.Contains(take2), $"{take2} not found for {colour}");
            }

        }

        [TestCase(ChessFile.A)]
        [TestCase(ChessFile.H)]
        public void MovesFrom_returns_possible_starting_moves_for_edge_pieces(ChessFile file)
        {
            foreach (var colour in new[] {Colours.Black, Colours.White})
            {
                var startRank = Pawn.StartRankFor(colour);
                var directionModifer = Move.DirectionModifierFor(colour);
                var allMoves = Pawn.MovesFrom($"{file}{startRank}", colour);

                Assert.That(allMoves.Count, Is.EqualTo(3));

                var moves = allMoves.Where(m => m.MoveType == MoveType.MoveOnly).ToList();
                var expectedMoves = CreateExpectedPawnStartMoves(file, startRank, directionModifer);
                AssertExpectedStartMoves(moves, expectedMoves, colour);

                var takes = allMoves.Where(m => m.MoveType == MoveType.TakeOnly).ToList();

                var take = file == ChessFile.A
                    ? Move.Create($"{file}{startRank}", $"{file + 1}{startRank + directionModifer}")
                    : Move.Create($"{file}{startRank}", $"{file - 1}{startRank + directionModifer}");

                Assert.True(takes.Contains(take));
            }
        }

        [Test]
        public void MovesFrom_returns_possible_moves_and_takes_for_normal_moves()
        {
            foreach (var colour in new[] {Colours.Black, Colours.White})
            {
                var startRank = Pawn.StartRankFor(colour);
                var directionModifer = Move.DirectionModifierFor(colour);

                var allMoves = Pawn.MovesFrom($"D4", colour).ToList();

                Assert.That(allMoves.Count, Is.EqualTo(3));

                var moves = allMoves.Where(m => m.MoveType == MoveType.MoveOnly);
                var move1 = Move.Create($"D4", colour.ConvertTo("D5","D3"));

                Assert.True(moves.Contains(move1), $"{move1} not found for {colour}");

                var takes = allMoves.Where(m => m.MoveType == MoveType.TakeOnly);
                var take1 = Move.Create($"D4", colour.ConvertTo("C5" , "C3"));
                var take2 = Move.Create($"D4", colour.ConvertTo("E5" , "E3"));

                Assert.True(takes.Contains(take1), $"{take1} not found for {colour}");
                Assert.True(takes.Contains(take2), $"{take2} not found for {colour}");
            }
        }

        [Test]
        public void MovesFrom_is_not_responsible_for_enpassant()
        {
            Assert.Pass("En-passant is a normal take move for a pawn, so the physical locations are not special, just the 'Take' conditions");
        }

        private static void AssertExpectedStartMoves(List<Move> moves, (Move move1, Move move2) expectedMoves, Colours colour)
        {
            Assert.True(moves.Contains(expectedMoves.move1), $"{expectedMoves.move1} not found for {colour}");
            Assert.True(moves.Contains(expectedMoves.move2), $"{expectedMoves.move2} not found for {colour}");
        }

        private (Move move1, Move move2) CreateExpectedPawnStartMoves(ChessFile file, int startRank, int directionModifier)
        {
            var move1 = Move.Create($"{file}{startRank}", $"{file}{startRank + directionModifier}");
            var move2 = Move.Create($"{file}{startRank}", $"{file}{startRank + (directionModifier * 2)}");
            return (move1, move2);
        }
    }
}