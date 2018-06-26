import { Component, OnInit, Input, Output, EventEmitter, ElementRef, ViewChild } from '@angular/core';
import { PgnJsonMove } from "../../models/pgn";

@Component({
  selector: 'app-movelist',
  templateUrl: './movelist.component.html',
  styleUrls: ['./movelist.component.scss']
})
export class MovelistComponent implements OnInit {
  constructor() { }

  @Input() moves : PgnJsonMove[];
  @Output() onNextMove: EventEmitter<PgnJsonMove> = new EventEmitter();
  @Output() onResetBoard: EventEmitter<boolean> = new EventEmitter();

  @Output() currentMoveIndex: number = 0;

  ngOnInit() {
    // this.moves = this.game.moves;
  }

  public gotoMove(index: number) : void {
    // this.currentMoveIndex = index;
    // let move = this.moves[index];

    // // TODO: Highlight currnet move;
    // this.makeMove.emit(move);
  }

  public nextMove() : void {
    let move = this.moves[this.currentMoveIndex++];

    this.scrollMoveIntoView(this.currentMoveIndex-1)

    this.onNextMove.emit(move);
  }
  public resetBoard() : void {
    this.onResetBoard.emit(true);
    this.currentMoveIndex = 0;
    this.scrollMoveIntoView(0);
  }

  // Would have prefered not to use dom elements directly here, probably missing some NG trick.
  @ViewChild('movelistTable') tableElement:ElementRef;
  private scrollMoveIntoView(rowIndex: number) : void {
    this.tableElement.nativeElement
      .querySelector(`tr[data-index='${rowIndex}']`)
      .scrollIntoView(false);
  }
}


