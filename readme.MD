# TODOS

* Add tests for board actions
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
