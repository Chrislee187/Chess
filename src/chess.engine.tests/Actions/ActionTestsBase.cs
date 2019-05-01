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

        protected Mock<IBoardState> StateMock = new Mock<IBoardState>();
        protected Mock<IBoardActionFactory> FactoryMock = new Mock<IBoardActionFactory>();
        protected Mock<IBoardAction> BoardActionMock = new Mock<IBoardAction>();

        protected T Action;


        protected void SetupReturnedPiece(BoardLocation at, ChessPieceEntity piece) 
            => StateMock.Setup(m => m.GetEntity(at)).Returns(piece);

        protected void SetupActionCreateForMockAction(ChessMoveType moveType)
            => FactoryMock.Setup(m => m.Create(moveType, It.IsAny<IBoardState>()))
            .Returns(BoardActionMock.Object);

        protected void VerifyLocationCleared(BoardLocation loc)
            => StateMock.Verify(s => s.ClearLocation(loc), Times.Once);
        
        protected void VerifyActionWasCreated(ChessMoveType moveType)
            => FactoryMock.Verify(m => m.Create(moveType, It.IsAny<IBoardState>()), Times.Once);

        protected void VerifyActionWasExecuted(ChessMove move)
        => BoardActionMock.Verify(m => m.Execute(move), Times.Once);

        protected void VerifyEntityRetrieved(BoardLocation loc)
            => StateMock.Verify(m => m.GetEntity(loc), Times.Once);

        protected void VerifyEntityPlaced(BoardLocation loc, ChessPieceEntity piece)
            => StateMock.Verify(m => m.SetEntity(loc, piece), Times.Once);
    }
}