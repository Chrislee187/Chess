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
  @Output() moveIndexChanged: EventEmitter<PgnJsonMove> = new EventEmitter();


  ngOnInit() {
    // this.moves = this.game.moves;
  }

  public gotoMove(index: number) : void {
    let move = this.moves[index];

    // TODO: Highlight currnet move;
    this.moveIndexChanged.emit(move);
  }
}
