using System.Linq;
using Chess.Common.Tests.Helpers;
using CSharpChess.Extensions;
using CSharpChess.Movement;
using CSharpChess.System;
using NUnit.Framework;

namespace Chess.Common.Tests.ValidMoveGeneration.Pawns
{
    [TestFixture]
    // ReSharper disable once InconsistentNaming
    public class captures : BoardAssertions
    {
        private PawnMoveGenerator _pawnMoveGenerator;

        [SetUp]
        public void SetUp()
        {
            _pawnMoveGenerator = new PawnMoveGenerator();
        }
        [Test]
        public void can_enpassant()
        {
            var asOneChar =
                "rnbqkbnr" +
                "pppppppp" +
                "........" +
                "..P....." +
                "........" +
                "........" +
                "PP.PPPPP" +
                "RNBQKBNR";


            var board = BoardBuilder.CustomBoard(asOneChar, Colours.Black);

            var result = board.Move("d7-d5");

            var expected = BoardLocation.List("D6");
            var moves = _pawnMoveGenerator.All(board, BoardLocation.At("C5"));

            AssertMovesContainsExpectedWithType(moves, expected, MoveType.TakeEnPassant);

            result = board.Move("c5d6");
            Assert.That(result.Succeeded);

            moves = _pawnMoveGenerator.All(board, BoardLocation.At("C7"));

            AssertMovesContainsExpectedWithType(moves, expected, MoveType.Take);
        }
        [Test]
        public void cannot_take_piece_one_square_in_front_of_it()
        {
            var asOneChar =
                "rnbqkbnr" +
                "pppppppp" +
                "........" +
                "........" +
                "........" +
                "p......." +
                "PPPPPPPP" +
                "RNBQKBNR";

            var board = BoardBuilder.CustomBoard(asOneChar, Colours.White);

            var moves = _pawnMoveGenerator.All(board, BoardLocation.At("A2")).Takes().ToList();

            Assert.That(moves, Is.Empty);

        }
        [Test]
        public void cannot_take_piece_two_squares_in_front_of_it()
        {
            var asOneChar =
                "rnbqkbnr" +
                "pppppppp" +
                "........" +
                "........" +
                "p......." +
                "........" +
                "PPPPPPPP" +
                "RNBQKBNR";

            var expected = BoardLocation.List("A3");

            var board = BoardBuilder.CustomBoard(asOneChar, Colours.White);

            var moves = _pawnMoveGenerator.All(board, BoardLocation.At("A2")).Moves();

            AssertMovesContainsExpectedWithType(moves, expected, MoveType.Move);
        }
        [Test]
        public void can_take_pieces_diagonally_opposite()
        {
            var asOneChar =
                "rnbqkbnr" +
                "pppppppp" +
                "........" +
                "........" +
                "........" +
                ".p.p...." +
                "PPPPPPPP" +
                "RNBQKBNR";

            var expected = BoardLocation.List("B3", "D3");

            var board = BoardBuilder.CustomBoard(asOneChar, Colours.White);

            var moves = _pawnMoveGenerator.All(board, BoardLocation.At("C2")).Takes();

            AssertMovesContainsExpectedWithType(moves, expected, MoveType.Take);
        }
        [Test]
        public void can_take_pieces_diagonally_opposite_black()
        {
            var asOneChar =
                "rnbqkbnr" +
                "pppppppp" +
                ".P.P...." +
                "........" +
                "........" +
                "........" +
                "PPPPPPPP" +
                "RNBQKBNR";

            var expected = BoardLocation.List("B6", "D6");

            var board = BoardBuilder.CustomBoard(asOneChar, Colours.Black);

            var moves = _pawnMoveGenerator.All(board, BoardLocation.At("C7")).Takes();

            AssertMovesContainsExpectedWithType(moves, expected, MoveType.Take);
        }
    }
}