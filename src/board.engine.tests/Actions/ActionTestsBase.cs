using board.engine.Actions;
using board.engine.Board;
using board.engine.Movement;
using Moq;

namespace board.engine.tests.Actions
{
    public abstract class ActionTestsBase<TAction, TEntity> 
        where TAction : IBoardAction 
        where TEntity : class, IBoardEntity
    {
        protected readonly BoardMove AnyMove = BoardMove.Create(BoardLocation.At(4,2), BoardLocation.At(4,4), (int) DefaultActions.MoveOnly);
        protected readonly BoardMove AnyTake = BoardMove.Create(BoardLocation.At(4, 2), BoardLocation.At(4, 5), (int)DefaultActions.MoveOrTake);

        protected Mock<IBoardState<TEntity>> StateMock;
        protected Mock<IBoardActionProvider<TEntity>> ActionFactoryMock;
        protected Mock<IBoardAction> BoardActionMock = new Mock<IBoardAction>();
        protected Mock<IBoardEntityFactory<TEntity>> EntityFactoryMock = new Mock<IBoardEntityFactory<TEntity>>();

        protected TAction Action;

        protected void SetUp()
        {
            StateMock = new Mock<IBoardState<TEntity>>();
            ActionFactoryMock = new Mock<IBoardActionProvider<TEntity>>();
            BoardActionMock = new Mock<IBoardAction>();
        }

        protected void SetupPromotionPiece(TEntity piece)
            => EntityFactoryMock.Setup(f => f.Create(It.IsAny<object>()))
                .Returns(piece);

        protected void SetupLocationReturn(BoardLocation at, TEntity piece) 
            => StateMock.Setup(m => m.GetItem(at)).Returns(new LocatedItem<TEntity>(at, piece, null));


        protected void SetupMockActionForMoveType(int action)
            => ActionFactoryMock.Setup(m => m.Create(action, It.IsAny<IBoardState<TEntity>>()))
                .Returns(BoardActionMock.Object);

        protected void VerifyLocationWasCleared(BoardLocation loc)
            => StateMock.Verify(s => s.Remove(loc), Times.Once);

        protected void VerifyLocationWasNOTCleared(BoardLocation loc)
            => StateMock.Verify(s => s.Remove(loc), Times.Never);
        protected void VerifyActionWasCreated(int action)
            => ActionFactoryMock.Verify(m => m.Create(action, It.IsAny<IBoardState<TEntity>>()), Times.Once);

        protected void VerifyActionWasExecuted(BoardMove move)
        => BoardActionMock.Verify(m => m.Execute(move), Times.Once);

        protected void VerifyEntityWasRetrieved(BoardLocation loc)
            => StateMock.Verify(m => m.GetItem(loc), Times.Once);

        protected void VerifyEntityWasNOTRetrieved(BoardLocation loc)
            => StateMock.Verify(m => m.GetItem(loc), Times.Never);

        protected void VerifyEntityWasPlaced(BoardLocation loc, TEntity piece)
            => StateMock.Verify(m => m.PlaceEntity(loc, piece), Times.Once);
        protected void VerifyNewEntityWasPlaced(BoardLocation loc, TEntity piece)
            => StateMock.Verify(m => m.PlaceEntity(loc,
                It.Is<TEntity>(cpe => cpe.Equals(piece))), Times.Once);
        protected void VerifyNewEntityWasNOTPlaced(BoardLocation loc, TEntity piece)
            => StateMock.Verify(m => m.PlaceEntity(loc,
                It.Is<TEntity>(cpe => cpe.Equals(piece))), Times.Never);

        protected void SetupStateIsEmpty(BoardLocation at, bool isEmpty) 
            => StateMock.Setup(s => s.IsEmpty(It.Is<BoardLocation>(bl => bl.Equals(at)))).Returns(isEmpty);
    }
}