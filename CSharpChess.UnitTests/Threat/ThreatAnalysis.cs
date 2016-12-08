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
        public void knights_generate_threat()
        {
            Assert.That(_newBoard.Pieces
                    .Where(p => p.Piece.Is(Chess.PieceNames.Knight))
                    .Where(p => !_newBoardThreats.AttacksFrom(p.Location).Any())
                , Is.Empty);
        }

        [Test]
        public void other_pieces_generate_threat()
        {
            Assert.Fail();
        }
    }
}