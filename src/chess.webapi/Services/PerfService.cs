﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using chess.engine.Game;

namespace chess.webapi.Services
{
    public class PerfService : IPerfService
    {
        public PerfResult PlayWikiGame(int iterations)
        {
            var times = new List<TimeSpan>();
            for (int i = 0; i < iterations; i++)
            {
                var game = ChessFactory.NewChessGame(ChessFactory.LoggerType.Null);
                var sw = Stopwatch.StartNew();
                foreach (var move in WikiGameMoves)
                {
                    var msg = game.Move(move);
                    if (!string.IsNullOrEmpty(msg))
                    {
                        if (msg.Contains("Error:"))
                            return new PerfResult(msg);
                    }
                }
                times.Add(sw.Elapsed);
            }

            return new PerfResult(times);

        }

        public static readonly string[] WikiGameMoves =
        {
            "e4",
            "e5",
            "Nf3",
            "Nc6",
            "Bb5",
            "a6",
            "Ba4",
            "Nf6",
            "O-O",
            "Be7",
            "Re1",
            "b5",
            "Bb3",
            "d6",
            "c3",
            "O-O",
            "h3",
            "Nb8",
            "d4",
            "Nbd7",
            "c4",
            "c6",
            "cxb5",
            "axb5",
            "Nc3",
            "Bb7",
            "Bg5",
            "b4",
            "Nb1",
            "h6",
            "Bh4",
            "c5",
            "dxe5",
            "Nxe4",
            "Bxe7",
            "Qxe7",
            "exd6",
            "Qf6",
            "Nbd2",
            "Nxd6",
            "Nc4",
            "Nxc4",
            "Bxc4",
            "Nb6",
            "Ne5",
            "Rae8",
            "Bxf7+",
            "Rxf7",
            "Nxf7",
            "Rxe1+",
            "Qxe1",
            "Kxf7",
            "Qe3",
            "Qg5",
            "Qxg5",
            "hxg5",
            "b3",
            "Ke6",
            "a3",
            "Kd6",
            "axb4",
            "cxb4",
            "Ra5",
            "Nd5",
            "f3",
            "Bc8",
            "Kf2",
            "Bf5",
            "Ra7",
            "g6",
            "Ra6+",
            "Kc5",
            "Ke1",
            "Nf4",
            "g3",
            "Nxh3",
            "Kd2",
            "Kb5",
            "Rd6",
            "Kc5",
            "Ra6",
            "Nf2",
            "g4",
            "Bd3",
            "Re6"
        };

    }
}