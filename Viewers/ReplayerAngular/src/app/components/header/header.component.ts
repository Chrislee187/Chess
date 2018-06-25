import { Component, OnInit, Input } from '@angular/core';
import { PgnJson } from "../../models/PgnJson";
@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {
  @Input() game: PgnJson;
  constructor() { }

  event: string = '?';
  site: string = '?';
  date: Date;
  white:string = '?';
  black:string = '?';
  round: Number;
  result:string = '?';

  ngOnInit() {
    this.event = this.game.event;
    this.site = this.game.site;
    this.date = this.game.date;
    this.white = this.game.white;
    this.black = this.game.black;
    this.round = this.game.round;
    this.result = this.game.result;
  }

}
