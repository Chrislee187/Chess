using chess.pgn;
using chess.web.Pages.Pgn.Convert;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moq;
using NUnit.Framework;

namespace chess.web.tests
{
    public class ConvertModelTests
    {
        private const string AnyPgn = "Some PGN text";
        private const string AnyJson = "Some JSON text";
        private Mock<IPgnSerialisationService> _serialisationService;
        private ConvertModel _model;

        [SetUp]
        public void Setup()
        {
            _serialisationService = new Mock<IPgnSerialisationService>();
            _model = new ConvertModel(_serialisationService.Object);
        }

        [Test]
        public void OnGet_returns_default_content()
        {
            var result = _model.OnGet();

            Assert.That(result, Is.TypeOf<PageResult>());
            Assert.That(_model.PgnText, Is.EqualTo(ConvertModel.WikiPgnText));
            Assert.That(_model.PgJson, Is.EqualTo(ConvertModel.DefaultJson));
        }

        [Test]
        public void OnPost_serializes_PgnText()
        {
            _model.PgnText = AnyPgn;

            SetupSerialiseAllGamesReturn(AnyPgn, AnyJson);

            var result = _model.OnPost();

            Assert.That(result, Is.TypeOf<PageResult>());
            Assert.That(_model.PgnText, Is.EqualTo(AnyPgn));
            Assert.That(_model.PgJson, Is.EqualTo(AnyJson));
        }

        [Test]
        public void OnPost_returns_error_when_model_invalid()
        {
            new RazorPagesModelTester(_model)
                .WithInvalidModelState();
            
            var result = _model.OnPost();

            Assert.That(result, Is.TypeOf<PageResult>());
            Assert.That(_model.PgJson, Is.EqualTo("MODEL INVALID"));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void OnPost_uses_ExpandedFormat_flag(bool uses)
        {
            _model.ExpandedFormat = uses;

            var result = _model.OnPost();

            Assert.That(result, Is.TypeOf<PageResult>());

            _serialisationService.Verify(ss 
                => ss.SerializeAllGames(
                    It.IsAny<string>(), 
                    It.Is<bool>(v => v.Equals(uses)))
                );
        }

        private void SetupSerialiseAllGamesReturn(string forPgn, string someJson)
        {
            _serialisationService
                .Setup(ss => ss.SerializeAllGames(
                    It.Is<string>(s => s.Equals(forPgn)), 
                    It.IsAny<bool>()))
                .Returns(someJson);
        }
    }
}