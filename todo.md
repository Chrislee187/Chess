# TODOS
* Basic web integration test, ensure index.html exists and contains a chessboard and moves
* PGN file reader to validate game engine against LOTS of games
* Was hoping to avoid it but looks like will need Facade patterns around BoardState object acces to allow better testing, i.e. Check detection code is currently using full boards to test small units of logic.
* proper error responses for invalid boards, moves etc. Just see a useless page on production boxes, drive out with the integration tests I still need to do.
* Integration test library for the webapi
* Split board.engine tests from chess.engine.tests
* Fix up team-city to use dotnet better not the existing msbuild/nunit etc. tasks
* RESTplayer doesn't show promotions properly and the moves don't work because of this
* Add SAN move support to ChessGame.Move()
* PGN File Parser
* setup tests to parse LOTS of PGN files.
* Create command parser to detect between SAN move and Coord moves

## Azure Deployment
* Couldn't get the CI/CD stuff working, Publish from VS works a treat though


## PGN Parsing
* Implement PGN Parsing, base on old version
  * Create a simple console app to convert PGN to a similar JSON format
* Need to this to do more comprehensive testing against a larger database of games, think there are still some edge cases around castling that need resolving, need to get PGN parsing up and running to parse a ton (10000's) of games.

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
	* PGN output (optional)
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