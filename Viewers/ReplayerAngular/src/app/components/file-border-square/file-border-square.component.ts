import { Component, OnInit, Input, Output } from '@angular/core';

@Component({
  selector: 'file-border-square',
  templateUrl: './file-border-square.component.html',
  styleUrls: ['./file-border-square.component.scss']
})
export class FileBorderSquareComponent implements OnInit {

  constructor() { }
  
  @Input() file: string;

  @Output() title: string;
  
  ngOnInit() {
    this.title = `File ${this.file}`;
  }

}
