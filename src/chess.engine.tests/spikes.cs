using System;
using System.Linq;
using chess.engine.Board;
using chess.engine.Chess;
using chess.engine.Entities;
using chess.engine.Game;
using chess.engine.Movement;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using NUnit.Framework;

namespace chess.engine.tests
{
    [TestFixture]
    public class spikes
    {
        [Test]
        public void Should()
        {
            var engine = new BoardEngine<ChessPieceEntity>(new ChessBoardSetup(), 
                new ChessPathsValidator(new ChessPathValidator(new MoveValidationFactory<ChessPieceEntity>())),
                    new ChessRefreshAllPaths(NullLogger<ChessRefreshAllPaths>.Instance));

            var startLocation = BoardLocation.At("B2");

            var piece = engine.BoardState.GetItem(startLocation);

            Assert.That(piece.Item.Piece, Is.EqualTo(ChessPieceName.Pawn));

            var paths = piece.Paths;
            Assert.That(paths.Count(), Is.EqualTo(1));
            Assert.That(paths.SelectMany(m => m).Count(), Is.EqualTo(2));
        }

        [Test]
        public void Spike_easy_board_builder_to_from_ChessGame()
        {
            var setup = new EasyBoardBuilder()
                    //                .X(8, "rnbqkbnr")
                    //                .Y(ChessFile.A, "RP    pr")
                    //                .At(ChessFile.D, 2, 'P')
                    //                .FromChessGame(new ChessGame())
                    .Board("rnbqkbnr" +
                           "pppppppp" +
                           "        " +
                           "        " +
                           "        " +
                           "        " +
                           "PPPPPPPP" +
                           "RNBQKBNR")
                    .ToGameSetup()
                ;

            IMoveValidationFactory<ChessPieceEntity> validationFactory = new MoveValidationFactory<ChessPieceEntity>();
            var game = new ChessGame(new ChessRefreshAllPaths(NullLogger<ChessRefreshAllPaths>.Instance), setup, new ChessPathsValidator(new ChessPathValidator(validationFactory)));

            var board = new EasyBoardBuilder().FromChessGame(game).ToString();
            Console.WriteLine(board);
        }

        [Test]
        public void Spike_easy_board_builder_to_from_ChessGame2()
        {
            var setup = new EasyBoardBuilder()
                    //                .X(8, "rnbqkbnr")
                    //                .Y(ChessFile.A, "RP    pr")
                    //                .At(ChessFile.D, 2, 'P')
                    //                .FromChessGame(new ChessGame())
                    .Board("rnbqkbnr" +
                           "pppppppp" +
                           "        " +
                           "        " +
                           "        " +
                           "        " +
                           "PPPPPPPP" +
                           "RNBQKBNR")
                    .ToGameSetup()
                ;

            IMoveValidationFactory<ChessPieceEntity> validationFactory = new MoveValidationFactory<ChessPieceEntity>();
            var game = new ChessGame(new ChessRefreshAllPaths(NullLogger<ChessRefreshAllPaths>.Instance), setup, new ChessPathsValidator(new ChessPathValidator(validationFactory)));

            var board = new EasyBoardBuilder().FromChessGame(game).ToString();
            Console.WriteLine(board);
        }

        [Test]
        public void Spike_debugging_a_move_problem()
        {
            var board = new EasyBoardBuilder()
                    //                .X(8, "rnbqkbnr")
                    //                .Y(ChessFile.A, "RP    pr")
                    //                .At(ChessFile.D, 2, 'P')
                    //                .FromChessGame(new ChessGame())
                    .Board("r.bq.rk." +
                           "...nbppp" +
                           "p.pp.n.." +
                           ".p..p..." +
                           "..PPP..." +
                           ".B...N.P" +
                           "PP...PP." +
                           "RNBQR.K.")
                ;

            IMoveValidationFactory<ChessPieceEntity> validationFactory = new MoveValidationFactory<ChessPieceEntity>();
            var game = new ChessGame(new ChessRefreshAllPaths(NullLogger<ChessRefreshAllPaths>.Instance), board.ToGameSetup(), new ChessPathsValidator(new ChessPathValidator(validationFactory)), Colours.White);

            // TODO: Fix this bug, pawn can't take pawn, something to do with
            // enpassant i guess based on pawn positions

            var msg = game.Move("c4b5");

            Assert.IsEmpty(msg, msg);
        }

        [Test]
        public void Should_play_the_manually_parsed_wiki_gamed()
        {
            IMoveValidationFactory<ChessPieceEntity> moveValidationFactory = new MoveValidationFactory<ChessPieceEntity>();
            var game = new ChessGame(new ChessRefreshAllPaths(NullLogger<ChessRefreshAllPaths>.Instance), new ChessPathsValidator(new ChessPathValidator(moveValidationFactory)));
            var moveIdx = 0;
            foreach (var move in ManullyParsedWikiGame)
            {
                Console.WriteLine($"Move #{++moveIdx}: {move}");
                var msg = game.Move(move);
                Console.WriteLine(game.GameState.ToString() + " - " + game.CurrentPlayer + " to move.");
                Console.WriteLine(game.ToText());
                if (!string.IsNullOrEmpty(msg))
                {
                    if (msg.Contains("Error:")) Assert.Fail($"Error: {msg}");
                    Console.WriteLine(msg);
                }
            }

            Console.WriteLine("GAME OVER!");
        }

        // Manually parsed to co-ordinate notation from
        // https://en.wikipedia.org/wiki/Portable_Game_Notation
        private static readonly string[] ManullyParsedWikiGame = new[]
        {
            "e2e4",
            "e7e5",
            "g1f3",
            "B8c6",
            "f1b5",
            "a7a6",
            "b5a4",
            "g8f6",
            "e1g1",
            "f8e7",
            "f1e1",
            "b7b5",
            "a4b3",
            "d7d6",
            "c2c3",
            "e8g8",
            "h2h3",
            "c6b8",
            "d2d4",
            "b8d7",
            "c3c4",
            "c7c6",
            "c4b5",
            "a6b5",
            "b1c3",
            "c8b7",
            "c1g5",
            "b5b4",
            "c3b1",
            "h7h6",
            "g5h4",
            "c6c5",
            "d4e5",
            "f6e4",
            "h4e7",
            "d8e7",
            "e5d6",
            "e7f6",
            "b1d2",
            "e4d6",
            "d2c4",
            "d6c4",
            "b3c4",
            "d7b6",
            "f3e5",
            "a8e8",
            "c4f7",
            "f8f7",
            "e5f7",
            "e8e1",
            "d1e1",
            "g8f7",
            "e1e3",
            "f6g5",
            "e3g5",
            "h6g5",
            "b2b3",
            "f7e6",
            "a2a3",
            "e6d6",
            "a3b4",
            "c5b4",
            "a1a5",
            "b6d5",
            "f2f3",
            "b7c8",
            "g1f2",
            "c8f5",
            "a5a7",
            "g7g6",
            "a7a6",
            "d6c5",
            "f2e1",
            "d5f4",
            "g2g3",
            "f4h3",
            "e1d2",
            "c5b5",
            "a6d6",
            "b5c5",
            "d6a6",
            "h3f2",
            "g3g4",
            "f5d3",
            "a6e6"
        };
    }
}