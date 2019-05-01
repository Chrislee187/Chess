# TODOS

* Castling board actions
* Pawn promotion move generators 
	* move validator
	* board actions
* Check/CheckMate/Stalemate detection
* Move history
	* Enhance enpassant rule to ensure enemy pawn did it's double step the previous turn
	* Enhance castline move validation to ensure king and castle haven't moved and king doesn't move through check
	* Undo/Redo support
		* Advanced Feature: Branched Undo/Redo
* PGN Move support
* Performance Metrics
* ChessBoardEngine tests
* pgn2json to use new engine
* Invalid board state detection (should be able to be turned off) to allow custom boards without kings

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
