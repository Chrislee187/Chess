# ReplayerAngular

This project was generated with [Angular CLI](https://github.com/angular/angular-cli) version 6.0.8.

A basic skeleton for a Chess game Replayer, developed purely as an exercise in Angular.

I already had some C# code to parse [PGN](https://en.wikipedia.org/wiki/Portable_Game_Notation) files in to long algebraic notation which I used to pregenerate the parsed sample game used by this example, 

NB. PGN format is a standard format used for years to record chess moves. Unfortunately this format doesn't typically use full algebraic notations (where both the source and destination square are noted) and instead uses an approach that describes the destination and the minium amount of information needed to work out the source square.

This is a problem I had already solved in C#, and for the purposes of this exercise I didn't want to spend time producing a PgnParser in typescript, I wanted to focus on the Angular aspects.

[Run example](https://htmlpreview.github.io/?https://github.com/Chrislee187/Chess/blob/master/Viewers/ReplayerAngular/dist/ReplayerAngular/index.html)


## Development server

Run `ng serve` for a dev server. Navigate to `http://localhost:4200/`. The app will automatically reload if you change any of the source files.

## Code scaffolding

Run `ng generate component component-name` to generate a new component. You can also use `ng generate directive|pipe|service|class|guard|interface|enum|module`.

## Build

Run `ng build` to build the project. The build artifacts will be stored in the `dist/` directory. Use the `--prod` flag for a production build.

## Running unit tests

Run `ng test` to execute the unit tests via [Karma](https://karma-runner.github.io).

## Running end-to-end tests

Run `ng e2e` to execute the end-to-end tests via [Protractor](http://www.protractortest.org/).

## Further help

To get more help on the Angular CLI use `ng help` or go check out the [Angular CLI README](https://github.com/angular/angular-cli/blob/master/README.md).
