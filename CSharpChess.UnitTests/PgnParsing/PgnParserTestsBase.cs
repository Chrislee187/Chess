using System.Collections.Generic;
using CsChess.Pgn;
using CSharpChess.Extensions;
using NUnit.Framework;

namespace CSharpChess.UnitTests.PgnParsing
{
    public class PgnParserTestsBase
    {
        protected static void AssertPgnMoveQueryIs(PgnQuery query, Colours colour, PieceNames pieceName,
            string destination, ChessFile chessFile = ChessFile.None)
        {
            Assert.That(query.Piece, Is.EqualTo(new ChessPiece(colour, pieceName)));

            if (chessFile != ChessFile.None)
            {
                Assert.That(query.FromFile, Is.EqualTo(chessFile));
            }
            var dest = BoardLocation.At(destination);
            Assert.That(query.ToFile, Is.EqualTo(dest.File));
            Assert.That(query.ToRank, Is.EqualTo(dest.Rank));
        }

        protected static IEnumerable<PgnTurnQuery> AssertPgnTurnQueryParsed(string text)
        {
            IEnumerable<PgnTurnQuery> pgnTurns;
            var parsed = PgnTurnParser.TryParse(text, out pgnTurns);
            Assert.True(parsed, $"'{text}' did not parse as a Pgn turn");
            return pgnTurns;
        }

        internal static void AssertMoveQueryLocations(PgnQuery pgnQuery, ChessFile fromFile, int fromRank, ChessFile toFile, int toRank)
        {
            AssertMoveFromLocation(pgnQuery, fromFile, fromRank);
            AssertMoveToLocation(pgnQuery, toFile, toRank);
        }

        internal static void AssertMoveToLocation(PgnQuery pgnQuery, ChessFile toFile, int toRank)
        {
            Assert.That(pgnQuery.ToFile, Is.EqualTo(toFile));
            Assert.That(pgnQuery.ToRank, Is.EqualTo(toRank));
        }

        internal static void AssertMoveFromLocation(PgnQuery pgnQuery, ChessFile fromFile, int fromRank)
        {
            Assert.That(pgnQuery.FromFile, Is.EqualTo(fromFile));
            Assert.That(pgnQuery.FromRank, Is.EqualTo(fromRank));
        }


    }
}