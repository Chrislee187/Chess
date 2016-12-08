using System;
using System.Collections.Generic;
using System.Linq;
using CSharpChess.TheBoard;
using CSharpChess.Threat;
using CSharpChess.UnitTests.Helpers;
using NUnit.Framework;

namespace CSharpChess.UnitTests.Threat
{
    [TestFixture]
    public class rooks : BoardAssertions
    {
        [Test]
        public void rooks_generate_threat()
        {
            var customBoard = BoardBuilder.CustomBoard(NoPawnBoard, Chess.Colours.White);

            AssertPiecesGeneratesVerticalThreat(customBoard, Chess.PieceNames.Rook, BuildVerticalThreats);
        }
    }
}