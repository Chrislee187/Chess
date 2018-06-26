import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { WikiPgn } from '../../models/sample.pgn'
import { MovelistComponent } from './movelist.component';

describe('MovelistComponent', () => {
  let component: MovelistComponent;
  let fixture: ComponentFixture<MovelistComponent>;
  let moveList: any[] = [];
  

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MovelistComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MovelistComponent);
    component = fixture.componentInstance;
    component.moves = new WikiPgn().moves;
    component.onNextMove.subscribe(m => {
      moveList.push(m);
    });
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should emit first move of wiki game from nextMove', () => {
    component.nextMove();

    expect(moveList.length).toEqual(1);
    expect(moveList[0].toString()).toEqual("E2-E4");
  });


});
