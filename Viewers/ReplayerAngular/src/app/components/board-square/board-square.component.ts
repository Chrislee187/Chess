import { Component, OnInit, Input, Output } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { ChessBoard } from '../../models/ChessBoard';
import { ChessBoardService } from '../../services/chess-board.service';
@Component({
  // tslint:disable-next-line:component-selector
  selector: 'board-square',
  templateUrl: './board-square.component.html',
  styleUrls: ['./board-square.component.scss']
})
export class BoardSquareComponent implements OnInit {

  @Input() boardKey: string;

  @Input() rank: string;
  @Input() file: number;


  @Output() pieceContent: string;
  @Output() titleContent: string;

  private chessPiece: Observable<string>;

  constructor(private chessBoardService: ChessBoardService) {
  }

  ngOnInit() {
    const board = this.chessBoardService.get(this.boardKey);

    this.updateContent(board.pieceAt(this.rank, this.file));
    this.setupSubscription(board);
  }

  setupSubscription(board: ChessBoard): void {
    this.chessPiece = board.observableAt(this.rank, this.file);
    this.chessPiece.subscribe(piece => this.updateContent(piece));
  }

  updateContent(piece: string): void {
    this.pieceContent = piece;
    this.titleContent = ChessBoard.squareTooltip(this.rank, this.file, piece);
  }

  public get isWhiteBackground(): boolean {
    return ChessBoard.isWhiteBackground(this.rank, this.file);
  }
}
