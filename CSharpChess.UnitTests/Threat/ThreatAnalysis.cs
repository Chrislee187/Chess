using System;
using System.Linq;
using CSharpChess.TheBoard;
using CSharpChess.Threat;
using CSharpChess.UnitTests.Helpers;
using NUnit.Framework;

// ReSharper disable InconsistentNaming

namespace CSharpChess.UnitTests.Threat
{
    [TestFixture]
    public class ThreatAnalysis : BoardAssertions
    {
        private ChessBoard _newBoard;
        private ThreatAnalyser _newBoardThreats;

        [SetUp]
        public void SetUp()
        {
            _newBoard = BoardBuilder.NewGame;
            _newBoardThreats = new ThreatAnalyser(_newBoard);
            _newBoardThreats.BuildTable();
        }

        [Test]
        public void pawn_at_B2_generates_threat_against_A3_and_C3()
        {
            Assert.That(_newBoardThreats.AttacksFrom(BoardLocation.At("B2")), Contains.Item((BoardLocation) "A3"));
            Assert.That(_newBoardThreats.AttacksFrom(BoardLocation.At("B2")), Contains.Item((BoardLocation) "C3"));
        }

        [Test]
        public void A3_and_C3_have_threat_from_B2()
        {
            Assert.That(_newBoardThreats.DefendingAt(BoardLocation.At("A3"), Chess.Colours.Black),
                Contains.Item(BoardLocation.At("B2")));
            Assert.That(_newBoardThreats.DefendingAt(BoardLocation.At("C3"), Chess.Colours.Black),
                Contains.Item(BoardLocation.At("B2")));
        }

        [Test]
        public void normal_pawn_movement_does_not_generate_threat()
        {
            foreach (Chess.ChessFile file in Enum.GetValues(typeof(Chess.ChessFile)))
            {
                foreach (var threatRank in new[] {3, 4, 5, 6})
                {
                    var loc = BoardLocation.At(file, threatRank);

                    var defender = threatRank < 5 ? Chess.Colours.Black : Chess.Colours.White;
                    var pawnFile = defender == Chess.Colours.White ? 7 : 2;
                    var pawnLocation = BoardLocation.At(file, pawnFile);

                    CollectionAssert.DoesNotContain(_newBoardThreats.DefendingAt(loc, defender), pawnLocation);
                }
            }
        }

        [TestCase(Chess.PieceNames.Pawn)]
        [TestCase(Chess.PieceNames.Knight)]
        public void pawns_and_knights_generate_threat(Chess.PieceNames piece)
        {
            Assert.That(_newBoard.Pieces
                    .Where(p => p.Piece.Is(piece))
                    .Where(p => !_newBoardThreats.AttacksFrom(p.Location).Any())
                , Is.Empty);
        }

        [Test]
        public void rooks_generate_threat()
        {
            // TODO: Refactor this in to a seperate class with checks against on all the rooks on the board below
            // also gives us a place for custom tests as and when required.
            const string NoPawnBoard =
                "rnbqkbnr" +
                "........" +
                "........" +
                "........" +
                "........" +
                "........" +
                "........" +
                "RNBQKBNR";

            var customBoard = BoardBuilder.CustomBoard(NoPawnBoard, Chess.Colours.White);

            var analyser = new ThreatAnalyser(customBoard);
            analyser.BuildTable();

            var attacking = analyser.AttacksFrom(BoardLocation.At("A1")).ToList();
            var expected = BoardLocation.List("A2", "A3", "A4", "A5", "A6", "A7", "A8");
            CollectionAssert.AreEquivalent(expected, attacking);

            var defending = analyser.DefendingAt(BoardLocation.At("A2"), Chess.Colours.Black).ToList();
            Assert.That(defending.Any(d => d.Equals(BoardLocation.At("A1"))));
        }

        [Test]
        public void other_pieces_generate_threat()
        {
            Assert.Fail();
        }
    }
}