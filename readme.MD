# TODOS
* Split board.engine tests from chess.engine.tests
* Proper README.MD for each project
* HIgh level readme.md in root
* 
* Publish WebAPI somewhere
* Put in github and hook up to my teamcity for automated build and deployment
* Design docs

* Need to this to do more comprehensive testing against a larger database of games, 

* Integration test library for the webapi
	* Not much to test in this as the controllers are VERY light but should check they return something

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

------------------------------------------------GLOBAL READ ME--------------------------------
# Introduction
I've always had largish personal projects of some sort on the go, I like to have larger more complex sandbox to play in than small kata's and nothing beats having a large more complex code-base to really thrash out design and implementation approachs.

Several several years ago I was playing some online chess while working away from home and started thinking (in C#) about how, if you had a `IEnumerable<ChessPiece>` and `ChessPiece` contained a `IEnumerable<Move>` you had almost everything you needed to produce an "engine" that understood the rules of chess, from which you could easily produce simple chess players for two humans to play chess on to validate and execute the moves.

The next step would be to use all the valid move information to produce "AI" players which could analyse the state of the board and produce a move to play.

My "Chess Engine" has been through several iterations since then, this is my current version, and is complete ground up re-write in .NET Core and doesn't have all the features I want yet but currently does support the playing (with two humans, at one keyboard) of valid game of chess (although at time of writing I haven't written a decent interface so your stuck with a noddy console app [Spiker](link.Spiker) or playing chess with cut-n-paste through the [web-api][link.chess.webapi.live].


# Comments/Notes/Thoughts
As long as your happy a

[link.chess.webapi.live]: http:xyz
[link.Spiker]: http:todo


