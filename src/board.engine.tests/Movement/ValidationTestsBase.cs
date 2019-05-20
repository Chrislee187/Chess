using System.Collections.Generic;
using board.engine.Actions;
using board.engine.Board;
using board.engine.Movement;
using board.engine.Movement.Validators;
using board.engine.tests.Actions;
using Moq;

namespace board.engine.tests.Movement
{
    public class ValidationTestsBase
    {
        protected const int Friend = 0;
        protected const int Enemy = 1;
        protected Mock<IReadOnlyBoardState<TestBoardEntity>> RoBoardStateMock;

        protected void InitMocks()
        {
            RoBoardStateMock = new Mock<IReadOnlyBoardState<TestBoardEntity>>();
        }

        protected void SetupFromEntity(BoardMove move, TestBoardEntity entity = null)
        {
            LocatedItem<TestBoardEntity> locatedEntity = null;
            if (entity != null)
            {
                locatedEntity = new LocatedItem<TestBoardEntity>(move.From, entity, new Paths());
            }
            RoBoardStateMock.Setup(m => m.GetItem(It.Is<BoardLocation>(l => move.From.Equals(l))))
                .Returns(locatedEntity);
        }

        protected void SetupToEntity(BoardMove move, TestBoardEntity entity = null)
        {
            LocatedItem<TestBoardEntity> locatedEntity = null;
            if (entity != null)
            {
                locatedEntity = new LocatedItem<TestBoardEntity>(move.To, entity, new Paths());
            }
            RoBoardStateMock.Setup(m => m.GetItem(It.Is<BoardLocation>(l => move.To.Equals(l))))
                .Returns(locatedEntity);
        }

        protected void SetupGetNonOwnerEntities(BoardMove move, TestBoardEntity entity)
        {
            var from = BoardLocation.At(1,1);
            var itemAttackingMoveToLocation = new LocatedItem<TestBoardEntity>(
                from,
                entity,
                new Paths
                {
                    new Path
                    {
                        new BoardMove(from, move.To, (int) DefaultActions.MoveOrTake)
                    }
                });
            RoBoardStateMock.Setup(b => b.GetItems())
                .Returns(new List<LocatedItem<TestBoardEntity>>
                {
                    itemAttackingMoveToLocation
                });
        }

        protected void SetupGetNonOwnerEntitiesReturnsNone()
        {
            RoBoardStateMock.Setup(m => m.GetItems())
                .Returns(new List<LocatedItem<TestBoardEntity>>());
        }
    }
}