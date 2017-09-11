using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using CsChess.Pgn;
using CSharpChess.System;
using NUnit.Framework;

namespace CSharpChess.UnitTests.PgnParsing
{
    [TestFixture]
    public class FullGameParsing
    {
        #region Test Games
        // https://en.wikipedia.org/wiki/Portable_Game_Notation
        private const string WikiGame = @"
[Event ""F/S Return Match""]
[Site ""Belgrade, Serbia JUG""]
[Date ""1992.11.04""]
[Round ""29""]
[White ""Fischer, Robert J.""]
[Black ""Spassky, Boris V.""]
[Result ""1/2-1/2""]
        
1. e4 e5 2. Nf3 Nc6 3. Bb5 a6 
4. Ba4 Nf6 5. O-O Be7 6. Re1 b5 7. Bb3 d6 8. c3 O-O 9. h3 Nb8 10. d4 Nbd7
11. c4 c6 12. cxb5 axb5 13. Nc3 Bb7 14. Bg5 b4 15. Nb1 h6 16. Bh4 c5 17. dxe5
Nxe4 18. Bxe7 Qxe7 19. exd6 Qf6 20. Nbd2 Nxd6 21. Nc4 Nxc4 22. Bxc4 Nb6
23. Ne5 Rae8 24. Bxf7+ Rxf7 25. Nxf7 Rxe1+ 26. Qxe1 Kxf7 27. Qe3 Qg5 28. Qxg5
hxg5 29. b3 Ke6 30. a3 Kd6 31. axb4 cxb4 32. Ra5 Nd5 33. f3 Bc8 34. Kf2 Bf5
35. Ra7 g6 36. Ra6+ Kc5 37. Ke1 Nf4 38. g3 Nxh3 39. Kd2 Kb5 40. Rd6 Kc5 41. Ra6
Nf2 42. g4 Bd3 43. Re6 1/2-1/2
";

        private const string TestGame = @"        [Event ""Hastings8283""]
        [Site ""Hastings""]
        [Date ""1982.??.??""]
        [Round ""?""]
        [White ""Plaskett, Jim""]
        [Black ""Short, Nigel D""]
        [Result ""1-0""]
        [WhiteElo ""2430""]
        [BlackElo ""2485""]
        [ECO ""E11""]

        1.d4 Nf6 2.c4 e6 3.Nf3 Bb4+ 4.Bd2 a5 5.g3 Nc6 6.Bg2 d5 7.a3 Bxd2+ 8.Nbxd2 O-O
        9.O-O Qe7 10.Qc2 Rd8 11.Rad1 e5 12.dxe5 Nxe5 13.Nxe5 Qxe5 14.Nf3 Bf5 15.Qb3 Qxe2
        16.Nd4 Qh5 17.Nxf5 Qxf5 18.Qxb7 Qc2 19.Rc1 Qd2 20.cxd5 Rab8 21.Qxc7 Nxd5
        22.Qc5 Nf6 23.Rc2 Qd3 24.Rc3 Qa6 25.b3 h6 26.Rfc1 Rd2 27.h3 Rb2 28.Qf5 g6
        29.Qf4 R8xb3 30.Rc6 Nh5 31.Qxh6  1-0";

#endregion

        [Test]
        public void can_parse_wiki_sample_game()
        {
            var pgnGameText = WikiGame;

            var pgnGame = PgnGame.Parse(pgnGameText).First();

            Assert.That(pgnGame.Event, Is.EqualTo("F/S Return Match"));
            Assert.That(pgnGame.Site, Is.EqualTo("Belgrade, Serbia JUG"));
            Assert.That(pgnGame.Date.ToString(), Is.EqualTo("1992.11.04"));
            Assert.That(pgnGame.Round, Is.EqualTo(29));
            Assert.That(pgnGame.White, Is.EqualTo("Fischer, Robert J."));
            Assert.That(pgnGame.Black, Is.EqualTo("Spassky, Boris V."));
            Assert.That(pgnGame.Result, Is.EqualTo(ChessGameResult.Draw));

            Assert.That(pgnGame.TurnQueries.Count(), Is.EqualTo(43));

            Assert.That(pgnGame.TurnQueries.Count(), Is.EqualTo(43));
        }

        [Test]
        public void can_parse_lots_of_games()
        {
            
            var pgnText = File.ReadAllText(@"C:\Src\Chess\CSharpChess.UnitTests\bin\Debug\short.pgn");

            var pgnGames = PgnGame.Parse(pgnText).ToList();

            Assert.That(pgnGames.Count(), Is.GreaterThan(0));
            Console.WriteLine($"{pgnGames.Count()} parsed.");

        }


        [Test, Explicit]
        public void can_play_lots_of_games()
        {
            var stream = File.OpenRead(@"C:\Src\Chess\CSharpChess.UnitTests\bin\Debug\short.pgn");

            int count = 0;

            using (var reader = new PgnReader(stream))
            {
                var game = reader.ReadGame();

                while (game != null)
                {
                    PlayGame(game);

                    game = reader.ReadGame();

                    Console.Out.FlushAsync();
                }
            }
            Assert.That(count, Is.GreaterThan(1));
            Console.WriteLine(count);

        }

        [Test]
        public void can_play_test_game()
        {
            PlayGame(TestGame);
        }


        [Test]
        public void can_play_pgn_with_piece_blocking_check()
        {
            var stream = File.OpenRead(@"C:\Src\Chess\CSharpChess.UnitTests\bin\Debug\has-piece-blocking-check.pgn");
            
            using (var reader = new PgnReader(stream))
            {
                var game = reader.ReadGame();

                while (game != null)
                {
                    PlayGame(game);

                    game = reader.ReadGame();
                }
            }
        }

        private void PlayGame(string game)
        {

            var sw = new Stopwatch();
            sw.Start();

            var pgnGame = PgnGame.Parse(game).Single();
            Console.WriteLine($"Processing game: {pgnGame}");

            var board = new PgnGameResolver().Resolve(pgnGame);

            var result = ChessGameResultToPgnResult(board.GameState);

            if (result != ChessGameResult.Unknown) // Resignation & Draws can't be determined from the board state alone
            {
                Assert.That(pgnGame.Result, Is.EqualTo(result), pgnGame + "\n" + game);
            }

            Console.WriteLine($"  Game took {sw.ElapsedMilliseconds}ms");
        }

        [Test]
        public void can_play_pgn_with_pawn_promotion()
        {
            var stream = File.OpenRead(@"C:\Src\Chess\CSharpChess.UnitTests\bin\Debug\has-black-pawn-to-queen-promotion.pgn");


            using (var reader = new PgnReader(stream))
            {
                var game = reader.ReadGame();

                while (game != null)
                {
                    var pgnGame = PgnGame.Parse(game).Single();
                    var board = new PgnGameResolver().Resolve(pgnGame);

                    var result = ChessGameResultToPgnResult(board.GameState);

                    if (IsInCheckmate(board.GameState))
                    {
                        Assert.That(pgnGame.Result, Is.EqualTo(result), pgnGame.ToString());
                    }

                    game = reader.ReadGame();
                }
            }
        }
        private bool IsInCheckmate(GameState boardGameState)
        {
            return new[]
                {
                    GameState.CheckMateBlackWins,
                    GameState.CheckMateWhiteWins,
                }
                .Any(a => a == boardGameState);
        }

        private static ChessGameResult ChessGameResultToPgnResult(GameState pgnGame)
        {
            switch (pgnGame)
            {
                case GameState.CheckMateBlackWins:
                    return ChessGameResult.BlackWins;
                case GameState.CheckMateWhiteWins:
                    return ChessGameResult.WhiteWins;
                case GameState.Stalemate:
                case GameState.Draw:
                    return ChessGameResult.Draw;
                case GameState.WhiteKingInCheck:
                case GameState.BlackKingInCheck:
                case GameState.WaitingForMove:
                    return ChessGameResult.Unknown;
                default:
                    throw new ArgumentOutOfRangeException(pgnGame.ToString());
            }
        }
    }
}
