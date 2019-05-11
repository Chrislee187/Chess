import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BoardComponent } from './board.component';
import { EmptyBorderSquareComponent } from '../empty-border-square/empty-border-square.component';
import { RankBorderSquareComponent } from '../rank-border-square/rank-border-square.component';
import { FileBorderSquareComponent } from '../file-border-square/file-border-square.component';
import { BoardSquareComponent } from '../board-square/board-square.component';

import { ChessBoardService } from '../../services/chess-board.service';

import { MockChessBoardService } from "../../services/MockChessBoardService";

describe('BoardComponent', () => {
  let component: BoardComponent;
  let fixture: ComponentFixture<BoardComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ 
        BoardComponent,
        EmptyBorderSquareComponent, 
        RankBorderSquareComponent, 
        FileBorderSquareComponent, 
        BoardSquareComponent 
      ],
      providers: [
        { provide: ChessBoardService, useClass: MockChessBoardService}
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BoardComponent);
    component = fixture.componentInstance;
    component.boardKey = "1";
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
