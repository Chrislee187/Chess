import { Component, OnInit, Input, Output } from '@angular/core';
import { Observable, of } from 'rxjs';
import { ChessBoard } from '../../models/ChessBoard'
import { ChessBoardService } from "../../services/chess-board.service";
@Component({
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

  constructor(private chessBoardService: ChessBoardService) { }

  ngOnInit() {
    // console.log(`Loc: ${this.rank}${this.file}`);
    let board = this.chessBoardService.get(this.boardKey);

    this.updateContent(ChessBoard.initialPieceAt(this.rank, this.file));
    this.setupSubscription(board);
  }

  setupSubscription(board: ChessBoard): void {

    this.chessPiece = board.pieceAt(this.rank, this.file);
    this.chessPiece.subscribe(piece => this.updateContent(piece));
  }
  updateContent(piece: string): void {
    this.pieceContent = piece;
    this.titleContent = ChessBoard.squareTooltip(this.rank, this.file, piece);
  }
}
