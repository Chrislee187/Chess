import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { BoardSquareComponent } from './board-square.component';
import { EmptyBorderSquareComponent } from '../empty-border-square/empty-border-square.component';
import { ChangeDetectionStrategy } from '@angular/core';
import { ChessBoardService } from '../../services/chess-board.service';
import { MockChessBoardService } from "../../services/MockChessBoardService";

describe('BoardSquareComponent', () => {
  let component: BoardSquareComponent;
  let fixture: ComponentFixture<BoardSquareComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BoardSquareComponent, EmptyBorderSquareComponent ],
      providers: [
        { provide: ChessBoardService, useClass: MockChessBoardService}
      ]
    })
    .overrideComponent(BoardSquareComponent, {
      set: { changeDetection: ChangeDetectionStrategy.OnPush }
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BoardSquareComponent);
    component = fixture.componentInstance;
    component.boardKey = "1";
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should set tooltip for square with piece on', () => {
    setLocation("A",2, fixture);

    expect(component.titleContent).toEqual("White Pawn at A2");
  });

  it('should set tooltip for square without piece on', () => {
    setLocation("A",3, fixture);

    expect(component.titleContent).toEqual("A3");
  });

  function setLocation(rank: string, file: number, fixture: ComponentFixture<BoardSquareComponent> ) {
    component.rank = rank;
    component.file = file;
    fixture.detectChanges();
  }
  
});
