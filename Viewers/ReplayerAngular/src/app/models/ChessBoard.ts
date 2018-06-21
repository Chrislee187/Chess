import { Observable, Subject } from 'rxjs';
import { BoardComponent } from '../components/board/board.component';
export class ChessBoard {

    private board:string[] = [
        'rnbqkbnr',
        'pppppppp',
        '........',
        '........',
        '........',
        '........',
        'PPPPPPPP',
        'RNBQKBNR',
    ];

    private pieces: Subject<string>[][] =[
        [null, null, null, null, null, null, null, null],
        [null, null, null, null, null, null, null, null],
        [null, null, null, null, null, null, null, null],
        [null, null, null, null, null, null, null, null],
        [null, null, null, null, null, null, null, null],
        [null, null, null, null, null, null, null, null],
        [null, null, null, null, null, null, null, null],
        [null, null, null, null, null, null, null, null]
    ];

    constructor() {
        for (let rankIdx = 0; rankIdx < 8; rankIdx++) {
            for (let fileIdx = 0; fileIdx < 8; fileIdx++) {
                this.pieces[rankIdx][fileIdx] = new Subject<string>();
            }
        }
    }
    
    public physicalPieceAt(rank: string, file: number) : string {
        let r = this.rankCharToIndex(rank);
        let f = file - 1;
        let piece = this.board[7-f][r];

        return piece;
    }

    public pieceAt(rank: string, file: number): Observable<string> {
        let r = this.rankCharToIndex(rank);
        let f = file - 1;

        return this.pieces[r][f];
    }

    public move() : void {
        console.log("moving");
        this.pieces[0][1].next(' ');
        this.pieces[0][2].next('p');
        this.pieces[5][5].next('p');
    }

    public static squareTooltip(rank: string, file: number, piece: string): string {

        const location = `${rank}${file}`;

        if(ChessBoard.pieceNameText(piece) === '') {
            return location;
        }

        let colour = ChessBoard.pieceColourText(piece);

        return `${colour} ${this.pieceNameText(piece)} at ${location}`;
    }
    
    private rankCharToIndex(rank: string) : number {
        return rank.charCodeAt(0) - 65;
    }

    private static pieceColourText(piece: string) {
        let colour = "Black";
        if (ChessBoard.isWhitePiece(piece)) {
            colour = "White";
        }
        return colour;
    }

    private static isWhitePiece(piece: string) : boolean {
        let p = piece.charAt(0);

        return p.toLocaleUpperCase() === p;
    }

    private static pieceNames: { [key:string]: string} = {
        "p": "Pawn",
        "P": "Pawn",
        "r": "Rook",
        "R": "Rook",
        "n": "Knight",
        "N": "Knight",
        "b": "Bishop",
        "B": "Bishop",
        "q": "Queen",
        "Q": "Queen",
        "k": "King",
        "K": "King",
    };
    private static pieceNameText(piece: string) : string {
        return this.pieceNames[piece];
    }
}