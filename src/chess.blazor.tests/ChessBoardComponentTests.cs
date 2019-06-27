using System;
using System.Collections.Generic;
using chess.blazor.Shared.Chess;
using chess.webapi.client.csharp;
using NUnit.Framework;

namespace chess.blazor.tests
{
    [TestFixture]
    public class ChessBoardComponentTests
    {
        private ChessBoardComponent _component;

        [SetUp]
        public void SetUp()
        {
            _component = new ChessBoardComponent();
        }
//
//        [Test]
//        public void PerformMove_deselects_selection()
//        {
//            // TODO: Hook up CI to the components
//            throw new NotImplementedException();
//        }
//        [Test]
//        public void MoveSelectedAsync_invokes_OnMoveSelectedAsync_delegate()
//        {
//            // TODO: Hook up CI to the components
//            throw new NotImplementedException();
//        }
//        [Test]
//        public void Selection_is_deselected_when_move_is_invoked()
//        {
//            // TODO: Hook up CI to the components
//            throw new NotImplementedException();
//        }
//
//        [Test]
//        public void PieceSelectedAsync_raises_move_event_went_complete_move_captured()
//        {
//            // TODO: Hook up CI to the components
//            throw new NotImplementedException();
//        }

        [Test]
        public void Update_sets_Parameters()
        {
            Assert.That(_component.Board, Is.EqualTo(new string('.', 64)));
            Assert.That(_component.AvailableMoves, Is.Null);
            Assert.That(_component.WhiteToPlay, Is.False);

            var board = "PPPPPPP";
            var moves = new List<Move> { new Move() }.ToArray();
            _component.Update(board, moves, true);
            Assert.That(_component.Board, Is.EqualTo(board));
            Assert.That(_component.AvailableMoves, Is.EqualTo(moves));
            Assert.That(_component.WhiteToPlay, Is.True);
        }

        [Test]
        public void Enpassant_encoded_pawns_are_converted_to_plain_pawn()
        {
            var moves = new List<Move> { new Move() }.ToArray();
            _component.Update("eE", moves, true);

            Assert.That(_component.Board, Is.EqualTo("pP"));
        }

    }
}