using board.engine;
using board.engine.Actions;
using board.engine.Board;
using chess.engine.Entities;
using chess.engine.Game;
using Moq;
using NUnit.Framework;

namespace chess.engine.tests.Game
{
    [TestFixture]
    public class CheckDetectionServiceTests
    {
        private Mock<IBoardActionProvider<ChessPieceEntity>> _actionProviderMock;
        private Mock<IPlayerStateService> _playerStateServiceMock;
        private Mock<IBoardState<ChessPieceEntity>> _boardStateMock;
        private Mock<IBoardMoveService<ChessPieceEntity>> _moveServiceMock;

        [SetUp]
        public void Setup()
        {
            _actionProviderMock = ChessTestFactory.BoardActionProviderMock();
            _playerStateServiceMock = ChessTestFactory.ChessGameStateServiceMock();
            _boardStateMock = new Mock<IBoardState<ChessPieceEntity>>();
            _boardStateMock.Setup(mb => mb.Clone()).Returns(_boardStateMock.Object);
            _moveServiceMock = ChessTestFactory.BoardMoveServiceMock();
        }

        [TestCase(PlayerState.InProgress, PlayerState.InProgress, GameCheckState.None)]
        [TestCase(PlayerState.Check, PlayerState.InProgress, GameCheckState.WhiteInCheck)]
        [TestCase(PlayerState.Checkmate, PlayerState.InProgress, GameCheckState.WhiteCheckmated)]
        [TestCase(PlayerState.InProgress, PlayerState.Check, GameCheckState.BlackInCheck)]
        [TestCase(PlayerState.InProgress, PlayerState.Checkmate, GameCheckState.BlackCheckmated)]
        public void Check_returns_valid_check_states(PlayerState whiteState, PlayerState blackState, GameCheckState expectedGameState)
        {
            var service = new CheckDetectionService(
                ChessFactory.Logger<CheckDetectionService>(),
                _playerStateServiceMock.Object,
                ChessFactory.BoardMoveService(ChessFactory.ChessBoardActionProvider())
            );

            SetupCheckState(whiteState, Colours.White);
            SetupCheckState(blackState, Colours.Black);

            Assert.That(service.Check(_boardStateMock.Object), Is.EqualTo(expectedGameState));
        }

        [Test]
        public void Check_throws_if_both_sides_in_check()
        {
            var service = new CheckDetectionService(
                ChessFactory.Logger<CheckDetectionService>(),
                _playerStateServiceMock.Object,
                _moveServiceMock.Object
            );

            SetupCheckState(PlayerState.Check, Colours.White);
            SetupCheckState(PlayerState.Checkmate, Colours.Black);

            Assert.That(() => service.Check(_boardStateMock.Object), Throws.Exception);
        }
        private void SetupCheckState(PlayerState inProgress, Colours colours)
        {
            _playerStateServiceMock.Setup(s
                    => s.CurrentPlayerState(
                        It.IsAny<IBoardState<ChessPieceEntity>>(),
                        It.Is<Colours>(c => c == colours)))
                .Returns(inProgress);
        }
    }
}