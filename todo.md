# TODOS
* proper error responses for invalid boards, moves etc. Just see a useless page on production boxes, drive out with the integration tests I still need to do.
* Integration test library for the webapi
* Split board.engine tests from chess.engine.tests
* Fix up team-city to use dotnet better not the existing msbuild/nunit etc. tasks
* RESTplayer doesn't show promotions properly and the moves don't work because of this
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
	* Enhance castline move validation to ensure king and castle haven't moved and king doesn't move through check
	* Stalemate detection
	* PGN output (optional)
* PGN Move support
	* Dependent on the PGNParser/PGN2JSON
	* Already have the list of moves to query now so just need the parsed text to match against

* Still need to seperate out NextPlayer logic from the Board

* Performance tests
	* Add some multithreading where approriate around the path regeneration mechanisms
* Invalid board state detection (should be able to be turned off) to allow custom boards without kings
* Undo/Redo support
	* Advanced Feature: Branched Undo/Redo

## Chess.WebAPI
* better error handling in ChessGame
* Sort out getting the default root `index.html` to work, having to go to /StaticFiles is pants
* Sort out where the deployment goes when using the Azure CI/CD that currently deploys but isn't where I expected!
* Document the deployment stuff, the two version (VS Publish/Azure CI)



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