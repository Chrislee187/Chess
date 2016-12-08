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
    public class pawns : BoardAssertions
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
            DumpBoardToConsole(_newBoard);
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

        [Test]
        public void pawns_generate_threat()
        {
            Assert.That(_newBoard.Pieces
                    .Where(p => p.Piece.Is(Chess.PieceNames.Pawn))
                    .Where(p => !_newBoardThreats.AttacksFrom(p.Location).Any())
                , Is.Empty);
        }
    }
}