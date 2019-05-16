using board.engine.Actions;
using board.engine.Board;
using chess.engine.Chess;
using chess.engine.Chess.Entities;
using chess.engine.Game;
using Moq;
using NUnit.Framework;

namespace chess.engine.tests.Chess
{
    [TestFixture]
    public class CheckDetectionServiceTests
    {
        private Mock<IBoardActionProvider<ChessPieceEntity>> _chessBoardActionProvider;
        private Mock<IPlayerStateService> _chessGameStateService;
        private Mock<IBoardState<ChessPieceEntity>> _mockedBoard;

        [SetUp]
        public void Setup()
        {
            _chessBoardActionProvider = ChessTestFactory.ChessBoardActionProvider();
            _chessGameStateService = ChessTestFactory.ChessGameStateService();
            _mockedBoard = new Mock<IBoardState<ChessPieceEntity>>();
            _mockedBoard.Setup(mb => mb.Clone()).Returns(_mockedBoard.Object);
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
                _chessBoardActionProvider.Object,
                _chessGameStateService.Object
            );

            SetupCheckState(whiteState, Colours.White);
            SetupCheckState(blackState, Colours.Black);

            Assert.That(service.Check(_mockedBoard.Object), Is.EqualTo(expectedGameState));
        }

        [Test]
        public void Check_throws_if_both_sides_in_check()
        {
            var service = new CheckDetectionService(
                ChessFactory.Logger<CheckDetectionService>(),
                _chessBoardActionProvider.Object,
                _chessGameStateService.Object
            );

            SetupCheckState(PlayerState.Check, Colours.White);
            SetupCheckState(PlayerState.Checkmate, Colours.Black);

            Assert.That(() => service.Check(_mockedBoard.Object), Throws.Exception);
        }
        private void SetupCheckState(PlayerState inProgress, Colours colours)
        {
            _chessGameStateService.Setup(s
                    => s.CurrentPlayerState(
                        It.IsAny<IBoardState<ChessPieceEntity>>(),
                        It.Is<Colours>(c => c == colours)))
                .Returns(inProgress);
        }
    }
}