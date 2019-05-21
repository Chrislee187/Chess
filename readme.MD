# Introduction
My "Chess Engine" has been through several iterations since I first started thinking about, this is my current version, and is complete ground up re-write in .NET Core and doesn't have all the features I had in previous versions, A web player app, simple AI players to play against and PGN support to name a few, but currently does support the playing of valid game of chess for two humans, at that same keyboard, please note, there is no real UI yet so your stuck with the [Simple UI][link.chess.webapi.index.live] I overlayed over a couple of the API calls, the [Swagger UI][link.swaggerui] or my console test host [Spiker][link.Spiker] to excercise the code.

# Overview
The "engine" currently consists of the following components;

* The [board.engine][link.board.engine] which contains core mechanics required to manage pieces on a chess board.
* The [board.engine.tests][link.board.engine.tests] tests for core board mechanics.
* The [chess.engine][link.chess.engine] which contains the mechanics specific to the game of chess.
* The [chess.engine.tests][link.chess.engine.tests] tests for chess specific mechanics
* The [chess.webapi][link.chess.webapi] a simple, REST like inteface which can be used to generate boards, move pieces, and indeed play a simple game of chess (against another human)
* [Spiker][link.Spiker] a simple console application which I use to quickly test out ideas that don't really work via spike tests, a proper console chess player application will be coming shortly.

# General Comments/Notes/Thoughts/Conclusions/Observations


* .NET Core is finally starting to get up-to-speed with XP approaches. I've lost count of how many times I've had to develop my owner logger and config implementations so I can test properly. `ILogger` and `IConfig` hopefully mean I won't ever had to write another one!
* .NET Core makes the TeamCity etc. setup much simpler, no more manually crafted `build.sh/.cmd/.ps1` for the most part
* I still can't bring myself to move out of Visual Studio with R# as my IDE of choice, I think I've just been using them to long now. One day maybe:)
* Azure publish. One click, very nice:) 
* So... I've used and loved Generics for years, but it does create quite the rabbit hole sometimes.
* No Property Injection in Microsoft.Extensions.DependencyInjection, shame have to manually pass Loggers in everywhere unless make them always last, optional param type approach, more thought needed.
* Using R# Profiler found that `ToString()` (which I was using in `DebuggerDisplayAttributes`) can have some interesting performance implications, even when not really used outside of loggin etc., surrounded most of them with `#if DEBUG` for convience when debugging.
* Starting seeing sub one second avg game parse times, a mini-milestone, 0.5 seconds next!


[link.swaggerui]: https://chess-web-api.azurewebsites.net/swagger/index.html
[link.chess.webapi.index.live]: https://chess-web-api.azurewebsites.net
[link.Spiker]: /src/spiker

[link.board.engine]: /src/board.engine
[link.chess.engine]: /src/chess.engine
[link.chess.webapi]: /src/chess.webapi
[link.chess.engine.tests]: /src/chess.engine.tests
[link.board.engine.tests]: /src/board.engine.tests

