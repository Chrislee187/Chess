using chess.engine.Actions;
using chess.engine.Board;
using chess.engine.Game;
using chess.engine.Movement;
using Moq;

namespace chess.engine.tests.Actions
{
    public abstract class ActionTestsBase<TAction, TEntity> 
        where TAction : IBoardAction 
        where TEntity : class, IBoardEntity
    {
        protected readonly BoardMove AnyMove = BoardMove.Create("D2", "D4", MoveType.MoveOnly);
        protected readonly BoardMove AnyTake = BoardMove.Create("D2", "D5", MoveType.MoveOrTake);

        protected Mock<IBoardState<TEntity>> StateMock;
        protected Mock<IBoardActionFactory<TEntity>> FactoryMock;
        protected Mock<IBoardAction> BoardActionMock = new Mock<IBoardAction>();

        protected TAction Action;

        protected void SetUp()
        {
            StateMock = new Mock<IBoardState<TEntity>>();
            FactoryMock = new Mock<IBoardActionFactory<TEntity>>();
            BoardActionMock = new Mock<IBoardAction>();
        }

        protected void SetupPieceReturn(BoardLocation at, TEntity piece) 
            => StateMock.Setup(m => m.GetItem(at)).Returns(new LocatedItem<TEntity>(at, piece, null));

        protected void SetupCreateMockActionForMoveType(MoveType moveType)
            => FactoryMock.Setup(m => m.Create(moveType, It.IsAny<IBoardState<TEntity>>()))
                .Returns(BoardActionMock.Object);

        protected void SetupCreateMockActionForMoveType(DefaultActions action)
            => FactoryMock.Setup(m => m.Create(action, It.IsAny<IBoardState<TEntity>>()))
                .Returns(BoardActionMock.Object);

        protected void VerifyLocationWasCleared(BoardLocation loc)
            => StateMock.Verify(s => s.Remove(loc), Times.Once);

        protected void VerifyLocationWasNOTCleared(BoardLocation loc)
            => StateMock.Verify(s => s.Remove(loc), Times.Never);

        protected void VerifyActionWasCreated(MoveType moveType)
            => FactoryMock.Verify(m => m.Create(moveType, It.IsAny<IBoardState<TEntity>>()), Times.Once);

        protected void VerifyActionWasCreated(DefaultActions action)
            => FactoryMock.Verify(m => m.Create(action, It.IsAny<IBoardState<TEntity>>()), Times.Once);

        protected void VerifyActionWasExecuted(BoardMove move)
        => BoardActionMock.Verify(m => m.Execute(move), Times.Once);

        protected void VerifyEntityWasRetrieved(BoardLocation loc)
            => StateMock.Verify(m => m.GetItem(loc), Times.Once);

        protected void VerifyEntityWasNOTRetrieved(BoardLocation loc)
            => StateMock.Verify(m => m.GetItem(loc), Times.Never);

        protected void VerifyEntityWasPlaced(BoardLocation loc, TEntity piece)
            => StateMock.Verify(m => m.PlaceEntity(loc, piece), Times.Once);
        protected void VerifyNewEntityWasPlaced(BoardLocation loc, IBoardEntity piece)
            => StateMock.Verify(m => m.PlaceEntity(loc,
                It.Is<TEntity>(cpe => cpe.EntityName.Equals(piece.EntityName) && cpe.Owner.Equals(piece.Owner))), Times.Once);
        protected void VerifyNewEntityWasNOTPlaced(BoardLocation loc, IBoardEntity piece)
            => StateMock.Verify(m => m.PlaceEntity(loc,
                It.Is<TEntity>(cpe => cpe.EntityName.Equals(piece.EntityName) && cpe.Owner.Equals(piece.Owner))), Times.Never);

        protected void SetupStateIsEmpty(BoardLocation at, bool isEmpty) 
            => StateMock.Setup(s => s.IsEmpty(It.Is<BoardLocation>(bl => bl.Equals(at)))).Returns(isEmpty);
    }
}