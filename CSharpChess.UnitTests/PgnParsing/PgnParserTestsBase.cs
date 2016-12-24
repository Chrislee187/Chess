using System.Collections.Generic;
using CSharpChess.Pgn;
using CSharpChess.TheBoard;
using NUnit.Framework;

namespace CSharpChess.UnitTests.PgnParsing
{
    public class PgnParserTestsBase
    {
        protected static void AssertPgnMoveQueryIs(PgnMoveQuery query, Chess.Colours colour, Chess.PieceNames pieceName,
            MoveType moveType, string destination, Chess.Board.ChessFile chessFile = Chess.Board.ChessFile.None)
        {
            Assert.That(query.Piece, Is.EqualTo(new ChessPiece(colour, pieceName)));
            Assert.That(query.Destination.ToString(), Is.EqualTo(destination));
            Assert.That(query.MoveType, Is.EqualTo(moveType));
            Assert.That(query.FromFile, Is.EqualTo(chessFile));
        }

        protected static IEnumerable<PgnTurnQuery> AssertPgnTurnQueryParsed(string text)
        {
            IEnumerable<PgnTurnQuery> pgnTurns;
            var parsed = PgnTurnsParser.TryParse(text, out pgnTurns);
            Assert.True(parsed, $"'{text}' did not parse as a Pgn turn");
            return pgnTurns;
        }

    }
}