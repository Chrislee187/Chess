import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FileBorderSquareComponent } from './file-border-square.component';

describe('FileBorderSquareComponent', () => {
  let component: FileBorderSquareComponent;
  let fixture: ComponentFixture<FileBorderSquareComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FileBorderSquareComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FileBorderSquareComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
