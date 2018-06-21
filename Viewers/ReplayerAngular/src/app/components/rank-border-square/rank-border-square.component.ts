import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'rank-border-square',
  templateUrl: './rank-border-square.component.html',
  styleUrls: ['./rank-border-square.component.scss']
})
export class RankBorderSquareComponent implements OnInit {

  constructor() { }

  @Input() rank: string;

  ngOnInit() {
  }

}
