# Introduction
Component to create and manage a chess games database.

Although chess game move information probably isn't ideally suited to a relational DB approach, chess game event data is, so to this end will use the SQlite support available in .NET Core for our initial Chess Game database.

For most write based operations to the database we will use Entity Framework to leverage it's code-first approach and allow us to easily version control and manage updates to the database schema during development.

When we come to reading the data back we will use Dapper for better performance and easier development against SQL queries

This component will use EF code-first to create and update the DB as required, as well as offer services that can be used to interact with the chess game db.
