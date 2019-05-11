import { Component, OnInit, Input, Output } from '@angular/core';

@Component({
  selector: 'rank-border-square',
  templateUrl: './rank-border-square.component.html',
  styleUrls: ['./rank-border-square.component.scss']
})
export class RankBorderSquareComponent implements OnInit {

  constructor() { }

  @Input() rank: string;

  @Output() title: string;

  ngOnInit() {
    this.title = `Rank ${this.rank}`;
  }

}
