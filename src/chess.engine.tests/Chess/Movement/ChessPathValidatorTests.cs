using System.Collections.Generic;
using System.Linq;
using board.engine.Actions;
using board.engine.Movement;
using chess.engine.Chess;
using chess.engine.Chess.Entities;
using chess.engine.Extensions;
using chess.engine.Game;
using chess.engine.tests.Builders;
using chess.engine.tests.Movement;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using NUnit.Framework;

namespace chess.engine.tests.Chess.Movement
{
    [TestFixture]
    public class ChessPathValidatorTests : PathGeneratorTestsBase
    {
        private Mock<IMoveValidationProvider<ChessPieceEntity>> _factoryMock;
        private IEnumerable<BoardMovePredicate<ChessPieceEntity>> _moveTests;


        [SetUp]
        public new void SetUp()
        {
            base.SetUp();
            _factoryMock = new Mock<IMoveValidationProvider<ChessPieceEntity>>();

            _moveTests = new List<BoardMovePredicate<ChessPieceEntity>>
            {
                (move, state) => true
            };
            _factoryMock.Setup(f => f.TryGetValue(It.IsAny<int>(), out _moveTests))
                .Returns(true);

        }

        [Test]
        public void ValidPath_should_return_validPath()
        {
            var validator = new ChessPathValidator(NullLogger<ChessPathValidator>.Instance, _factoryMock.Object);
            var path = new ChessPathBuilder().Build();

            validator.ValidatePath(BoardStateMock.Object, path);

            Assert.That(path.Any());
        }

        [Test]
        public void ValidPath_should_return_truncated_path_when_move_test_fails()
        {
            var validator = new ChessPathValidator(NullLogger<ChessPathValidator>.Instance, _factoryMock.Object);
            var path = new ChessPathBuilder().From("D2").To("D3").To("D4").To("D5", (int)DefaultActions.TakeOnly).Build();

            IEnumerable<BoardMovePredicate<ChessPieceEntity>> failOnD5 = new List<BoardMovePredicate<ChessPieceEntity>>
            {
                (move, state) => !move.To.Equals("D5".ToBoardLocation())
            };

            _factoryMock.Setup(f => f.TryGetValue(
                    It.IsAny<int>(), 
                    out failOnD5))
                .Returns(true);

            var validPath = validator.ValidatePath(BoardStateMock.Object, path);
            
            AssertPathContains(new List<Path>{validPath}, 
                new ChessPathBuilder().From("D2")
                    .To("D3")
                    .To("D4")
                    .Build(), Colours.White);
        }

        [Test]
        public void ValidPath_should_throw_for_unsupported_MoveType()
        {
            _factoryMock.Setup(f => f.TryGetValue(It.IsAny<int>(), out _moveTests))
                .Returns(false);

            var validator = new ChessPathValidator(NullLogger<ChessPathValidator>.Instance, _factoryMock.Object);
            var path = new ChessPathBuilder().Build();
            Assert.That(() => validator.ValidatePath(BoardStateMock.Object, path), 
                Throws.Exception);
        }
    }
}