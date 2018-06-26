import { Component, OnInit, Input, Output  } from '@angular/core';
import { Observable, Subject } from 'rxjs';

import { PgnJson } from "../../models/PgnJson";

@Component({
  selector: 'app-board',
  templateUrl: './board.component.html',
  styleUrls: ['./board.component.scss']
})
export class BoardComponent implements OnInit {
  @Input() boardKey: string;
  @Input() game : PgnJson;

  @Output() boardState : Subject<string>[][] =[
    [null, null, null, null, null, null, null, null],
    [null, null, null, null, null, null, null, null],
    [null, null, null, null, null, null, null, null],
    [null, null, null, null, null, null, null, null],
    [null, null, null, null, null, null, null, null],
    [null, null, null, null, null, null, null, null],
    [null, null, null, null, null, null, null, null],
    [null, null, null, null, null, null, null, null]
];


  constructor() { 
    for (let rankIdx = 0; rankIdx < 8; rankIdx++) {
      for (let fileIdx = 0; fileIdx < 8; fileIdx++) {
          var subject = new Subject<string>();
          this.boardState[fileIdx][rankIdx] = subject;
      }
    }    
  }

  ngOnInit() {
  }
}


