import { Component, OnInit, Input, Output, EventEmitter, ElementRef, ViewChild } from '@angular/core';
import { PgnJsonMove } from '../../models/pgn';

@Component({
  selector: 'app-movelist',
  templateUrl: './movelist.component.html',
  styleUrls: ['./movelist.component.scss']
})
export class MovelistComponent implements OnInit {
  constructor() { }
  // Would have prefered not to use dom elements directly here, probably missing some NG trick.
  @ViewChild('movelistTable') tableElement: ElementRef;

  @Input() moves: PgnJsonMove[];
  @Output() nextMoveEvent: EventEmitter<PgnJsonMove> = new EventEmitter();
  @Output() resetBoardEvent: EventEmitter<boolean> = new EventEmitter();

  @Output() currentMoveIndex = 0;

  ngOnInit() {
    // this.moves = this.game.moves;
  }

  public gotoMove(index: number): void {
    // this.currentMoveIndex = index;
    // let move = this.moves[index];

    // // TODO: Highlight currnet move;
    // this.makeMove.emit(move);
  }

  public nextMove(): void {
    const move = this.moves[this.currentMoveIndex++];

    this.scrollMoveIntoView(this.currentMoveIndex - 1);

    this.nextMoveEvent.emit(move);
  }
  public resetBoard(): void {
    this.resetBoardEvent.emit(true);
    this.currentMoveIndex = 0;
    this.scrollMoveIntoView(0);
  }

  private scrollMoveIntoView(rowIndex: number): void {
    // TODO: Move this raw dom functionality in to a service so we can mock and check it happens.
    this.tableElement.nativeElement
      .querySelector(`tr[data-index='${rowIndex}']`)
      .scrollIntoView(false);
  }
}


