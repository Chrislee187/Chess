# Why does this project exist?

* Because I love coding and even though I do it for a living (I'm an agile, full stack consultant developer by trade) I still like to do it for fun.
* Even though I enjoy my work, developing web-api's/UI's to access a database can get a little repetitive. So I wanted something that was a little different than your typical "business" application.
* I like to have "weekend" projects that are a bit more substantial than your average kata or coding challenge.
* As an online "portfolio" of some of my skills and abilities.

So, to these ends, and about five'ish years ago, I started developing a "chess engine". This "engine" would understand the rules of chess enough to be able to enforce them on a chess board and allow simple access to the pieces and a list of there available moves, this would further be used to develop "AI" chess players (by analysing the board and move lists to determine "threats") that the engine could play off against each other (at the time I was also interested in multi-threading optimisations, and having lots of chess games being played or analysed seemd like a interesting "workload" to parallelise).

There were also other less concrete ideas that got to various stages of development in my previous iterations that will be ported/reimplented in due course to this version, there included;

* [PGN support](https://en.wikipedia.org/wiki/Portable_Game_Notation) this will allow me to validate the engine against a large number of games as much as anything else and would also include supporting [Algebraic notation](https://en.wikipedia.org/wiki/Algebraic_notation_(chess)) for move entry. NB. This is non-trivial as algebraic notation is NOT explicit with regard to the "from" location of chess moves and requires the current state of the chess board to work out what exact move a notation refers to. i.e. `Nf3` effectively means "move the Knight that CAN move to f3, to f3", this Knight could be in any one of eight locations!
* PGN Player applications - Simple player apps, in a variety of technologies (React, Angular, pure TS/JS, etc.)
* PGN Database applications - Complex DB with complex querying of moves, openings etc.

Skip ahead to now, and I'm on the fourth iteration, a complete ground-up rewrite in .NET core and I'm pretty happy with what I've got so far.

This code is to what I call "enterprise" standard, in that it largely uses the same practices, processes and techniques as I have and do use on real-world, team based, enterprise level projects. This includes fully tested/integrated and clean code as well as an automated CI/CD pipeline (I run TeamCity, amongst other things, on a homebrew NAS).

Other `.MD` files exist that go in to more technical detail on the code itself, please see the root [README.MD][link.RootReadme] for more details or you can dive straight in with the [Swagger UI][link.swaggerui] or my simple [handlebars template][link.chess.webapi.index.live] sitting on top of the web api.

[link.swaggerui]: https://chess-web-api.azurewebsites.net/swagger/index.html
[link.chess.webapi.index.live]: https://chess-web-api.azurewebsites.net/index.html

[link.RootReadme]: /src/readme.md
