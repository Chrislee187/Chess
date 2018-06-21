import { ChessBoard } from "../../models/ChessBoard";

import { Component, OnInit  } from '@angular/core';
import { ChessBoardService } from "../../services/chess-board.service";
@Component({
  selector: 'app-board',
  templateUrl: './board.component.html',
  styleUrls: ['./board.component.scss']
})
export class BoardComponent implements OnInit {

  constructor(private chessBoardService: ChessBoardService) { }

  
  public boardKey: string;
  private chessBoard : ChessBoard;
  
  ngOnInit() {
    this.boardKey = this.chessBoardService.generateSubscriberBoard();
    this.chessBoard = this.chessBoardService.get(this.boardKey);
  }

  public testmove() : void {
    this.chessBoard.testmove();
  }
}


