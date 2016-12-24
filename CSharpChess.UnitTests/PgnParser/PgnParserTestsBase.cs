using System.Collections.Generic;
using CSharpChess.Pgn;
using CSharpChess.TheBoard;
using NUnit.Framework;

namespace CSharpChess.UnitTests.PgnParser
{
    public class PgnParserTestsBase
    {
        protected static void AssertPgnMoveQueryIs(PgnMoveQuery query, Chess.Board.ChessFile chessFile, Chess.Colours colour, Chess.PieceNames pieceNames, MoveType moveType, string destination)
        {
            Assert.True(query.Piece.Is(colour, pieceNames));
            Assert.That(query.Destination.ToString(), Is.EqualTo(destination));
            Assert.That(query.MoveType, Is.EqualTo(moveType));
            Assert.That(query.FromFile, Is.EqualTo(chessFile));
        }

        protected static IEnumerable<PgnTurnQuery> AssertPgnTurnQueryParsed(string text)
        {
            IEnumerable<PgnTurnQuery> pgnTurns;
            var parsed = PgnTurnsParser.TryParse(text, out pgnTurns);
            Assert.True(parsed);
            return pgnTurns;
        }
    }
}