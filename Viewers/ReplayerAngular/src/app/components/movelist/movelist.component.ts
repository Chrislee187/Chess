import { Component, OnInit, Input } from '@angular/core';
import { PgnJson, PgnJsonMove } from "../../models/pgn";

@Component({
  selector: 'app-movelist',
  templateUrl: './movelist.component.html',
  styleUrls: ['./movelist.component.scss']
})
export class MovelistComponent implements OnInit {
  @Input() game : PgnJson;

  constructor() { }

  public moves : PgnJsonMove[];


  ngOnInit() {
    this.moves = this.game.moves;
  }

  public gotoMove(index: number) : void {
    let move = this.moves[index];

    alert(`goto move ${move}`);
  }
}
