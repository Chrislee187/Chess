# TODOS
* Performamce tests becoming more important, game parsing is slow which is affecting my ability to run 400k games through the engine for test purposes
* Basic web integration test, ensure index.html exists and contains a chessboard and moves
* Create a chess.engine.tests.utils project contain for stuff like the builders. Will be need by the other high-level tests that are planned (.integration.tests for the API and .feature.tests for the web pages)
* Create feature and integration tests assemblies
* **IN PROGRESS** validate game engine against LOTS of PGN games
* proper error responses for invalid boards, moves etc. Just see a useless page on production boxes, drive out with the integration tests I still need to do.
* Integration test library for the webapi
* Fix up team-city to use dotnet better not the existing msbuild/nunit etc. tasks
* RESTplayer doesn't show promotions properly and the moves don't work because of this
* ~~not required, Coord moves are valid SAN moves, Create command parser to detect between SAN move and Coord moves~~
* ~~PGN file reader~~
* ~~Split board.engine tests from chess.engine.tests~~
* ~~Add SAN move support to ChessGame.Move()~~
* ~~Done (BoardStateWrapper's)- Was hoping to avoid it but looks like will need some sort of simple facade wrappers around the BoardState object for its interactions with validators and actions to allow better testing, they currently need a full create boardstate to test with.~~

## Performance
### Measurements
* Counters - get some counters and timers in the frequently used stuff

### Paralleise opportunities
* Path refreshing
* Board cloning
* Path validation

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

## Chess.DB
* Database of games
* Database of board states and available moves? 
  * For first x moves in game? At what point does retrieval become slower/faster than calculation
  * How quickly would such a DB grow?


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