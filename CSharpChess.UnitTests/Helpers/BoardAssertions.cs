using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CSharpChess.System.Extensions;
using CSharpChess.TheBoard;
using NUnit.Framework;
#pragma warning disable 162

namespace CSharpChess.UnitTests.Helpers
{
    [SuppressMessage("ReSharper", "UnusedParameter.Global")]
    public class BoardAssertions
    {
        protected const string NoPawnBoard = "rnbqkbnr" +
                                           "........" +
                                           "........" +
                                           "........" +
                                           "........" +
                                           "........" +
                                           "........" +
                                           "RNBQKBNR";

        protected static void AssertNewGameBoard(ChessBoard board)
        {
            var ranks = new OneCharBoard(board).Ranks.ToList();
            Assert.That(ranks[7], Is.EqualTo("rnbqkbnr"));
            Assert.That(ranks[6], Is.EqualTo("pppppppp"));
            Assert.That(ranks[5], Is.EqualTo("........"));
            Assert.That(ranks[4], Is.EqualTo("........"));
            Assert.That(ranks[3], Is.EqualTo("........"));
            Assert.That(ranks[2], Is.EqualTo("........"));
            Assert.That(ranks[1], Is.EqualTo("PPPPPPPP"));
            Assert.That(ranks[0], Is.EqualTo("RNBQKBNR"));
        }

        protected static void AssertMoveSucceeded(MoveResult result, ChessBoard board, string move, ChessPiece chessPiece, MoveType moveType = MoveType.Move) 
            => AssertMoveTypeSucceeded(result, board, move, chessPiece, moveType);

        protected static void AssertTakeSucceeded(MoveResult result, ChessBoard board, string move, ChessPiece chessPiece, MoveType moveType = MoveType.Take) 
            => AssertMoveTypeSucceeded(result, board, move, chessPiece, moveType);

        private static void AssertMoveTypeSucceeded(MoveResult result, ChessBoard board, string move, ChessPiece chessPiece, MoveType moveType)
        {
            var m = (ChessMove)move;
            Assert.True(result.Succeeded, result.Message);
            Assert.That(result.MoveType, Is.EqualTo(moveType));
            Assert.True(board.IsEmptyAt(m.From), $"Move start square '{m.From}' not empty, contains '{board[m.From].Piece}'.");
            Assert.True(board.IsNotEmptyAt(m.To), "Move destination square empty.");
            Assert.True(board[m.To].Piece.Is(chessPiece.Colour, chessPiece.Name),
                $"'{board[m.From].Piece}' found at destination, expected' {chessPiece}'");
        }

        protected static void AssertMovesContainsExpectedWithType(IEnumerable<ChessMove> actual,
            IEnumerable<BoardLocation> expected, MoveType moveType)
        {
            var expectedLocations = expected as IList<BoardLocation> ?? expected.ToList();
            var actualMoves = actual as IList<ChessMove> ?? actual.ToList();

            if (expectedLocations.None() || actualMoves.None()) Assert.Fail("No moves found!");

            var startLoc = actualMoves.First().From;
            var expectedMoves = expectedLocations.Select(e =>
                new ChessMove(startLoc, e, moveType)
                );

            var movesOfType = actualMoves.Where(m => m.MoveType == moveType).ToList();
            CollectionAssert.AreEquivalent(expectedMoves, movesOfType);

            Assert.That(movesOfType.Count(), Is.EqualTo(expectedLocations.Count()));
        }

        protected static void AssertMovesContains(IEnumerable<ChessMove> moves, IEnumerable<string> locations, MoveType moveType)
        {
            foreach (var location in locations)
            {
                AssertMovesContains(moves, location, moveType);
            }
        }

        protected static void AssertMovesDoesNotContain(IEnumerable<ChessMove> moves, IEnumerable<string> locations, MoveType moveType)
        {
            foreach (var location in locations)
            {
                var found = moves.FirstOrDefault(m =>
                   m.To.Equals(BoardLocation.At(location))
                   && m.MoveType == moveType);

                Assert.IsNull(found, $"MoveType of '{moveType}' to '{location}' unexpectedly found in '{string.Join(",", moves)}'!");
            }
        }
        private static void AssertMovesContains(IEnumerable<ChessMove> moves, string location, MoveType moveType)
        {
            var found = moves.FirstOrDefault( m => 
                m.To.Equals(BoardLocation.At(location))
                && m.MoveType == moveType);

            Assert.IsNotNull(found, $"MoveType of '{moveType}' to '{location}' not found in '{string.Join(",",moves)}'!");
        }

        protected static void AssertAllMovesAreOfType(IEnumerable<ChessMove> moves, MoveType moveType) 
            => Assert.That(moves.All(m => m.MoveType == moveType), "Unexpected MoveType found");

        protected IEnumerable<BoardLocation> BuildVerticalThreats(BoardLocation fromPieceAtLocation, int vertDirectionModifier)
        {
            var expected = new List<BoardLocation>();
            for (int i = 1; i <= 7; i++)
            {
                var rank = fromPieceAtLocation.Rank + (i * vertDirectionModifier);
                if (Chess.Board.Validations.IsValidLocation((int)fromPieceAtLocation.File, rank))
                {
                    expected.Add(BoardLocation.At(fromPieceAtLocation.File, rank));
                }
            }

            return expected;
        }

        protected static void DumpBoardToConsole(ChessBoard board) 
            => new MediumConsoleBoard(board).Build()
                .ToStrings().ToList()
                .ForEach(Console.WriteLine);

        protected static void DumpBoardLocations(IEnumerable<BoardLocation> attacking)
        {
            var boardLocations = attacking as IList<BoardLocation> ?? attacking.ToList();
            Console.Write($"{string.Join(",", boardLocations)}");
            Console.WriteLine($" - {boardLocations.Count()}");
        }
    }
}