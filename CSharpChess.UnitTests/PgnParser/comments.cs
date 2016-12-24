using System.Linq;
using CSharpChess.TheBoard;
using NUnit.Framework;

namespace CSharpChess.UnitTests.PgnParser
{
    [TestFixture]
    public class Comments : PgnParserTestsBase
    {
        [Test]
        public void end_of_line_comments_in_pairs_pgn_moves_are_ignored()
        {
            var text = "3. e4 e5 ; An end of line comment";

            var pgnTurns = AssertPgnTurnQueryParsed(text);

            Assert.That(pgnTurns.Count(), Is.EqualTo(1));
            var turn = pgnTurns.First();

            AssertPgnMoveQueryIs(turn.White, Chess.Board.ChessFile.None, Chess.Colours.White, Chess.PieceNames.Pawn, MoveType.Move, "E4");
            AssertPgnMoveQueryIs(turn.Black, Chess.Board.ChessFile.None, Chess.Colours.Black, Chess.PieceNames.Pawn, MoveType.Move, "E5");
        }

        [Test]
        public void end_of_line_comments_in_single_pgn_moves_for_white_are_ignored()
        {
            var text = "3. e4 ; An end of line comment";

            var pgnTurns = AssertPgnTurnQueryParsed(text);

            Assert.That(pgnTurns.Count(), Is.EqualTo(1));
            var turn = pgnTurns.First();

            AssertPgnMoveQueryIs(turn.White, Chess.Board.ChessFile.None, Chess.Colours.White, Chess.PieceNames.Pawn, MoveType.Move, "E4");
        }

        [Test]
        public void end_of_line_comments_in_single_pgn_moves_for_black_are_ignored()
        {
            var text = "3... e5 ; An end of line comment";

            var pgnTurns = AssertPgnTurnQueryParsed(text);

            Assert.That(pgnTurns.Count(), Is.EqualTo(1));
            var turn = pgnTurns.First();

            AssertPgnMoveQueryIs(turn.Black, Chess.Board.ChessFile.None, Chess.Colours.Black, Chess.PieceNames.Pawn, MoveType.Move, "E5");
        }

        [Test]
        public void inline_comments_are_ignored()
        {
            var text = "3. e4 e5 { An in-line comment} 4. d4 d5";

            var pgnTurns = AssertPgnTurnQueryParsed(text);

            Assert.That(pgnTurns.Count(), Is.EqualTo(1));
            var turn = pgnTurns.First();

            AssertPgnMoveQueryIs(turn.White, Chess.Board.ChessFile.None, Chess.Colours.White, Chess.PieceNames.Pawn, MoveType.Move, "E4");
        }


    }
}