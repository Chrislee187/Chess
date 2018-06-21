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
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
