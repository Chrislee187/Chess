import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmptyBorderSquareComponent } from './empty-border-square.component';

describe('SquareEmptyBorderComponent', () => {
  let component: EmptyBorderSquareComponent;
  let fixture: ComponentFixture<EmptyBorderSquareComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmptyBorderSquareComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmptyBorderSquareComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
