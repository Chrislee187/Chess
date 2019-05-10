# Overview
A simple RESTlike web-api wrapping the core functionality of the [chess.engine][link.ChessEngine].

Swagger has been implemented using [NSwag][link.NSwag], a live example should be available at [link.Swagger.live][link.Swagger.live].

A simple wrapper around the webapi calls can be seen at [link.rest-wrapper.live][link.rest-wrapper.live] which uses [handlebars][link.handlebars] to render the JSON api results in to the simplest of boards and interactions for a game chess.

# Introduction
Uses standard .NET Core/AspNET/MVC approaches to expose a very simple web-api for interaction with the [chess.engine][link.ChessEngine]

# Notes/Comments/Observations
There are currently no tests around the Controller or the Service. They are pretty much acting a proxies directly to the [chess.engine][link.ChessEngine] with not logic of there own. This may of course change in the future if I add more complex API's.

# TODOS
* Flesh out this doc more
* Some basic integration tests on the API (basic requests work, invalid request return expected codes)





[link.BoardEngine]: http:todo
[link.ChessEngine]: http:todo
[link.Swagger.live]: link.Swagger.live
[link.rest-wrapper.live]: link.rest-wrapper.live
