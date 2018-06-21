import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RankBorderSquareComponent } from './rank-border-square.component';

describe('RankBorderSquareComponent', () => {
  let component: RankBorderSquareComponent;
  let fixture: ComponentFixture<RankBorderSquareComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RankBorderSquareComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RankBorderSquareComponent);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  
  it('should set tooltip for rank border square', () => {
    setRank(4);
    expect(component.title).toEqual("Rank 4");
  });

  function setRank(rank: number) {
    component.rank = rank.toString();
    fixture.detectChanges();
  }
});
