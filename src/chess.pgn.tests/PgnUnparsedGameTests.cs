using System.Linq;
using chess.pgn.Parsing;
using NUnit.Framework;

namespace chess.pgn.tests
{
    public class PgnUnparsedGameTests
    {

        [TestCase("", "", "")]
        [TestCase(" ", " ", " ")]
        [TestCase("\t", " ", " ")]
        [TestCase(" ", "\n", " ")]
        [TestCase("\n", "\n", "\n")]
        public void Parse_should_find_sections_regardless_of_whitespace(string preWhiteSpace, string midWhiteSpace,
            string postWhiteSpace)
        {
            var tagPairText = "[Event \"test event\"]";
            var moveListText = "1. d4 d5";

            var pgnFileText = $"{preWhiteSpace}{tagPairText}{midWhiteSpace}{moveListText}{postWhiteSpace}";
            var unparsedGame = PgnUnparsedGame.Parse(pgnFileText).Single();

            Assert.That(unparsedGame.TagPairSection, Is.EqualTo("[Event \"test event\"]"));
            Assert.That(unparsedGame.MoveListSection, Is.EqualTo(moveListText));
        }
    }
}