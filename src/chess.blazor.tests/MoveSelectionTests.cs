using chess.blazor.Shared.Chess;
using chess.blazor.tests.Builders;
using chess.webapi.client.csharp;
using NUnit.Framework;

namespace chess.blazor.tests
{
    public class MoveSelectionTests
    {
        private MoveSelection _moveSelection;

        private MoveSelectionCellProviderMocker _cellsProviderMocker;

        private const bool WhiteToPlay = true;
        private const bool BlackToPlay = false;
        private const string ALocation = "e1";
        private const string ADestination = "e2";

        [SetUp]
        public void Setup()
        {
            _cellsProviderMocker = new MoveSelectionCellProviderMocker();

            _cellsProviderMocker.SetupGet(new BoardCellComponentBuilder().Build());
            _cellsProviderMocker.SetupContainsPlayerPiece(true);

            _moveSelection = new MoveSelection(_cellsProviderMocker.Instance());

        }

        [Test]
        public void Valid_selection_sets_From_and_highlights_From_and_To_destinations()
        {
            var availableMoves = new Move[] { };

            _moveSelection.Selected(ALocation, availableMoves, WhiteToPlay);

            Assert.That(_moveSelection.From, Is.EqualTo(ALocation));

            _cellsProviderMocker.VerifySourceCellIsSet(ALocation);
            _cellsProviderMocker.VerifySomeDestinationCellsWereSet();
        }

        [Test]
        public void Selection_that_does_not_contain_valid_location_is_ignored_key()
        {
            var availableMoves = new Move[] {};

            _cellsProviderMocker.SetupGetFail();

            _moveSelection.Selected("invalid-location", availableMoves, WhiteToPlay);
            Assert.That(_moveSelection.From, Is.EqualTo(string.Empty));

            _cellsProviderMocker.VerifySourceCellWasNotSet();
            _cellsProviderMocker.VerifyDestinationCellsWereNotSet();
        }

        [Test]
        public void Selection_that_does_not_contain_current_players_piece_is_ignored()
        {
            var availableMoves = new Move[] { };

            _cellsProviderMocker.SetupContainsPlayerPiece(false);

            _moveSelection.Selected(ALocation, availableMoves, WhiteToPlay);
            Assert.That(_moveSelection.From, Is.EqualTo(string.Empty));

            _cellsProviderMocker.VerifySourceCellWasNotSet();
            _cellsProviderMocker.VerifyDestinationCellsWereNotSet();
        }


        [Test]
        public void Reselecting_same_location_deselects_location()
        {
            var availableMoves = new Move[] { };

            _moveSelection.Selected(ALocation, availableMoves, WhiteToPlay);
            _moveSelection.Selected(ALocation, availableMoves, WhiteToPlay);

            Assert.That(_moveSelection.From, Is.EqualTo(string.Empty));

            _cellsProviderMocker.VerifySourceCleared();
            _cellsProviderMocker.VerifyDestinationsCleared();
        }

        [Test]
        public void Selecting_new_location_of_correct_piece_colour_updates_From()
        {
            var location2 = "e2";
            var availableMoves1 = new Move[] { };
            var availableMoves2 = new Move[] { };

            _moveSelection.Selected(ALocation, availableMoves1, WhiteToPlay);
            _moveSelection.Selected(location2, availableMoves2, WhiteToPlay);

            Assert.That(_moveSelection.From, Is.EqualTo(location2));

            _cellsProviderMocker.VerifySourceCellIsSet(location2);
            _cellsProviderMocker.VerifySomeDestinationCellsWereSet();
        }

        [Test]
        public void Selecting_invalid_destination_location_is_ignored()
        {

            var availableMoves1 = new Move[] { };
            var availableMoves2 = new Move[] { };

            _moveSelection.Selected(ALocation, availableMoves1, WhiteToPlay);

            _cellsProviderMocker.SetupGet(new BoardCellComponentBuilder().WithEmptySquare().Build(), ADestination);
            _moveSelection.Selected(ADestination, availableMoves2, WhiteToPlay);

            Assert.That(_moveSelection.From, Is.EqualTo(ALocation));
            Assert.That(_moveSelection.To, Is.EqualTo(string.Empty));
        }

        [Test]
        public void Selecting_valid_destination_sets_To()
        {
            var availableMoves = new Move[] { };

            _moveSelection.Selected(ALocation, availableMoves, WhiteToPlay);

            _cellsProviderMocker.SetupGet(new BoardCellComponentBuilder().WithEmptySquare().WithIsDestinationLocation().Build(), ADestination);
            _moveSelection.Selected(ADestination, availableMoves, WhiteToPlay);

            Assert.That(_moveSelection.From, Is.EqualTo(ALocation));
            Assert.That(_moveSelection.To, Is.EqualTo(ADestination));
        }
    }
}