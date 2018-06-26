import { Component, Output } from '@angular/core';
import { WikiPgn } from './models/sample.pgn'
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
  @Output() boardKey: string;

  title = 'PGN Replay (Angular)';

  chessBoard: ChessBoard;

  constructor(private chessBoardService: ChessBoardService) {
    this.game = new WikiPgn();
  }

  ngOnInit() {
    this.boardKey = this.chessBoardService.generateSubscriberBoard();
    this.chessBoard = this.chessBoardService.get(this.boardKey);
  }

  makeMove(move: PgnJsonMove) {
    this.chessBoard.move(move.from.toString(), move.to.toString());
  }

  resetBoard() {
    this.chessBoard.resetBoard(true);
  }
}
