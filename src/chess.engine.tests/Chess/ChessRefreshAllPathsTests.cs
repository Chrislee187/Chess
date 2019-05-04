using System.Collections.Generic;
using System.Linq;
using chess.engine.Board;
using chess.engine.Chess;
using chess.engine.Chess.Entities;
using chess.engine.Entities;
using chess.engine.Game;
using chess.engine.tests.Builders;
using Moq;
using NUnit.Framework;

namespace chess.engine.tests.Chess
{
    [TestFixture]
    public class ChessRefreshAllPathsTests
    {
        private LocatedItem<ChessPieceEntity> _whiteKing;
        private LocatedItem<ChessPieceEntity> _blackKing;
        private LocatedItem<ChessPieceEntity> _aPawn;

        [SetUp]
        public void Setup()
        {
            _whiteKing = new LocatedItem<ChessPieceEntity>(BoardLocation.At("A1"), ChessPieceEntityFactory.CreateKing(Colours.White), null);
            _blackKing = new LocatedItem<ChessPieceEntity>(BoardLocation.At("H8"), ChessPieceEntityFactory.CreateKing(Colours.Black), null);
            _aPawn = new LocatedItem<ChessPieceEntity>(BoardLocation.At("D2"), ChessPieceEntityFactory.CreatePawn(Colours.White), null);
        }
        [Test]
        public void RefreshAllPaths_generates_king_paths_last()
        {
            var items = new List<LocatedItem<ChessPieceEntity>>
            {
                _whiteKing, _blackKing, _aPawn
            };

            var boardState = CreateCustomBoardStateMock(items, _whiteKing, _blackKing);

            var refresher = new ChessRefreshAllPaths();

            refresher.RefreshAllPaths(boardState.Object, false);

            boardState.VerifyGet(g => g.GetAllItemLocations);
            boardState.Verify(g => g.GetItems(ChessPieceName.King));

            // NOTE: Moq output from Sequence mocks can be hard to read, the important fact here is that
            // king paths must be calculated after all enemy paths as they need to know whether they are moving in to
            // check or not.

            boardState.Verify(bs => bs.GeneratePaths(_blackKing.Item, _blackKing.Location, It.IsAny<bool>()));
            boardState.Verify(bs => bs.GeneratePaths(_whiteKing.Item, _whiteKing.Location, It.IsAny<bool>()));
            boardState.Verify(bs => bs.GeneratePaths(_aPawn.Item, _aPawn.Location, It.IsAny<bool>()));





        }

        private static Mock<IBoardState> CreateCustomBoardStateMock(List<LocatedItem<ChessPieceEntity>> items, LocatedItem<ChessPieceEntity> whiteKing, LocatedItem<ChessPieceEntity> blackKing)
        {
            // Create a custom board state, using Mocks,to test that refresh paths works correctly

            var boardState = new Mock<IBoardState>(MockBehavior.Strict);

            // Create the MockSequence to validate the call order
            var sequence = new MockSequence();

            // Each subsequent Setup() is only used if the previous one was successful
            // Verify order is unimportant, order here IS.
            boardState.InSequence(sequence).Setup(x
                => x.GeneratePaths(It.Is<ChessPieceEntity>(cpe => cpe.EntityType != ChessPieceName.King),
                    It.IsAny<BoardLocation>(), It.IsAny<bool>()));
            boardState.InSequence(sequence).Setup(x
                => x.GeneratePaths(It.Is<ChessPieceEntity>(cpe => cpe.EntityType == ChessPieceName.King),
                    It.IsAny<BoardLocation>(), It.IsAny<bool>()));
            boardState.InSequence(sequence).Setup(x
                => x.GeneratePaths(It.Is<ChessPieceEntity>(cpe => cpe.EntityType == ChessPieceName.King),
                    It.IsAny<BoardLocation>(), It.IsAny<bool>()));

            boardState.Setup(bs => bs.GetAllItemLocations)
                .Returns(() => items.Select(i => i.Location));
            boardState.Setup(bs => bs.GetItem(It.IsAny<BoardLocation>()))
                .Returns<BoardLocation>((l) => items.Single(i => i.Location.Equals(l)));

            boardState.Setup(bs => bs.GetItems(ChessPieceName.King))
                .Returns(new List<LocatedItem<ChessPieceEntity>> {whiteKing, blackKing});
            return boardState;
        }
    }
}