import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { Observable, of } from 'rxjs';
import { BoardSquareComponent } from './board-square.component';
import { EmptyBorderSquareComponent } from '../empty-border-square/empty-border-square.component';
import { ChessBoardService } from '../../services/chess-board.service';
import { ChessBoard } from '../../models/ChessBoard';

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
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BoardSquareComponent);
    component = fixture.componentInstance;
    component.boardKey = "1";
    setLocation("A",1);
    fixture.detectChanges();
  });

  it('should create', () => {
    console.log('before test');
    expect(component).toBeTruthy();
  });

  it('should set tooltip for square with piece on', () => {
    setLocation("A",2);

    expect(component.titleContent).toEqual("White Pawn at A2");
  });

  it('should set tooltip for square without piece on', () => {
    setLocation("A",3);

    expect(component.titleContent).toEqual("A3");
  });

  function setLocation(rank: string, file: number) {
    component.rank = rank ;
    component.file = file;
    fixture.detectChanges();
    component.ngOnInit();
  }
});
class MockChessBoard extends ChessBoard {
  constructor(private alwaysReturnPiece: string ) {
    super();
  }
  public pieceAt(rank: string, file: number) {
    return of(this.alwaysReturnPiece);
  }
}
class MockChessBoardService extends ChessBoardService {

  constructor() {
    super();
    this.generateSubscriberBoard();
  }
}