using NUnit.Framework;

namespace chess.pgn.tests
{
    public class PgnTagPairTests
    {
        [TestCase("[Event \"event value\"]", "Event", "event value")]
        [TestCase(" [ Event \"event value\" ]", "Event", "event value")]
        public void Parse_works(string tag, string key, string value)
        {
            var result = PgnTagPair.Parse(tag);

            Assert.That(result.Name, Is.EqualTo(key));
            Assert.That(result.Value, Is.EqualTo(value));
        }
    }
}