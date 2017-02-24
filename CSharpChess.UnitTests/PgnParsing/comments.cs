using System.Collections.Generic;
using System.Linq;
using CSharpChess.Pgn;
using NUnit.Framework;

namespace CSharpChess.UnitTests.PgnParsing
{
    [TestFixture]
    public class Comments : PgnParserTestsBase
    {
        [Test]
        public void end_of_line_comments_in_pairs_pgn_moves_are_ignored()
        {
            var text = "3. e4 e5 ; An end of line comment";

            IEnumerable<PgnTurnQuery> pgnTurns1;
            var parsed = PgnTurnParser.TryParse(text, out pgnTurns1);
            Assert.True(parsed, $"'{text}' did not parse as a Pgn turn");
            var pgnTurns = pgnTurns1.ToList();

            Assert.That(pgnTurns.Count(), Is.EqualTo(1));

            AssertPgnMoveQueryIs(pgnTurns[0].White, Chess.Colours.White, Chess.PieceNames.Pawn, "E4");
            AssertPgnMoveQueryIs(pgnTurns[0].Black, Chess.Colours.Black, Chess.PieceNames.Pawn, "E5");
        }

        [Test]
        public void end_of_line_comments_in_single_pgn_moves_for_white_are_ignored()
        {
            var text = "3. e4 ; An end of line comment";

            var pgnTurns = AssertPgnTurnQueryParsed(text).ToList();

            Assert.That(pgnTurns.Count(), Is.EqualTo(1));

            AssertPgnMoveQueryIs(pgnTurns[0].White, Chess.Colours.White, Chess.PieceNames.Pawn, "E4");
        }

        [Test]
        public void end_of_line_comments_in_single_pgn_moves_for_black_are_ignored()
        {
            var text = "3... e5 ; An end of line comment";

            var pgnTurns = AssertPgnTurnQueryParsed(text).ToList();

            Assert.That(pgnTurns.Count(), Is.EqualTo(1));

            AssertPgnMoveQueryIs(pgnTurns[0].Black, Chess.Colours.Black, Chess.PieceNames.Pawn, "E5");
        }

        [Test]
        public void inline_comments_are_ignored()
        {
            var text = "3. e4 e5 { An in-line comment } 4. d4 d5";

            var pgnTurns = AssertPgnTurnQueryParsed(text).ToList();

            Assert.That(pgnTurns.Count(), Is.EqualTo(2));

            AssertPgnMoveQueryIs(pgnTurns[0].White, Chess.Colours.White, Chess.PieceNames.Pawn, "E4");
            AssertPgnMoveQueryIs(pgnTurns[0].Black, Chess.Colours.Black, Chess.PieceNames.Pawn, "E5");

            AssertPgnMoveQueryIs(pgnTurns[1].White, Chess.Colours.White, Chess.PieceNames.Pawn, "D4");
            AssertPgnMoveQueryIs(pgnTurns[1].Black, Chess.Colours.Black, Chess.PieceNames.Pawn, "D5");
        }

        [Test]
        public void inline_comments_with_bad_spacing_are_ignored()
        {
            var text = "3. e4 e5{An in-line comment}4. d4 d5";

            var pgnTurns = AssertPgnTurnQueryParsed(text).ToList();

            Assert.That(pgnTurns.Count(), Is.EqualTo(2));

            AssertPgnMoveQueryIs(pgnTurns[0].White, Chess.Colours.White, Chess.PieceNames.Pawn, "E4");
            AssertPgnMoveQueryIs(pgnTurns[0].Black, Chess.Colours.Black, Chess.PieceNames.Pawn, "E5");

            AssertPgnMoveQueryIs(pgnTurns[1].White, Chess.Colours.White, Chess.PieceNames.Pawn, "D4");
            AssertPgnMoveQueryIs(pgnTurns[1].Black, Chess.Colours.Black, Chess.PieceNames.Pawn, "D5");
        }


    }
}