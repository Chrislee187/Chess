using chess.engine.Actions;
using chess.engine.Board;
using chess.engine.Chess.Entities;
using chess.engine.Game;
using chess.engine.Movement;
using Moq;

namespace chess.engine.tests.Actions
{
    public abstract class ActionTestsBase<T> where T : IBoardAction
    {
        protected readonly BoardMove AnyMove = BoardMove.Create("D2", "D4", MoveType.MoveOnly);
        protected readonly BoardMove AnyTake = BoardMove.Create("D2", "D5", MoveType.MoveOrTake);

        protected Mock<IBoardState> StateMock = new Mock<IBoardState>();
        protected Mock<IBoardActionFactory> FactoryMock = new Mock<IBoardActionFactory>();
        protected Mock<IBoardAction> BoardActionMock = new Mock<IBoardAction>();

        protected T Action;

        protected void SetUp()
        {
            StateMock = new Mock<IBoardState>();
            FactoryMock = new Mock<IBoardActionFactory>();
            BoardActionMock = new Mock<IBoardAction>();
        }

        protected void SetupPieceReturn(BoardLocation at, ChessPieceEntity piece) 
            => StateMock.Setup(m => m.GetItem(at)).Returns(new LocatedItem<ChessPieceEntity>(at, piece, null));

        protected void SetupCreateMockActionForMoveType(MoveType moveType)
            => FactoryMock.Setup(m => m.Create(moveType, It.IsAny<IBoardState>()))
                .Returns(BoardActionMock.Object);

        protected void SetupCreateMockActionForMoveType(DefaultActions action)
            => FactoryMock.Setup(m => m.Create(action, It.IsAny<IBoardState>()))
                .Returns(BoardActionMock.Object);

        protected void VerifyLocationWasCleared(BoardLocation loc)
            => StateMock.Verify(s => s.Remove(loc), Times.Once);

        protected void VerifyLocationWasNOTCleared(BoardLocation loc)
            => StateMock.Verify(s => s.Remove(loc), Times.Never);

        protected void VerifyActionWasCreated(MoveType moveType)
            => FactoryMock.Verify(m => m.Create(moveType, It.IsAny<IBoardState>()), Times.Once);

        protected void VerifyActionWasCreated(DefaultActions action)
            => FactoryMock.Verify(m => m.Create(action, It.IsAny<IBoardState>()), Times.Once);

        protected void VerifyActionWasExecuted(BoardMove move)
        => BoardActionMock.Verify(m => m.Execute(move), Times.Once);

        protected void VerifyEntityWasRetrieved(BoardLocation loc)
            => StateMock.Verify(m => m.GetItem(loc), Times.Once);

        protected void VerifyEntityWasNOTRetrieved(BoardLocation loc)
            => StateMock.Verify(m => m.GetItem(loc), Times.Never);

        protected void VerifyEntityWasPlaced(BoardLocation loc, ChessPieceEntity piece)
            => StateMock.Verify(m => m.PlaceEntity(loc, piece, It.IsAny<bool>()), Times.Once);
        protected void VerifyNewEntityWasPlaced(BoardLocation loc, ChessPieceEntity piece)
            => StateMock.Verify(m => m.PlaceEntity(loc,
                It.Is<ChessPieceEntity>(cpe => cpe.EntityType == piece.EntityType && cpe.Player == piece.Player)
                , It.IsAny<bool>()
            ), Times.Once);
        protected void VerifyNewEntityWasNOTPlaced(BoardLocation loc, ChessPieceEntity piece)
            => StateMock.Verify(m => m.PlaceEntity(loc,
                It.Is<ChessPieceEntity>(cpe => cpe.EntityType == piece.EntityType && cpe.Player == piece.Player)
                , It.IsAny<bool>()
            ), Times.Never);
    }
}