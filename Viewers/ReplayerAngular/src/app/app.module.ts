import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppComponent } from './app.component';
import { HeaderComponent } from './components/header/header.component';
import { BoardComponent } from './components/board/board.component';
import { MovelistComponent } from './components/movelist/movelist.component';
import { NotesComponent } from './components/notes/notes.component';
import { EmptyBorderSquareComponent } from './components/empty-border-square/empty-border-square.component';
import { RankBorderSquareComponent } from './components/rank-border-square/rank-border-square.component';
import { FileBorderSquareComponent } from './components/file-border-square/file-border-square.component';
import { BoardSquareComponent } from './components/board-square/board-square.component';

@NgModule({
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
  imports: [
    BrowserModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
