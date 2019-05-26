# What type of database to use

Game event data consists typically of the following fields;

* Event, Site, Date, Round, White, Black, Result, MoveText 

Other custom tags can be present, they will initially be ignored, support may be added later.

This data is perfectly suited to a Relational DB and can see no benefit to using any other type (graph, object etc.)

So to that end, SQlite, available in .Net core, self-contained and free will be used.

I expect to use both, EF for code-first DB creation/management and write functionality and Dapper for read-access. The plan is to use the same "Entity" objects for both where possible, with additional entities created for Dapper to map to any any custom queries we create.


# "Framework" comparison
The two main contenders are Entity Framework and Dapper, I've used both extensively in the past and both have their strengths and weaknesses.

## Entity Framework
Not traditionaly been a fan of the "heavyweight" ORM's such as EF or NHibernate, been burned by them to many times in the past.

That said I've have used EF's code-first approach with some success (although there were nasty issues that occured merging migrations from some long lived feature branches but that's a different problem) and the ability to version control the DB along side the code is nice.

### PROS
* Code based implmentations for producing and executing your DB (i.e. TSQL) code
* Easily version controlled (with caveat around branches, see Cons below)
* Fully fledged ORM framework, so ultimately will be as simple or as complex as your requirements dictate.
* Can make integration level testing easier as it's easy to create and maintain control of a fully fledged test-database, however, good code design can minimise or even negate the need for a real database in testing.
* You don't have to know any SQL to handle the basics of creating a db, CRUD operations or simple relationships

### CONS
* Heavyweight abstraction, which although simple for the basics there are obviously complexities when dealing with complex relational databases, when things don't work or go wrong it can be a PIA to work out whats going on, even more frustrating if you understand exactly what SQL you need but can't quite get EF to produce the results you want. Not particularily EF's fault, but for someone with plenty of SQL experience it can be frustrating.
* Branches can be problematic when merging migrations, conflicts in .xxproj files a norm
* Generally considered to be slower than more lightwight/micro ORM solutions due to it's more heavyweight nature of modelling all the relationships.
* Well known to be slower than Dapper or raw ADO.NET
* You have to know how to use EF to generate the correct code/config to handle complex relationships

## Dapper
My goto for db access given a choice but that is typicallly in environments where the DB already exists and already has it's own process and pipelines in place for management of it's schema and content.

If you all need to do is write queries and get the results in to C#, Dapper is ideal.


### PROS
* Light weight solution, no specific mappings or setting up of relationships required
* Ideal if you just to want to write SQL and have the results map back to C# objects quickly
* Thin layer between C#/SQL generally considered to be faster than say EF for heavier workloads
* You can write your own SQL and don't have to understand an abstraction layer to generate results
* Complex relationships are handled in the database, we just map the flattened results to collections of objects.

### CONS
* No support for code-first DB management or the associated benefits of such (version control, etc.), a seperate solution would be needed for creating new and maintaining existing DB's.
* You have to know how to use TSQL to generate the correct code/config to handle complex relationships
