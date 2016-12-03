using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CSharpChess.TheBoard;
using NUnit.Framework;

namespace CSharpChess.UnitTests.TheBoard
{
    [SuppressMessage("ReSharper", "UnusedParameter.Global")]
    public class BoardAssertions
    {
        protected static void AssertNewGameBoard(ChessBoard board)
        {
            var ranks = DumpBoardToConsole(board);
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
        {
            var m = (ChessMove)move;
            Assert.True(result.Succeeded);
            Assert.That(result.MoveType, Is.EqualTo(moveType));
            Assert.True(board.IsEmptyAt(m.From), $"Move start square '{m.From}' not empty, contains '{board[m.From].Piece}'.");
            Assert.True(board.IsNotEmptyAt(m.To), "Move destination square empty.");
            Assert.True(board[m.To].Piece.Is(chessPiece.Colour, chessPiece.Name), $"'{board[m.From].Piece}' found at destination, expected' {chessPiece}'");
        }
        protected static void AssertTakeSucceeded(MoveResult result, ChessBoard board, string move, ChessPiece chessPiece, MoveType moveType = MoveType.Take)
        {
            var m = (ChessMove)move;
            Assert.True(result.Succeeded);
            Assert.That(result.MoveType, Is.EqualTo(moveType));
            Assert.True(board.IsEmptyAt(m.From), $"Move start square '{m.From}' not empty, contains '{board[m.From].Piece}'.");
            Assert.True(board.IsNotEmptyAt(m.To), "Move destination square empty.");
            Assert.True(board[m.To].Piece.Is(chessPiece.Colour, chessPiece.Name), $"'{board[m.From].Piece}' found at destination, expected' {chessPiece}'");
        }

        protected static void AssertMovesAreAsExpected(IEnumerable<ChessMove> actual, IEnumerable<BoardLocation> expected)
        {
            var expectedLocations = expected as IList<BoardLocation> ?? expected.ToList();
            var acutalMoves = actual as IList<ChessMove> ?? actual.ToList();

            if (!expectedLocations.Any() && !acutalMoves.Any()) return;

            var actualMoves = actual as IList<ChessMove> ?? acutalMoves.ToList();
            var startLoc = actualMoves.First().From;
            var expectedMoves = expectedLocations.Select(e => new ChessMove(startLoc, e, MoveType.Unknown));

            CollectionAssert.AreEquivalent(expectedMoves, actualMoves);

            Assert.That(actualMoves.Count(), Is.EqualTo(expectedLocations.Count()));
        }

        protected static void AssertMovesContains(IEnumerable<ChessMove> moves, string location, MoveType moveType)
        {
            var found = moves.FirstOrDefault( m => 
                m.To.Equals(BoardLocation.At("A3"))
                && m.MoveType == MoveType.Move);

            Assert.IsNotNull(found, $"MoveType of '{moveType}' to ${location} not found!.");
        }

        protected static void AssertAllMovesAreOfType(IEnumerable<ChessMove> moves, MoveType moveType)
        {
            Assert.That(moves.All(m => m.MoveType == moveType), "Unexpected MoveType found");
        }


        protected static List<string> DumpBoardToConsole(ChessBoard board)
        {
            var view = new OneCharBoard(board);
            var ranks = view.Ranks.ToList();
            ranks.ForEach(Console.WriteLine);
            return ranks;
        }
    }
}