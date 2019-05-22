using System.Collections.Generic;
using System.Linq;
using board.engine;
using board.engine.Board;
using board.engine.Movement;
using chess.engine.Entities;
using chess.engine.Game;
using Moq;
using NUnit.Framework;


namespace chess.engine.tests.Game
{
    [TestFixture]
    public class CheckDetectionServiceTests
    {
        private Mock<IPlayerStateService> _playerStateServiceMock;
        private Mock<IBoardState<ChessPieceEntity>> _boardStateMock;
        private Mock<IBoardMoveService<ChessPieceEntity>> _moveServiceMock;
        private LocatedItem<ChessPieceEntity> _kingItem;

        [SetUp]
        public void Setup()
        {
            _playerStateServiceMock = ChessTestFactory.ChessGameStateServiceMock();
            _boardStateMock = new Mock<IBoardState<ChessPieceEntity>>();
            _boardStateMock.Setup(mb => mb.Clone()).Returns(_boardStateMock.Object);
            _kingItem = new LocatedItem<ChessPieceEntity>(
                BoardLocation.At(1,1), 
                new PawnEntity(Colours.White), 
                null);
            _boardStateMock.Setup(mb => mb.GetItems(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(new List<LocatedItem<ChessPieceEntity>>
                    {
                        _kingItem}.AsEnumerable());

            _moveServiceMock = ChessTestFactory.BoardMoveServiceMock();
        }

        [TestCase(PlayerState.None, PlayerState.None, GameCheckState.None)]
        [TestCase(PlayerState.Check, PlayerState.None, GameCheckState.WhiteInCheck)]
        [TestCase(PlayerState.Checkmate, PlayerState.None, GameCheckState.WhiteCheckmated)]
        [TestCase(PlayerState.None, PlayerState.Check, GameCheckState.BlackInCheck)]
        [TestCase(PlayerState.None, PlayerState.Checkmate, GameCheckState.BlackCheckmated)]
        public void Check_returns_valid_check_states(PlayerState whiteState, PlayerState blackState, GameCheckState expectedGameState)
        {
            var service = new CheckDetectionService(
                ChessFactory.Logger<CheckDetectionService>(),
                _playerStateServiceMock.Object,
                ChessFactory.BoardMoveService(ChessFactory.ChessBoardActionProvider()),
                ChessFactory.FindAttackPaths()
            );

            if (expectedGameState != GameCheckState.BlackCheckmated &&
                expectedGameState != GameCheckState.WhiteCheckmated)
            {
                // give the king a get out move
                var kingPaths = new Paths();
                kingPaths.Add(new Path { new BoardMove(BoardLocation.At(1, 1), BoardLocation.At(1, 2), 1) });
                _kingItem.UpdatePaths(kingPaths);
            }
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
                _moveServiceMock.Object,
                ChessFactory.FindAttackPaths()

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