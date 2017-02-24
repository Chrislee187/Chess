using System.Collections.Generic;
using CSharpChess.Pgn;
using CSharpChess.TheBoard;
using NUnit.Framework;

namespace CSharpChess.UnitTests.PgnParsing
{
    public class PgnParserTestsBase
    {
        protected static void AssertPgnMoveQueryIs(MoveQuery query, Chess.Colours colour, Chess.PieceNames pieceName,
            MoveType moveType, string destination, Chess.Board.ChessFile chessFile = Chess.Board.ChessFile.None)
        {
            Assert.Fail();
//            Assert.That(query.Piece, Is.EqualTo(new ChessPiece(colour, pieceName)));
//            Assert.That(query.Destination.ToString(), Is.EqualTo(destination));
//            Assert.That(query.MoveType, Is.EqualTo(moveType));
//            Assert.That(query.FromFile, Is.EqualTo(chessFile));
        }

        protected static IEnumerable<PgnTurnQuery> AssertPgnTurnQueryParsed(string text)
        {
            IEnumerable<PgnTurnQuery> pgnTurns;
            var parsed = PgnTurnParser.TryParse(text, out pgnTurns);
            Assert.True(parsed, $"'{text}' did not parse as a Pgn turn");
            return pgnTurns;
        }

        internal static void AssertMoveQueryLocations(MoveQuery moveQuery, Chess.Board.ChessFile fromFile, int fromRank, Chess.Board.ChessFile toFile, int toRank)
        {
            AssertMoveFromLocation(moveQuery, fromFile, fromRank);
            AssertMoveToLocation(moveQuery, toFile, toRank);
        }

        internal static void AssertMoveToLocation(MoveQuery moveQuery, Chess.Board.ChessFile toFile, int toRank)
        {
            Assert.That(moveQuery.ToFile, Is.EqualTo(toFile));
            Assert.That(moveQuery.ToRank, Is.EqualTo(toRank));
        }

        internal static void AssertMoveFromLocation(MoveQuery moveQuery, Chess.Board.ChessFile fromFile, int fromRank)
        {
            Assert.That(moveQuery.FromFile, Is.EqualTo(fromFile));
            Assert.That(moveQuery.FromRank, Is.EqualTo(fromRank));
        }


    }
}