using System.Linq;
using CSharpChess.TheBoard;
using CSharpChess.Threat;
using CSharpChess.UnitTests.Helpers;
using NUnit.Framework;

// ReSharper disable InconsistentNaming

namespace CSharpChess.UnitTests.Threat
{
    [TestFixture]
    public class knights : BoardAssertions
    {
        private ChessBoard _newBoard;
        private ThreatAnalyser _newBoardThreats;

        [SetUp]
        public void SetUp()
        {
            _newBoard = BoardBuilder.NewGame;
            _newBoardThreats = new ThreatAnalyser(_newBoard);
        }

        [Test]
        public void knight_at_B1_can_move_to_A3_and_C3()
        {
            Assert.That(_newBoardThreats.For(BoardLocation.At("B1")).Moves.Select(t => t.To), Contains.Item((BoardLocation) "A3"));
            Assert.That(_newBoardThreats.For(BoardLocation.At("B1")).Moves.Select(t => t.To), Contains.Item((BoardLocation) "C3"));
        }

        [Test]
        public void knight_at_B1_has_no_takes()
        {
            Assert.That(_newBoardThreats.For(BoardLocation.At("B1")).Takes.Any(), Is.False);
        }

        [Test]
        public void knight_at_B1_covers_d2()
        {
            Assert.That(_newBoardThreats.For(BoardLocation.At("B1")).Covers.Select(t => t.To), Contains.Item((BoardLocation)"D2"));
        }
    }
}