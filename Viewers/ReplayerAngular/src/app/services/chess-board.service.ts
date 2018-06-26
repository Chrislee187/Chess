import { Injectable } from '@angular/core';
import { ChessBoard } from '../models/ChessBoard';

@Injectable({
  providedIn: 'root'
})
export class ChessBoardService {

  constructor() {  }

  private matches: Map<string, ChessBoard> = new Map<string, ChessBoard>();
  private id = 0;

  public generateSubscriberBoard(): string {
    this.id++;

    this.matches.set(this.id.toString(), new ChessBoard());

    return this.id.toString();
  }

  public get(boardKey: string): ChessBoard {
    if (this.matches.has(boardKey)) {
      return this.matches.get(boardKey);
    }

    throw new Error(`Board with key '${boardKey}' not found`);
  }
}


