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
        private Mock<IChessGameStateService> _chessGameStateService;
        private Mock<IBoardState<ChessPieceEntity>> _mockedBoard;

        [SetUp]
        public void Setup()
        {
            _chessBoardActionProvider = ChessTestFactory.ChessBoardActionProvider();
            _chessGameStateService = ChessTestFactory.ChessGameStateService();
            _mockedBoard = new Mock<IBoardState<ChessPieceEntity>>();
            _mockedBoard.Setup(mb => mb.Clone()).Returns(_mockedBoard.Object);
        }

        [TestCase(GameState.InProgress, GameState.InProgress, GameCheckState.None)]
        [TestCase(GameState.Check, GameState.InProgress, GameCheckState.WhiteInCheck)]
        [TestCase(GameState.Checkmate, GameState.InProgress, GameCheckState.WhiteCheckmated)]
        [TestCase(GameState.InProgress, GameState.Check, GameCheckState.BlackInCheck)]
        [TestCase(GameState.InProgress, GameState.Checkmate, GameCheckState.BlackCheckmated)]
        public void Check_returns_valid_check_states(GameState whiteState, GameState blackState, GameCheckState expectedGameState)
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

            SetupCheckState(GameState.Check, Colours.White);
            SetupCheckState(GameState.Checkmate, Colours.Black);

            Assert.That(() => service.Check(_mockedBoard.Object), Throws.Exception);
        }
        private void SetupCheckState(GameState inProgress, Colours colours)
        {
            _chessGameStateService.Setup(s
                    => s.CurrentGameState(
                        It.IsAny<IBoardState<ChessPieceEntity>>(),
                        It.Is<Colours>(c => c == colours)))
                .Returns(inProgress);
        }
    }
}