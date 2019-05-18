using NUnit.Framework;

namespace chess.pgn.tests
{
    public class PgnDateTests
    {
        [TestCase("1992.11.04", 1992, 11, 04)]
        [TestCase("1992.11", 1992, 11, null)]
        public void Parse_works(string pgn, int? year, int? month, int? day)
        {
            var parsed = PgnDate.Parse(pgn);

            Assert.That(parsed.Day, Is.EqualTo(day), "Day parsing failed");
            Assert.That(parsed.Month, Is.EqualTo(month), "Month parsing failed");
            Assert.That(parsed.Year, Is.EqualTo(year), "Year parsing failed");
        }
    }
}