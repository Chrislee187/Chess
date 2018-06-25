import { Component, Output } from '@angular/core';
import { ExamplePgnJson } from './models/sample.pgn'
import { PgnJson } from "./models/PgnJson";
import { PgnJsonMove } from './models/pgn';
import { ChessBoardService } from "./services/chess-board.service";
import { ChessBoard } from "./models/ChessBoard";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})

export class AppComponent {
  @Output() game : PgnJson;
  title = 'Replayer (Angular)';

  constructor(private chessBoardService: ChessBoardService) {
    this.game = new ExamplePgnJson();
  }

  onMoveIndexChanged(move: PgnJsonMove) {
    let key = "1";
    let board = this.chessBoardService.get(key);

    board.move(move.from.toString(), move.to.toString());
  }
}
