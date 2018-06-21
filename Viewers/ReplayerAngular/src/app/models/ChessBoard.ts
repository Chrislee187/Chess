import { Observable, Subject } from 'rxjs';
import { BoardComponent } from '../components/board/board.component';
export class ChessBoard {

    public board:string[] = [
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
        // [new Subject<string>(), new Subject<string>(), new Subject<string>(), new Subject<string>(), new Subject<string>(), new Subject<string>(), new Subject<string>(), new Subject<string>()],
        // [new Subject<string>(), new Subject<string>(), new Subject<string>(), new Subject<string>(), new Subject<string>(), new Subject<string>(), new Subject<string>(), new Subject<string>()],
        // [new Subject<string>(), new Subject<string>(), new Subject<string>(), new Subject<string>(), new Subject<string>(), new Subject<string>(), new Subject<string>(), new Subject<string>()],
        // [new Subject<string>(), new Subject<string>(), new Subject<string>(), new Subject<string>(), new Subject<string>(), new Subject<string>(), new Subject<string>(), new Subject<string>()],
        // [new Subject<string>(), new Subject<string>(), new Subject<string>(), new Subject<string>(), new Subject<string>(), new Subject<string>(), new Subject<string>(), new Subject<string>()],
        // [new Subject<string>(), new Subject<string>(), new Subject<string>(), new Subject<string>(), new Subject<string>(), new Subject<string>(), new Subject<string>(), new Subject<string>()],
        // [new Subject<string>(), new Subject<string>(), new Subject<string>(), new Subject<string>(), new Subject<string>(), new Subject<string>(), new Subject<string>(), new Subject<string>()],
        // [new Subject<string>(), new Subject<string>(), new Subject<string>(), new Subject<string>(), new Subject<string>(), new Subject<string>(), new Subject<string>(), new Subject<string>()]
    ];

    constructor() {
        for (let rankIdx = 0; rankIdx < 8; rankIdx++) {
            for (let fileIdx = 0; fileIdx < 8; fileIdx++) {
                this.pieces[rankIdx][fileIdx] = new Subject<string>();
            }
        }
    }

    public initNewBoard() : void {
        for (let rankIdx = 0; rankIdx < 8; rankIdx++) {
            for (let fileIdx = 0; fileIdx < 8; fileIdx++) {
                let piece =this.board[rankIdx].substr(fileIdx,1);
                // console.log(`Init ${piece} at ${rankIdx}${fileIdx}`);
                this.pieces[fileIdx][rankIdx].next(piece);
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
        
        var piece = this.pieces[r][f];

        if(piece === null) {
            piece = new Subject<string>();
            this.pieces[8-r][f] = piece;
        }
        return piece;
    }

    private rankCharToIndex(rank: string) : number {
        return rank.charCodeAt(0) - 65;
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
    
    public static pieceColourText(piece: string) {
        let colour = "Black";
        if (ChessBoard.isWhitePiece(piece)) {
            colour = "White";
        }
        return colour;
    }

    public static isWhitePiece(piece: string) : boolean {
    let p = piece.charAt(0);

    return p.toLocaleUpperCase() === p;
    }

    public static pieceNameText(piece: string) : string {
    switch(piece) {
        case 'P':
        case 'p':
        return 'Pawn';
        case 'R':
        case 'r':
        return 'Rook';
        case 'N':
        case 'n':
        return 'Knight';
        case 'B':
        case 'b':
        return 'Bishop';
        case 'Q':
        case 'q':
        return 'Queen';
        case 'K':
        case 'k':
        return 'King';
        default:
        return '';
    }
    }
}