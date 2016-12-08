using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CSharpChess.TheBoard;
using CSharpChess.Threat;
using NUnit.Framework;

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
            Assert.True(board[m.To].Piece.Is(chessPiece.Colour, chessPiece.Name), $"'{board[m.To].Piece}' found at destination, expected' {chessPiece}'");
        }

        protected static void AssertMovesContainsExpectedWithType(IEnumerable<ChessMove> actual,
            IEnumerable<BoardLocation> expected, MoveType moveType)
        {
            var expectedLocations = expected as IList<BoardLocation> ?? expected.ToList();
            var actualMoves = actual as IList<ChessMove> ?? actual.ToList();

            if (!expectedLocations.Any() || !actualMoves.Any()) Assert.Fail("No moves found!");

            var startLoc = actualMoves.First().From;
            var expectedMoves = expectedLocations.Select(e =>
                new ChessMove(startLoc, e, moveType)
                );

            var movesOfType = actualMoves.Where(m => m.MoveType == moveType).ToList();
            CollectionAssert.AreEquivalent(expectedMoves, movesOfType);

            Assert.That(movesOfType.Count(), Is.EqualTo(expectedLocations.Count()));
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


        protected static void DumpBoardToConsole(ChessBoard board)
        {
            BoardConsoleWriter.Write(board);
//            var view = new OneCharBoard(board);
//            var ranks = view.Ranks.ToList();
//            ranks.ForEach(Console.WriteLine);
//            return ranks;
        }

        protected static void DumpBoardLocations(IEnumerable<BoardLocation> attacking)
        {
            Console.Write($"{string.Join(",", attacking)}");
            Console.WriteLine($" - {attacking.Count()}");
        }

        protected static void AssertPiecesGeneratesVerticalThreat(ChessBoard customBoard, Chess.PieceNames pieceName, Func<BoardLocation, int, IEnumerable<BoardLocation>> expectedThreatsBuilder)
        {
            var analyser = new ThreatAnalyser(customBoard);
            analyser.BuildTable();

            foreach (var rook in customBoard.Pieces.Where(p => p.Piece.Is(pieceName)))
            {
                foreach (var vertDirection in new[] {1, -1})
                {
                    var expected = expectedThreatsBuilder(rook.Location, vertDirection).ToList();

                    CollectionAssert.IsSubsetOf(expected, analyser.AttacksFrom(rook.Location));

                    foreach (var boardLocation in expected)
                    {
                        var defending = analyser.DefendingAt(boardLocation, Chess.ColourOfEnemy(rook.Piece.Colour)).ToList();
                        Assert.That(defending.Any(d => d.Equals(rook.Location)));
                    }
                }
            }

        }

        protected IEnumerable<BoardLocation> BuildVerticalThreats(BoardLocation fromPieceAtLocation, int vertDirectionModifier)
        {
            var expected = new List<BoardLocation>();
            for (int i = 1; i <= 7; i++)
            {
                var rank = fromPieceAtLocation.Rank + (i * vertDirectionModifier);
                if (Chess.Board.IsValidLocation((int) fromPieceAtLocation.File, rank))
                {
                    expected.Add(BoardLocation.At(fromPieceAtLocation.File, rank));
                }
            }

            return expected;
        }
    }

    public class BoardConsoleWriter
    {
        private const bool UseColours = false;
        public static void Write(ChessBoard board, ThreatAnalyser threats = null)
        {
            var consoleBoard = CreateConsoleBoard(board, threats);

            foreach (var rank in Chess.Ranks.Reverse())
            {
                foreach (var file in Chess.Files)
                {
                    consoleBoard[BoardLocation.At((int) file, rank)]();
                }
                Console.Write("\n");
            }

        }

        private static Dictionary<BoardLocation, Action> CreateConsoleBoard(ChessBoard board, ThreatAnalyser threats)
        {
            var t = threats ?? new ThreatAnalyser(board).BuildTable();

            var consoleBoard = new Dictionary<BoardLocation, Action>();


            foreach (var boardPiece in board.Pieces)
            {
                var underThreat = t.DefendingAt(boardPiece.Location, boardPiece.Piece.Colour).Any();

                Action write = () =>
                {
                    if (underThreat && UseColours)
                    {
                        Console.BackgroundColor = ConsoleColor.Red;
                    }
                    if (underThreat && !UseColours)
                    {
                        Console.Write("X");
                    }
                    else
                        Console.Write(OneCharBoard.ToChar(boardPiece));
                    if (underThreat && UseColours)
                    {
                        Console.ResetColor();
                    }
                };

                consoleBoard.Add(boardPiece.Location, write);
            }
            return consoleBoard;
        }
    }
}