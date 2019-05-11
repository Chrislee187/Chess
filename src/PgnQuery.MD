# PgnQuery.exe
Command Line App (.Net Core?)

## Features
* Reads pgn files (standard input + wildcard support) that contain one or more pgn files
* Outputs to standard output (or command line arg supplied filename/patterns)
* Order by [tag pair key...]
* Filter by [tag pair key expressions], support generic and explicit apporach to defining tag paris keys
* Output file pattern, to create new or append to existing files (append to check for duplicates?)

## Command Line Options
    -infiles [pattern] overrides STDIN
    -outfiles [pattern] overrides  STDOUT

    -order-by tag-pair-key,tag-pair-key....

### Filters
    -player         pattern
    -whiteplayer    pattern
    -blackplayer    pattern
    -event          pattern
    -site           pattern
    -date           pattern
    -round          pattern
    -result         Unknown, Draw, WhiteWins, BlackWins



