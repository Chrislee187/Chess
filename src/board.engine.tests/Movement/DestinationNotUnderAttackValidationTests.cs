using System.Collections.Generic;
using board.engine.Actions;
using board.engine.Board;
using board.engine.Movement;
using board.engine.Movement.Validators;
using board.engine.tests.Actions;
using Moq;
using NUnit.Framework;

namespace board.engine.tests.Movement
{
    [TestFixture]
    public class DestinationNotUnderAttackValidationTests
    {
        private DestinationNotUnderAttackValidator<TestBoardEntity> _validator;
        private Mock<DestinationNotUnderAttackValidator<TestBoardEntity>.IBoardStateWrapper> _wrapperMock;

        [SetUp]
        public void SetUp()
        {
            _wrapperMock = new Mock<DestinationNotUnderAttackValidator<TestBoardEntity>.IBoardStateWrapper>();
            _validator = new DestinationNotUnderAttackValidator<TestBoardEntity>();
        }

        [Test]
        public void Should_return_false_for_square_under_enemy_attack()
        {
            var move = BoardMove.Create(BoardLocation.At(5,1), BoardLocation.At(4, 1), (int) DefaultActions.MoveOrTake);
            SetupFromEntity(move, 1);
            SetupGetNonOwnerEntities(move, 2);

            Assert.False(_validator.ValidateMove(move, _wrapperMock.Object));
        }

        [Test]
        public void Should_return_true_for_square_under_friendly_attack()
        {
            var move = BoardMove.Create(BoardLocation.At(5, 1), BoardLocation.At(6, 1), (int) DefaultActions.MoveOrTake);

            SetupFromEntity(move, 1);
            SetupGetNonOwnerEntities(move, 1);

            Assert.True(_validator.ValidateMove(move, _wrapperMock.Object));

        }
        [Test]
        public void Should_return_true_for_square_under_no_attack()
        {
            var move = BoardMove.Create(BoardLocation.At(5, 1), BoardLocation.At(5, 2), (int) DefaultActions.MoveOrTake);

            SetupFromEntity(move, 1);
            SetupGetNonOwnerEntitiesReturnsNone();

            Assert.True(_validator.ValidateMove(move, _wrapperMock.Object));

        }

        protected void SetupFromEntity(BoardMove move, int owner)
        {
            var item = new LocatedItem<TestBoardEntity>(move.From, new TestBoardEntity(owner), new Paths());
            _wrapperMock.Setup(m => m.GetFromEntity(It.IsAny<BoardMove>()))
                .Returns(item);
        }

        private void SetupGetNonOwnerEntities(BoardMove move, int owner)
        {
            var itemAttackingMoveToLocation = new LocatedItem<TestBoardEntity>(
                null,
                new TestBoardEntity(owner),
                new Paths
                {
                    new Path
                    {
                        new BoardMove(null, move.To, (int) DefaultActions.MoveOrTake)
                    }
                });
            _wrapperMock.Setup(m => m.GetNonOwnerEntities(It.Is<int>(i => i != owner)))
                .Returns(new List<LocatedItem<TestBoardEntity>>
                {
                    itemAttackingMoveToLocation
                });
            _wrapperMock.Setup(m => m.GetNonOwnerEntities(It.Is<int>(i => i == owner)))
                .Returns(new List<LocatedItem<TestBoardEntity>>());
        }

        private void SetupGetNonOwnerEntitiesReturnsNone()
        {
            _wrapperMock.Setup(m => m.GetNonOwnerEntities(It.IsAny<int>()))
                .Returns(new List<LocatedItem<TestBoardEntity>>());
        }

    }
}