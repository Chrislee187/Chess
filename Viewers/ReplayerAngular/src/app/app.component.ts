import { Component, Output } from '@angular/core';
import { ExamplePgnJson } from './models/sample.pgn'
import { PgnJson } from "./models/PgnJson";
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})

export class AppComponent {
  @Output() game : PgnJson;
  title = 'Replayer (Angular)';

  constructor() {
    this.game = new ExamplePgnJson();
  }
}
