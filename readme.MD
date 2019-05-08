# TODOS
* Proper README.MD

* Design docs

* Need to this to do more comprehensive testing against a larger database of games, 

## PGN2JSON Library
* Implement PGN Parsing, base on old version

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
* 

* Performance tests
	* Add some multithreading where approriate around the path regeneration mechanisms
* Invalid board state detection (should be able to be turned off) to allow custom boards without kings
* Undo/Redo support
	* Advanced Feature: Branched Undo/Redo

## Chess.WebAPI
* better error handling in ChessGame


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



# FONTS
* Using chars from default Arial font
	* ЌќЮюВвЯяЍйРр



