using System.Linq;
using NUnit.Framework;

namespace chess.pgn.tests
{
    public class PgnTagPairTests
    {
        [TestCase("[Event \"test event\"]")]
        [TestCase("[ Event \"test event\"]")]
        [TestCase(" [Event \"test event\"]")]
        [TestCase(" [ Event \"test event\"]")]
        public void Parse_should_find_tag_name(string pair)
        {
            var result = PgnTagPair.Parse(pair);

            Assert.That(result.Name, Is.EqualTo("Event"));
        }

        [TestCase("[Event \"test event\"]")]
        [TestCase("[Event \t \"test event\"]")]
        [TestCase("[Event \"test event\" ]")]
        [TestCase("[Event \"test event\" \t]")]
        [TestCase("[Event \"test event\"] ")]
        [TestCase("[Event \"test event\"] \t")]
        public void Parse_should_find_tag_value(string pair)
        {
            var result = PgnTagPair.Parse(pair);

            Assert.That(result.Value, Is.EqualTo("test event"));
        }


        [TestCase("[Event \"test event\"][Site \"test site\"]")]
        [TestCase("[Event \"test event\"] [Site \"test site\"]")]
        [TestCase("[Event \"test event\"]\t[Site \"test site\"]")]
        [TestCase("[Event \"test event\"]\n[Site \"test site\"]")]
        public void ParseMultiple_should_find_all(string pairs)
        {
            var result = PgnTagPair.ParseMultiple(pairs).ToList();

            Assert.That(result.Count(), Is.EqualTo(2));

            var second = result[1];

            Assert.That(second.Name, Is.EqualTo("Site"));
            Assert.That(second.Value, Is.EqualTo("test site"));
        }
    }
}