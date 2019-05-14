using chess.engine.Algebraic;
using chess.engine.Chess;
using NUnit.Framework;

namespace chess.engine.tests.Algebraic
{
    [TestFixture]
    public class AlgebraicNotationTests
    {
        [TestCase("Re6", ChessPieceName.Rook)]
        [TestCase("Ne6", ChessPieceName.Knight)]
        [TestCase("Be6", ChessPieceName.Bishop)]
        [TestCase("Qe6", ChessPieceName.Queen)]
        [TestCase("Ke6", ChessPieceName.King)]
        [TestCase("e1", ChessPieceName.Pawn)]
        public void ShouldParsePieceName(string notation, ChessPieceName piece)
        {
            Assert.That(StandardAlgebraicNotation.TryParse(notation, out var an));

            Assert.That(an.Piece, Is.EqualTo(piece));
        }

        [TestCase("Rxe6", ChessPieceName.Rook)]
        public void ShouldParsePieceNameFromComplexNotations(string notation, ChessPieceName piece)
        {
            Assert.That(StandardAlgebraicNotation.TryParse(notation, out var an));

            Assert.That(an.Piece, Is.EqualTo(piece));
        }

        [TestCase("Rxe6")]
        public void ShouldParseTakeMoves(string notation)
        {
            Assert.That(StandardAlgebraicNotation.TryParse(notation, out var an));

            Assert.That(an.MoveType, Is.EqualTo(SanMoveTypes.Take));
        }

        [TestCase("Re6", 5)]
        [TestCase("Rxe6", 5)]
        public void ShouldParseToFile(string notation, int file)
        {
            Assert.That(StandardAlgebraicNotation.TryParse(notation, out var an));

            Assert.True(an.ToFile.HasValue);
            Assert.That(an.ToFile.Value, Is.EqualTo(file));
        }

        [TestCase("Re6", 6)]
        [TestCase("Rxe6", 6)]
        [TestCase("Ra6e6", 6)]
        [TestCase("Ra6xe6", 6)]
        public void ShouldParseToRank(string notation, int rank)
        {
            Assert.That(StandardAlgebraicNotation.TryParse(notation, out var an));

            Assert.True(an.ToFile.HasValue);
            Assert.That(an.ToRank.Value, Is.EqualTo(rank));
        }

        [TestCase("Rae6", 1)]
        [TestCase("Raxe6", 1)]
        public void ShouldParseFromFile(string notation, int file)
        {
            Assert.That(StandardAlgebraicNotation.TryParse(notation, out var an));

            Assert.True(an.FromFile.HasValue);
            Assert.That(an.FromFile.Value, Is.EqualTo(file));
        }

        [TestCase("Ra6e6", 1)]
        [TestCase("Ra6xe6", 1)]
        public void ShouldParseFromRank(string notation, int rank)
        {
            Assert.That(StandardAlgebraicNotation.TryParse(notation, out var an));

            Assert.True(an.FromFile.HasValue);
            Assert.That(an.FromFile.Value, Is.EqualTo(rank));
        }
        [TestCase("e8+Q", ChessPieceName.Queen)]
        [TestCase("dxe8+Q", ChessPieceName.Queen)]
        [TestCase("d7xe8+Q", ChessPieceName.Queen)]
        public void ShouldParsePromotion(string notation, ChessPieceName piece)
        {
            Assert.That(StandardAlgebraicNotation.TryParse(notation, out var an));

            Assert.That(an.PromotionPiece.HasValue);
            Assert.That(an.PromotionPiece.Value, Is.EqualTo(piece));
        }

        [TestCase("x8+Q")]
        [TestCase("DD4xe8")]
        [TestCase("e8+Z")]
        public void ShouldFailParsing(string notation)
        {
            Assert.False(StandardAlgebraicNotation.TryParse(notation, out var an));
        }
    }

}