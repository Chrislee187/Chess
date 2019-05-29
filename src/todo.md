# TODOS
* Basic web integration tests for chess-web-api endpoints
* Basic features tests chess-web
* Create .feature tests for both the chess.engine and the chess.webapi
  * chess engine feature level tests, Pawn_can_move_two_squares_at_start(), Pawn_cannot_move_two_squares_after_start()
  * How to handle these need to be easy to implement and setup the required states, want to be able to create the rules of chess in feature tests
* Add support to handle parsing of games inside zip files, most web resources are a zip file of a group of games.

* Game Database
  * Typical relational (SQL/SQLite) DB to store the PGN data
    * GUID ID's
    * Normalise the TagPair data
    * Event, Site, Player, MoveList tables
    * It's all basic data so should be simple enough
    * GOAL: Get the 450k+ games I downloaded stored and indexed in a DB
  * Import from
    * a filename (may contain multiple games)
    * a folder (or zip file), recursive all *.PGN files (default pattern)
      * nice to have: zip file support
    * a URL to a ZIP file
    * a URL to a PGN file
    * Console import tool for batch imports
    * Maybe a web page as well
  * Web App
    * Basic search/filter/display over the game database
    * Game selection and viewing
  * Console App
    * Basic search/filter/display over the game database
    * Game selection and viewing

* TODO's getting a bit long to keep managing in .MD files, really wanted to keep everything contained in the repo but my have to knock something up in Trello or similar to manage it better.

* **IN PROGRESS** approx 30000 games so far, 450k to go! - validate game engine against LOTS of PGN games
* proper error responses for invalid boards, moves etc. Just see a useless page on production boxes, drive out with the integration tests I still need to do.
* Fix up team-city to use dotnet better not the existing msbuild/nunit etc. tasks
* RESTplayer doesn't show promotions properly
* Proper feature flag mechanism NOT static flags
* Feature level test that plays a full game through the API
# Useful commands n stuff
Plays all games from a large PGN file through the engine, showing per game timings and average, NB PGN files can contain tens of thousands of games so this can still take quite some time.
Note: These tests are marked Explicit or Ignore to avoid slowing down development flow, you will need to comment the attributre out to run from the command line with the current NUNit runner at least, I can right click and run in the R# runner in VS
```
dotnet test --filter Should_play_all_games_in_a_single_file --configuration RELEASE
dotnet test --filter Measure_parse_game_time_100_games --configuration RELEASE

# powershell doesn't support direct stdin redirects so have to pipe it in
get-content D:\src\PGNArchive\pgn\Modern\Modern.pgn | dotnet run --project pgn2json -- >modern.pgn.j
son
```

## Performance
* seperate performance test assembly, appends results to a file that can be graphed (and asserted against?)
* Im pretty sure my algorithm for analysing the boards ends up generating paths for duplicate boards (when validating moves and generating paths for the validated moves)
  * Can we improve the apporach to not do this?
  * Can we cache results and reuse them if we are duplicating to avoid regenerating paths
* DONE  ~~did some simple analysis with R# profile, found some good improvements avg game parse time currently around 1.14ms~~ sub 1sec averager now
* DONE ~~Tried parallelise cloning of boardstate items, was slower~~

### Measurements
* Counters - get some counters and timers in the frequently used stuff

### Paralleise opportunities


* FAILED - Tried it, but it was slower, cloning wasn't super expensive so probably nothing to be gained by , did improve it a little by removing some ToString()'s that are only used for development conveinence ~~Board cloning~~
* DONE ~~Path refreshing~~
* DONE ~~Path validation~~

## Azure Deployment
* Couldn't get the CI/CD stuff working, Publish from VS works a treat though

## PGN Parsing
* Create a simple console app to convert PGN to a similar JSON format
* One tool or PGN console tool suite, 
  * pgn2json - basic tool created, only reads/write a single stream from stdin/out currently
  * pgnsort, 
  * pgnsearch

## SPIKER/Console Player
* Can I reuse anything from the old consoleplayer? Do I want to?
* Keep it simple, do not create the console window library you keep thinking about:)
	* Proper menu/command system , TBD
	* Save game ability

## Chess.Engine
* better error handling in ChessGame
* Move history
	* Enhance enpassant rule to ensure enemy pawn did it's double step the previous turn
	* Stalemate detection
	* DONE ~~SAN output~~
	* DONE ~~SAN input~~ all input goes through the SAN parser now
* Make characters used to display chess pieces (RKNBQP) configurable, other languages use different characters
* Invalid board state detection (should be able to be turned off) to allow custom boards without kings
* Undo/Redo support
	* Advanced Feature: Branched Undo/Redo

## Chess.WebAPI
* better error handling in ChessGame
* Sort out where the deployment goes when using the Azure CI/CD that currently deploys but isn't where I expected!
* Document the deployment stuff, the two version (VS Publish/Azure CI)
* Split index.html page out to a home page + navigation to others 
* Create a new MVC app, PGN replayer tool, React, Angular, others?
*

## Chess.DB
* Database of games
* Database of board states and available moves? 
  * For first x moves in game? At what point does retrieval become slower/faster than calculation
  * How quickly would such a DB grow?

# Misc other stuff
* Not done any WPF for a few years, so WPF client and others that use either the WebAPI of the assemblies directly to produce simply PGN replayer tool
  * Winforms
  * Blazor
  * Electron
  * Console


# CONSOLE STUFF SUPPORT

* Dynamic board and piece size
* Proper menu system
*	Debug options to dump moves/paths etc.
* Better error handling
* Screen layout
```
------------------------
|      |               |
| BOARD| MENU          |
|      |               |
------------------------
| prompt: input        |
------------------------
|                      |
|   ADDITIONAL         |
|     OUTPUT           |
|                      |
------------------------
```


# DONE
* DONE `/api/perf` endpoint ~~Setup a simple pref test using the Wiki PGN file that runs in the webapi and report avg time to play to wikigame. The single core 1.75Ghz (**dbl checki those fitures**) ENV should be more consistent (and slower) than my uber rig~~
* DONE ~~not required, Coord moves are valid SAN moves, Create command parser to detect between SAN move and Coord moves~~
* DONE ~~PGN file reader~~
* DONE ~~Split board.engine tests from chess.engine.tests~~
* DONE ~~Add SAN move support to ChessGame.Move()~~
* DONE ~~(BoardStateWrapper's)- NOTE: Refactored to use a ReadOnlyBoardState approach Was hoping to avoid it but looks like will need some sort of simple facade wrappers around the BoardState object for its interactions with validators and actions to allow better testing, they currently need a full create boardstate to test with.~~
* DONE chess.tests.utils ~~Create a chess.engine.tests.utils project contain for stuff like the builders~~
* DONE chess.engine.integration.tests ~~Create integration tests for the engine, ie. for tests that still use a full board state)~~
* DONE chess-web.azurewebsites.net ~~Create 'Chris's Sandbox' site and starting reference the chess stuff from there~~
  * ~~Reference PGN Convert uses Razor Pages (as opposed to Razor Views)~~
* ~~DONE~~ Tests around parsing of PGN comments, there are some issues
