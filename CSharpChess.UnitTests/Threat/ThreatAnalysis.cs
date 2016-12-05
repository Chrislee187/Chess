using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using CSharpChess.TheBoard;
using CSharpChess.UnitTests.Helpers;
using CSharpChess.ValidMoves;
using NUnit.Framework;

// ReSharper disable InconsistentNaming

namespace CSharpChess.UnitTests.Threat
{
    [TestFixture]
    public class ThreatAnalysis : BoardAssertions
    {
        private ChessBoard _newBoard;
        private ThreatAnalyser _newBoardThreats;

        [SetUp]
        public void SetUp()
        {
            _newBoard = BoardBuilder.CustomBoard(Chess.Board.NewBoardAsOneChar, Chess.Colours.White);
            _newBoardThreats = new ThreatAnalyser(_newBoard);
            _newBoardThreats.BuildTable();
        }
        [Test]
        public void pawn_at_B2_generates_threat_against_A3_and_C3()
        {
            Assert.That(_newBoardThreats.AttackingFrom(BoardLocation.At("B2")), Contains.Item((BoardLocation)"A3"));
            Assert.That(_newBoardThreats.AttackingFrom(BoardLocation.At("B2")), Contains.Item((BoardLocation)"C3"));
        }

        [Test]
        public void A3_and_C3_have_threat_from_B2()
        {
            Assert.That(_newBoardThreats.Attacking(BoardLocation.At("A3"), Chess.Colours.Black), Contains.Item(BoardLocation.At("B2")));
            Assert.That(_newBoardThreats.Attacking(BoardLocation.At("C3"), Chess.Colours.Black), Contains.Item(BoardLocation.At("B2")));
        }

        [Test]
        public void normal_pawn_movement_does_not_generate_threat()
        {
            foreach (Chess.ChessFile file in Enum.GetValues(typeof(Chess.ChessFile)))
            {
                foreach (var rank in new [] { 3,4,5,6})
                {
                    var colour = rank < 5 ? Chess.Colours.Black : Chess.Colours.White;
                    var pawnFile = rank < 5 ? 2 : 7;
                    var loc = BoardLocation.At(file, rank);
                    var pawnLocation = BoardLocation.At(file, pawnFile);

                    CollectionAssert.DoesNotContain(_newBoardThreats.Attacking(loc, Chess.Colours.Black), pawnLocation);
                }
            }
        }

        [TestCase(Chess.PieceNames.Pawn)]
        [TestCase(Chess.PieceNames.Knight)]
        public void pwns_and_knights_generate_threat(Chess.PieceNames piece)
        {
            Assert.That(_newBoard.Pieces
                .Where(p => p.Piece.Is(piece))
                .Where(p => !_newBoardThreats.AttackingFrom(p.Location).Any())
                , Is.Empty);
        }

        [Test]
        public void other_pieces_generate_threat()
        {
            Assert.Fail("TODO");
        }
    }

    public class ThreatAnalyser
    {
        private readonly ChessBoard _board;
        private readonly ValidMoveFactory _validMoveFactory = new ValidMoveFactory();

        private readonly IDictionary<Chess.Colours, ThreatDictionary> _attackingFrom;

        public ThreatAnalyser(ChessBoard board)
        {
            _board = board;
            _attackingFrom = new ConcurrentDictionary<Chess.Colours, ThreatDictionary>();
        }

        public void BuildTable()
        {
            _attackingFrom.Clear();
            _attackingFrom.Add(Chess.Colours.White, new ThreatDictionary());
            _attackingFrom.Add(Chess.Colours.Black, new ThreatDictionary());

            foreach (var attackerColour in _attackingFrom.Keys)
            {
                var threatDict = _attackingFrom[attackerColour];
                foreach (var boardPiece in _board.Pieces.Where(bp => bp.Piece.Is(attackerColour)))
                {
                    var validMoves = _validMoveFactory.GetThreateningMoves(_board, boardPiece.Location).ToList();

                    var threateningMoves = new List<BoardLocation>();

                    if (validMoves.Any())
                    {
                        threateningMoves.AddRange(_validMoveFactory.GetThreateningMoves(_board, boardPiece.Location));
                    }
                    threatDict.Add(boardPiece.Location, threateningMoves.ToList());
                }
            }
        }

        private class ThreatDictionary : Dictionary<BoardLocation, IEnumerable<BoardLocation>>
        {

        }

        public IEnumerable<BoardLocation> AttackingFrom(BoardLocation boardLocation)
        {
            var attacksFrom = _attackingFrom[_board[boardLocation].Piece.Colour];

            return attacksFrom[boardLocation];
        }

        public IEnumerable<BoardLocation> Attacking(BoardLocation boardLocation, Chess.Colours defendingColour)
        {
            return _attackingFrom[Chess.ColourOfEnemy(defendingColour)]
                    .Where(v => v.Value.Contains(boardLocation))
                    .Select(v => v.Key);
        }
    }
}