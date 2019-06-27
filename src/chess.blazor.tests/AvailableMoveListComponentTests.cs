using System;
using System.Collections.Generic;
using chess.blazor.Shared.Chess;
using chess.webapi.client.csharp;
using NUnit.Framework;

namespace chess.blazor.tests
{
    [TestFixture]
    public class AvailableMoveListComponentTests
    {
        private const string AnyMove = "a1a2";

        [Test]
        public void MoveSelected_throws_when_no_delegate_wired_up()
        {
            var component = new AvailableMoveListComponent();      
            Assert.That(() => component.MoveSelected(AnyMove), Throws.Exception);
        }
        [Test]
        public void Update_sets_parameters()
        {
            var component = new AvailableMoveListComponent();
            var moves = new List<Move> { new Move() }.ToArray();

            var aTitle = Guid.NewGuid().ToString();
            Assert.That(string.IsNullOrWhiteSpace(component.Title));
            Assert.That(component.Moves, Is.Null);
            Assert.That(component.ShowMoveList, Is.True);

            component.Update(aTitle, moves, false);

            Assert.That(component.Title, Is.EqualTo(aTitle));
            Assert.That(component.Moves, Is.EqualTo(moves));
            Assert.That(component.ShowMoveList, Is.False);
        }
    }
}