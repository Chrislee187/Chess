import { TestBed, async } from '@angular/core/testing';
import { AppComponent } from './app.component';
import { HeaderComponent } from './components/header/header.component';
import { BoardComponent } from './components/board/board.component';
import { MovelistComponent } from './components/movelist/movelist.component';
import { NotesComponent } from './components/notes/notes.component';
import { EmptyBorderSquareComponent } from './components/empty-border-square/empty-border-square.component';
import { RankBorderSquareComponent } from './components/rank-border-square/rank-border-square.component';
import { FileBorderSquareComponent } from './components/file-border-square/file-border-square.component';
import { BoardSquareComponent } from './components/board-square/board-square.component';

describe('AppComponent', () => {
  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [
        AppComponent,
        HeaderComponent,
        BoardComponent,
        MovelistComponent,
        NotesComponent,
        EmptyBorderSquareComponent,
        RankBorderSquareComponent,
        FileBorderSquareComponent,
        BoardSquareComponent        
      ],
    }).compileComponents();
  }));

  it('should create the app', async(() => {
    const fixture = TestBed.createComponent(AppComponent);
    const app = fixture.debugElement.componentInstance;
    expect(app).toBeTruthy();
  }));

  const expectedTitle :string = 'PGN Replay (Angular)';

  it(`should have correct title`, async(() => {
    const fixture = TestBed.createComponent(AppComponent);
    const app = fixture.debugElement.componentInstance;
    expect(app.title).toEqual(expectedTitle);
  }));

  it('should render title in a h1 tag', async(() => {
    const fixture = TestBed.createComponent(AppComponent);
    fixture.detectChanges();
    const compiled = fixture.debugElement.nativeElement;
    expect(compiled.querySelector('h1').textContent).toContain(expectedTitle);
  }));
});
