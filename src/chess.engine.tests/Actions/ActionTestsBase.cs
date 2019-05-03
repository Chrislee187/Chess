using chess.engine.Actions;
using chess.engine.Entities;
using chess.engine.Game;
using chess.engine.Movement;
using Moq;

namespace chess.engine.tests.Actions
{
    public abstract class ActionTestsBase<T> where T : IBoardAction
    {
        protected readonly ChessMove AnyMove = ChessMove.Create("D2", "D4", ChessMoveType.MoveOnly);
        protected readonly ChessMove AnyTake = ChessMove.Create("D2", "D5", ChessMoveType.MoveOrTake);

        protected Mock<IBoardStateActions> StateMock = new Mock<IBoardStateActions>();
        protected Mock<IBoardActionFactory> FactoryMock = new Mock<IBoardActionFactory>();
        protected Mock<IBoardAction> BoardActionMock = new Mock<IBoardAction>();

        protected T Action;

        protected void SetUp()
        {
            StateMock = new Mock<IBoardStateActions>();
            FactoryMock = new Mock<IBoardActionFactory>();
            BoardActionMock = new Mock<IBoardAction>();
        }

        protected void SetupPieceReturn(BoardLocation at, ChessPieceEntity piece) 
            => StateMock.Setup(m => m.GetEntity(at)).Returns(piece);

        protected void SetupCreateMockActionForMoveType(ChessMoveType moveType)
            => FactoryMock.Setup(m => m.Create(moveType, It.IsAny<IBoardStateActions>()))
                .Returns(BoardActionMock.Object);

        protected void SetupCreateMockActionForMoveType(DefaultActions action)
            => FactoryMock.Setup(m => m.Create(action, It.IsAny<IBoardStateActions>()))
                .Returns(BoardActionMock.Object);

        protected void VerifyLocationWasCleared(BoardLocation loc)
            => StateMock.Verify(s => s.ClearLocation(loc), Times.Once);

        protected void VerifyActionWasCreated(ChessMoveType moveType)
            => FactoryMock.Verify(m => m.Create(moveType, It.IsAny<IBoardStateActions>()), Times.Once);

        protected void VerifyActionWasCreated(DefaultActions action)
            => FactoryMock.Verify(m => m.Create(action, It.IsAny<IBoardStateActions>()), Times.Once);

        protected void VerifyActionWasExecuted(ChessMove move)
        => BoardActionMock.Verify(m => m.Execute(move), Times.Once);

        protected void VerifyEntityWasRetrieved(BoardLocation loc)
            => StateMock.Verify(m => m.GetEntity(loc), Times.Once);

        protected void VerifyEntityWasPlaced(BoardLocation loc, ChessPieceEntity piece)
            => StateMock.Verify(m => m.SetEntity(loc, piece), Times.Once);
        protected void VerifyNewEntityWasPlaced(BoardLocation loc, ChessPieceEntity piece)
            => StateMock.Verify(m => m.SetEntity(loc, It.Is<ChessPieceEntity>(cpe => cpe.EntityType == piece.EntityType && cpe.Player == piece.Player)), Times.Once);
    }
}