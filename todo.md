# TODOS
* Setup a simple pref test using the Wiki PGN file that runs in the webapi and report avg time to play to wikigame. The single core 1.75Ghz (**dbl checki those fitures**) ENV should be more consistent (and slower) than my uber rig
* 
* Basic web integration test, ensure index.html exists and contains a chessboard and moves
* Create a chess.engine.tests.utils project contain for stuff like the builders. Will be need by the other high-level tests that are planned (.integration.tests for the API and .feature.tests for the web pages)
* Create feature and integration tests assemblies
* **IN PROGRESS** validate game engine against LOTS of PGN games
* proper error responses for invalid boards, moves etc. Just see a useless page on production boxes, drive out with the integration tests I still need to do.
* Integration test library for the webapi
* Fix up team-city to use dotnet better not the existing msbuild/nunit etc. tasks
* RESTplayer doesn't show promotions properly and the moves don't work because of this
* DONE ~~not required, Coord moves are valid SAN moves, Create command parser to detect between SAN move and Coord moves~~
* DONE ~~PGN file reader~~
* DONE ~~Split board.engine tests from chess.engine.tests~~
* DONE ~~Add SAN move support to ChessGame.Move()~~
* DONE ~~(BoardStateWrapper's)- NOTE: Refactored to use a ReadOnlyBoardState approach Was hoping to avoid it but looks like will need some sort of simple facade wrappers around the BoardState object for its interactions with validators and actions to allow better testing, they currently need a full create boardstate to test with.~~
* Proper feature flag mechanism NOT static flags
* Feature level test that plays a full game through the API
* Detailed feature level tests, Pawn_can_move_two_squares_at_start(), Pawn_cannot_move_two_squares_after_start()
  * How to handle these need to be easy to implement and setup the required states, want to be able to create the rules of chess in feature tests
* Engine is now successfully parsing many (1000's) games

# Useful commands n stuff
Plays all games from a large PGN file through the engine, showing per game timings and average, NB PGN files can contain tens of thousands of games so this can still take quite some time.
```
dotnet test --filter Should_play_all_games_in_a_single_file --configuration RELEASE
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
Parallellised generation of paths, combined with a new check detection algorithm avg game replay time down to around 0.3 seconds
* Board cloning
* DONE ~~Path refreshing~~
* DONE ~~Path validation~~

## Azure Deployment
* Couldn't get the CI/CD stuff working, Publish from VS works a treat though

## PGN Parsing
* Create a simple console app to convert PGN to a similar JSON format

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
	* ~~SAN output~~
* Make characters used to display chess pieces (RKNBQP) configurable, other languages use different characters
* Performance tests
	* Add some multithreading where approriate around the path regeneration mechanisms
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