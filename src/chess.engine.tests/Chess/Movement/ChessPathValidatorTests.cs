using System.Collections.Generic;
using System.Linq;
using chess.engine.Actions;
using chess.engine.Chess;
using chess.engine.Entities;
using chess.engine.Game;
using chess.engine.Movement;
using chess.engine.tests.Builders;
using chess.engine.tests.Movement;
using Moq;
using NUnit.Framework;

namespace chess.engine.tests.Chess.Movement
{
    [TestFixture]
    public class ChessPathValidatorTests : PathGeneratorTestsBase
    {
        private Mock<IReadOnlyDictionary<MoveType, IEnumerable<BoardMovePredicate<ChessPieceEntity>>>> _factoryMock;
        private IEnumerable<BoardMovePredicate<ChessPieceEntity>> _moveTests;
        private readonly IBoardActionFactory<ChessPieceEntity> _boardActionFactory = new BoardActionFactory<ChessPieceEntity>();


        [SetUp]
        public new void SetUp()
        {
            base.SetUp();
            _factoryMock = new Mock<IReadOnlyDictionary<MoveType, IEnumerable<BoardMovePredicate<ChessPieceEntity>>>>();

            _moveTests = new List<BoardMovePredicate<ChessPieceEntity>>
            {
                (move, state) => true
            };
            _factoryMock.Setup(f => f.TryGetValue(It.IsAny<MoveType>(), out _moveTests))
                .Returns(true);

        }

        [Test]
        public void ValidPath_should_return_validPath()
        {
            var validator = new ChessPathValidator(_factoryMock.Object);
            var path = new PathBuilder().Build();

            validator.ValidatePath(BoardStateMock.Object, path);

            Assert.That(path.Any());
        }

        [Test]
        public void ValidPath_should_return_truncated_path_when_move_test_fails()
        {
            var validator = new ChessPathValidator(_factoryMock.Object);
            var path = new PathBuilder().From("D2").To("D3").To("D4").To("D5", MoveType.TakeOnly).Build();

            IEnumerable<BoardMovePredicate<ChessPieceEntity>> failOnD5 = new List<BoardMovePredicate<ChessPieceEntity>>
            {
                (move, state) => !move.To.Equals(BoardLocation.At("D5"))
            };

            _factoryMock.Setup(f => f.TryGetValue(
                    It.IsAny<MoveType>(), 
                    out failOnD5))
                .Returns(true);

            var validPath = validator.ValidatePath(BoardStateMock.Object, path);
            
            AssertPathContains(new List<Path>{validPath}, 
                new PathBuilder().From("D2")
                    .To("D3")
                    .To("D4")
                    .Build(), Colours.White);
        }

        [Test]
        public void ValidPath_should_throw_for_unsupported_MoveType()
        {
            _factoryMock.Setup(f => f.TryGetValue(It.IsAny<MoveType>(), out _moveTests))
                .Returns(false);

            var validator = new ChessPathValidator(_factoryMock.Object);
            var path = new PathBuilder().Build();
            Assert.That(() => validator.ValidatePath(BoardStateMock.Object, path), 
                Throws.Exception);
        }
    }
}