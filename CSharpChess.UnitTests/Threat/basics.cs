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
    [TestFixture, Explicit("WIP")]
    public class basics : BoardAssertions
    {
        [Test]
        public void pawn_at_B2_generates_threat_against_A3_and_C3()
        {
            var asOneChar =
               "rnbqkbnr" +
               "pppppppp" +
               "........" +
               "........" +
               "........" +
               "........" +
               "PPPPPPPP" +
               "RNBQKBNR";

            var board = BoardBuilder.CustomBoard(asOneChar, Chess.Colours.White);

            var threats = new ThreatAnalyser(board);
            threats.BuildTable();
            Assert.That(threats.AttackingFrom(BoardLocation.At("B2"), Chess.Colours.White), Contains.Item((BoardLocation)"A3"));
            Assert.That(threats.AttackingFrom(BoardLocation.At("B2"), Chess.Colours.White), Contains.Item((BoardLocation)"C3"));
        }
        [Test]
        public void feature()
        {
            var asOneChar =
               "rnbqkbnr" +
               "pppppppp" +
               "........" +
               "........" +
               "........" +
               "........" +
               "PPPPPPPP" +
               "RNBQKBNR";

            var board = BoardBuilder.CustomBoard(asOneChar, Chess.Colours.White);

            Assert.That(board.DefendingFrom("B3", Chess.Colours.Black).Contains(BoardLocation.At("A2")), "No ThreatAgainst found");
//            Assert.That(board.AttackingFrom("A2").Contains(BoardLocation.At("B3")), "No ThreatFrom found");

            var moves = new PawnValidMoveGenerator().For(board, "D4");

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
                    var validMoves = _validMoveFactory.GetValidMoves(_board, boardPiece.Location).ToList();

                    var threateningMoves = new List<BoardLocation>();

                    if (validMoves.Any())
                    {
                        switch (boardPiece.Piece.Name)
                        {
                            case Chess.PieceNames.Pawn:
                                threateningMoves.AddRange(PawnThreats(validMoves));
                                break;
                            case Chess.PieceNames.Rook:
                                break;
                            case Chess.PieceNames.Bishop:
                                break;
                            case Chess.PieceNames.Knight:
                                break;
                            case Chess.PieceNames.King:
                                break;
                            case Chess.PieceNames.Queen:
                                break;
                            case Chess.PieceNames.Blank:
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                    threatDict.Add(boardPiece.Location, threateningMoves.ToList());
                }
            }
        }

        private static IEnumerable<BoardLocation> PawnThreats(List<ChessMove> validMoves)
        {
            /*
             * TODO: Pawn has slightly different logic as the take moves are the only threats but there only 'Valid' moves if an opposing piece is on the square
             *  - Refactor MoveFactory's to implement ValidMoves & ThreateningMoves to encapsulate this logic.
             */
            return validMoves.Where(vm => vm.MoveType == MoveType.Take || vm.MoveType == MoveType.TakeEnPassant).Select(m => m.To);
        }

        private class ThreatDictionary : Dictionary<BoardLocation, IEnumerable<BoardLocation>>
        {

        }

        public IEnumerable<BoardLocation> AttackingFrom(BoardLocation boardLocation, Chess.Colours asPlayer)
        {
            var attacksFrom = _attackingFrom[asPlayer];

            return attacksFrom[boardLocation];
        }
    }
}