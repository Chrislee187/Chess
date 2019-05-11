# Introduction
My "Chess Engine" has been through several iterations since I first started thinking about, this is my current version, and is complete ground up re-write in .NET Core and doesn't have all the features I had in previous versions (A web player app, simple AI players to play against, PGN support to name a fewe) want yet but currently does support the playing of valid game of chess for two humans, at that same keyboard, although with no decent UI yet so your stuck with my console test host [Spiker](link.Spiker] or the very quick and [simple UI][link.chess.webapi.index.live] that I knocked up.

# Overview
The "engine" current consists of the following components;

* The [board.engine][link.board.engine] which contains core mechanics required to manage pieces on a chess board.
* The [chess.engine][link.chess.engine] which contains the mechanics specific to the game of chess.
* The [chess.webapi][link.chess.webapi] a simple, REST like inteface which can be used to generate boards, move pieces, and indeed play a simple game of chess (against another human)
* [Spiker][link.Spiker] a simple console application which I use to quickly test out ideas that don't really work via spike tests, a proper console chess player application will be coming shortly.

# General Comments/Notes/Thoughts/Conclusions/Observations


* .NET Core is finally starting to get up-to-speed with XP approaches. I've lost count of how many times I've had to implement my owner logger and config implementations so I can test properly. `ILogger` and `IConfig` hopefully mean I won't ever had to write another one!
* .NET Core makes the TeamCity etc. setup much simpler, no more manually crafted `build.sh/.cmd/.ps1` for the most part
* I still can't bring myself to move out of Visual Studio with R# as my IDE of choice, I think I've just been using them to long now. One day maybe:)

[link.chess.webapi.live]: http:tbd
[link.chess.webapi.index.live]: http:tbd
[link.Spiker]: /src/spiker

[link.board.engine]: /src/board.engine
[link.chess.engine]: /src/chess.engine
[link.chess.webapi]: /src/chess.webapi


