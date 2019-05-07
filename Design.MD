# Why does this project exist?

Around five years or so ago, I was looking at some new library or another, I forget what, when I was yet again disappointed with its documentation or lack of in-depth examples.

Too many times I had been bitten by the promises of the easy framework/library/ideas only to find myself debugging other peoples code to understand how something works, wiping out any initial gains we might have made from using the framework/library in the first place. 

What I really wanted was a sandbox project, unrelated to the actual new "thing" I wanted to play with. Something substantial and complex enough as to have real-world problems and issues associated with implementing new technologies, libraries, ideas etc. in. It also had to be something that was old understood as I didn't want to be defining the "business" as-such.

After a few false starts (a Dungeons and Dragons engine and various utilities to parse data from various games I play/played, mainly ETL type tools to extract statistical data drom strategy games), I settled on Chess! 

Firstly it's rules are very well defined and understood, there are enormous amounts of resources available at all levels for the game of chess.

Secondly it's relatively simple ruleset, creates enough complexity to produce real-world architectural, design and implentation issues

Finally I wanted also to investigate designs that allowed me to re-use core components in different ways, so I envisage a simple Console board that two players can play chess on, Web versions of the same thing (two local players, remote players eventually), chess board analysis/scoring (as part of a Chess playere "AI"). 

This code would be to what I call "enterprise" standard, in that it largely uses the same practices, processes and techniques as I would use on a real-world, team based, enterprise level projects. This included fully tested and integrated code, a CI/CD pipeline (run on personal hardware at home) and would be able to be easily re-used as a core "engine" for Chess and the Board it's played on.

To this end I'm aiming for a "V1" to contain;
	* A generic engine for handling games on a chess board (on any rectangular board)
	* An implementation supporting the ruleset for Chess for the above "board" engine
		* Further implementations (Checkers, Othello) may be forth coming in due course
	* A console "Player" application that two players (at the same keyboard) can play Chess on
	* A web based "Player" application that two players (at the same keyboard) can play Chess on
		* Possible multiple versions of this eg. Angular, React, pure TS, with as much re-use at all levels as possible
	* A RESTful web API that can be used to validate chess moves or get available moves against a specified board

Later versions may contains;
	* "Viewer" apps for replaying PGN games
		* As for the "Player" apps. multiple versions re-using core-logic as required
	* "Database" apps for managing large collections of PGN games
	* Integration in to other resources (not idea what yet, somethings must exist out there though)

# Design approach
My design approach was to abstract two core elements of the game "Chess".

1. The core elements of a "Board" or adding, moving and removing pieces from the board.
2. The ruleset of "Chess" or how pieces are allowed to be added, moved and removed from the board.