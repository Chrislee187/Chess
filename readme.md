# Chess Index

Welcome the index page for my Chess Project.

This project is designed to be a "sand-box" for playing around with new ideas and technologies on something larger and more complex than the examples typically found on the sites accompanying these ideas and techs.

To this end the project doesn't necessarily reflect the best implementation of chess but is meant to represent approaches used more typically in enterprise development (rather than game development). Chess is the "business" here.

## Components

The project is designed for the engine itself to be easily reusable by multiple clients, be they front end UI's with a (admittedly very basic) chessboard such as the Blazor, Angular and React clients or less visual tooling such as the webapi host for the engine (that the UI clients use).

### [chess.engine](https://github.com/Chrislee187/chess.engine)

Core chess engine in C#.

### [chessdb](https://github.com/Chrislee187/chess.db)

A database to manage chess games, using NET Core 3.0 and EF Core to create a mature RESTful API for managing the chess game database.

### [chess.webapi](https://github.com/Chrislee187/chess.webapi)

Exposes the chess engine through a simple REST-like JSON WebAPI using Core MVC WebAPI.

The Swagger UI for it can be seen at [https://chess-web-api.azurewebsites.net/swagger/index.html](https://chess-web-api.azurewebsites.net/swagger/index.html) (may take to a few moments to fire up, it's not exactly a popular site!).

### [chess.blazor](https://github.com/Chrislee187/chess.blazor)

A client-side blazor SPA that runs the native C# chess engine in the browser using Mono and WebAssembly.

A version of this can be found at [https://chessstaticstorage.z33.web.core.windows.net](https://chessstaticstorage.z33.web.core.windows.net/)

### [chess.react](https://github.com/Chrislee187/chess.reactredux)

A simple react client (typescript) created for comparison of "SPA" approaches. Uses the chess engine via the WebAPI.

### [chess.spa.feature.tests](https://github.com/Chrislee187/chess.spa.feature.tests)

A suite of [Specflow](https://specflow.org/)/[Selenium WebDriver](https://www.seleniumhq.org/)/[Shouldly](http://docs.shouldly-lib.net/v2.4.0/docs) BDD style tests that ensure the UX behaves as expected and then using these expected behaviors and custom board setups determines that the rules of chess are correctly implemented.

### [pgn-tools](https://github.com/Chrislee187/pgn-tools)

Tools to manipulate PGN [Portable Game Notation](https://en.wikipedia.org/wiki/Portable_Game_Notation) files.

### [chess.spikes](https://github.com/Chrislee187/chess.spikes)

Catch-all project for assorted other chess-related things im currently experimenting with.

### [ReplayerAngular](https://github.com/Chrislee187/ReplayerAngular)

A simple angular client, based on a previous and incomplete version of the engine and my thinking, needs to be updated to be brought in line with the other `chess.xxxx` clients.
