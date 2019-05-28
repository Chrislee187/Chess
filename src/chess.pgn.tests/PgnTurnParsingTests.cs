using System.Linq;
using NUnit.Framework;

namespace chess.pgn.tests
{
    public class PgnTurnParsingTests
    {
        [TestCase("1. d4 d5", 1, "d4", "d5")]
        [TestCase("3. d4 d5", 3, "d4", "d5")]
        [TestCase("3. d4 d5 " , 3, "d4", "d5")]
        [TestCase("3.\td4\td5\t" , 3, "d4", "d5")]
        public void Parse_should_handle_single_turn(string turnText, int turn, string white, string black)
        {
            var parsed = PgnTurn.Parse(turnText, out var result).First();

            Assert.That(parsed.Turn, Is.EqualTo(turn));
            Assert.That(parsed.White.San, Is.EqualTo(white));
            Assert.Null(parsed.White.Annotation);
            Assert.That(parsed.Black.San, Is.EqualTo(black));
            Assert.Null(parsed.Black.Annotation);
        }

        [TestCase("1. d4 {white annotation block} d5 {black annotation block}")]
        [TestCase("1. d4 { white annotation block } d5 { black annotation block }")]
        public void Parse_should_handle_annotations(string turnText)
        {
            var parsed = PgnTurn.Parse(turnText, out var result).First();

            Assert.That(parsed.Turn, Is.EqualTo(1));
            Assert.That(parsed.White.San, Is.EqualTo("d4"));
            Assert.That(parsed.White.Annotation, Is.EqualTo("white annotation block"));
            Assert.That(parsed.Black.San, Is.EqualTo("d5"));
            Assert.That(parsed.Black.Annotation, Is.EqualTo("black annotation block"));
        }
    }
}