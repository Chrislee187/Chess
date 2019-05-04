# TODOS

 * Tests for Check/CheckMate/Stalemate detection
  Improve readability of Path and Path's usage, extension methods?
* Move history
	* Enhance enpassant rule to ensure enemy pawn did it's double step the previous turn
	* Enhance castline move validation to ensure king and castle haven't moved and king doesn't move through check
* Better input error handling, extract out to a proper system with tests
* Find a test game to play out (one from the angular app?) as a simple integration test.
* PGN Move support
* Performance Metrics
* ChessBoardEngine tests
* pgn2json to use new engine
* Invalid board state detection (should be able to be turned off) to allow custom boards without kings
* Undo/Redo support
	* Advanced Feature: Branched Undo/Redo

# Chess WebAPI
Swagger!

* Get a newboard in serialized form
GET /chess
returns serialized newboard

serializeedboard  = 64 chars (Ee for pawns that can be taken with enpassant)
					+ 4 optional flags for White/BlackKingCanCastleQueen/Kingside 

* Get available moves for board, all or colour specific
GET /chess/serializedboard
GET /chess/serializedboard/white
GET /chess/serializedboard/black


* Play a move, returns new board
PUT /chess/serializedboard/move


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
