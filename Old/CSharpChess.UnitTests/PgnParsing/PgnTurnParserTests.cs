using System.Collections.Generic;
using System.Linq;
using CsChess.Pgn;
using CSharpChess.System;
using NUnit.Framework;

namespace CSharpChess.UnitTests.PgnParsing
{
    [TestFixture]
    public class PgnTurnParserTests : PgnParserTestsBase
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

            AssertPgnMoveQueryIs(pgnTurns[0].White, Colours.White, PieceNames.Pawn, "E4");
            AssertPgnMoveQueryIs(pgnTurns[0].Black, Colours.Black, PieceNames.Pawn, "E5");
        }

        [Test]
        public void end_of_line_comments_in_single_pgn_moves_for_white_are_ignored()
        {
            var text = "3. e4 ; An end of line comment";

            var pgnTurns = AssertPgnTurnQueryParsed(text).ToList();

            Assert.That(pgnTurns.Count(), Is.EqualTo(1));

            AssertPgnMoveQueryIs(pgnTurns[0].White, Colours.White, PieceNames.Pawn, "E4");
        }

        [Test]
        public void end_of_line_comments_in_single_pgn_moves_for_black_are_ignored()
        {
            var text = "3... e5 ; An end of line comment";

            var pgnTurns = AssertPgnTurnQueryParsed(text).ToList();

            Assert.That(pgnTurns.Count(), Is.EqualTo(1));

            AssertPgnMoveQueryIs(pgnTurns[0].Black, Colours.Black, PieceNames.Pawn, "E5");
        }

        [Test]
        public void inline_comments_are_ignored()
        {
            var text = "3. e4 e5 { An in-line comment } 4. d4 d5";

            var pgnTurns = AssertPgnTurnQueryParsed(text).ToList();

            Assert.That(pgnTurns.Count(), Is.EqualTo(2));

            AssertPgnMoveQueryIs(pgnTurns[0].White, Colours.White, PieceNames.Pawn, "E4");
            AssertPgnMoveQueryIs(pgnTurns[0].Black, Colours.Black, PieceNames.Pawn, "E5");

            AssertPgnMoveQueryIs(pgnTurns[1].White, Colours.White, PieceNames.Pawn, "D4");
            AssertPgnMoveQueryIs(pgnTurns[1].Black, Colours.Black, PieceNames.Pawn, "D5");
        }

        [Test]
        public void inline_comments_with_bad_spacing_are_ignored()
        {
            var text = "3. e4 e5{An in-line comment}4. d4 d5";

            var pgnTurns = AssertPgnTurnQueryParsed(text).ToList();

            Assert.That(pgnTurns.Count(), Is.EqualTo(2));

            AssertPgnMoveQueryIs(pgnTurns[0].White, Colours.White, PieceNames.Pawn, "E4");
            AssertPgnMoveQueryIs(pgnTurns[0].Black, Colours.Black, PieceNames.Pawn, "E5");

            AssertPgnMoveQueryIs(pgnTurns[1].White, Colours.White, PieceNames.Pawn, "D4");
            AssertPgnMoveQueryIs(pgnTurns[1].Black, Colours.Black, PieceNames.Pawn, "D5");
        }

        [Test]
        public void game_over_states_after_white_turns_are_detected()
        {
            var text = "72.Kb2  1-0";

            IEnumerable<PgnTurnQuery> pgnTurns1;
            var parsed = PgnTurnParser.TryParse(text, out pgnTurns1);
            Assert.True(parsed, $"'{text}' did not parse as a Pgn turn");
            var pgnTurns = pgnTurns1.ToList();

            Assert.That(pgnTurns.Count(), Is.EqualTo(1));

            AssertPgnMoveQueryIs(pgnTurns[0].White, Colours.White, PieceNames.King, "B2");
            Assert.That(pgnTurns[0].Black.GameOver, Is.True);
            Assert.That(pgnTurns[0].Black.GameResult, Is.EqualTo(ChessGameResult.WhiteWins));
        }
        [Test]
        public void game_over_states_after_black_turns_are_detected()
        {
            var text = "19.Nxf7 Ne4+ 0-1";

            IEnumerable<PgnTurnQuery> pgnTurns1;
            var parsed = PgnTurnParser.TryParse(text, out pgnTurns1);
            Assert.True(parsed, $"'{text}' did not parse as a Pgn turn");
            var pgnTurns = pgnTurns1.ToList();

            Assert.That(pgnTurns.Count(), Is.EqualTo(2));

            AssertPgnMoveQueryIs(pgnTurns[0].White, Colours.White, PieceNames.Knight, "F7");
            AssertPgnMoveQueryIs(pgnTurns[0].Black, Colours.Black, PieceNames.Knight, "E4");
            Assert.That(pgnTurns[1].White.GameOver, Is.True);
            Assert.That(pgnTurns[1].White.GameResult, Is.EqualTo(ChessGameResult.BlackWins));
        }

    }
}