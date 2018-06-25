import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { PgnJsonMove } from "../../models/pgn";

@Component({
  selector: 'app-movelist',
  templateUrl: './movelist.component.html',
  styleUrls: ['./movelist.component.scss']
})
export class MovelistComponent implements OnInit {
  constructor() { }

  @Input() moves : PgnJsonMove[];
  @Output() makeMove: EventEmitter<PgnJsonMove> = new EventEmitter();
  @Output() resetBoardEvent: EventEmitter<boolean> = new EventEmitter();

  private currentMoveIndex: number = 0;

  ngOnInit() {
    // this.moves = this.game.moves;
  }

  public gotoMove(index: number) : void {
    let move = this.moves[index];

    // TODO: Highlight currnet move;
    this.makeMove.emit(move);
  }

  public nextMove() : void {
    let move = this.moves[this.currentMoveIndex++];

    // TODO: Highlight currnet move;
    this.makeMove.emit(move);
  }
  public resetBoard() : void {
    this.resetBoardEvent.emit(true);
    this.currentMoveIndex = 0;
  }}
