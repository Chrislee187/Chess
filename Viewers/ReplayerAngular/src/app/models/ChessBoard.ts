import { Observable, Subject } from 'rxjs';
/*
    NOTES/TODO

    Unit tests

    This class is effectively coupled to the components by 'rxjs'.
     -  Abstract this away using some form of Provider pattern to supply our own
        Subject/Observable interfaces, that we can implement concrete versions of using 'rxjs'.
        Not sure if it's worth it, might make testing easier
     -  The components themselves can carry on using rxjs directly as that is correct approach for Angular
     -  Will make testing of the ChessBoard easier
*/
export class ChessBoard {
    private static pieceNames: { [key: string]: string} = {
        'p': 'Pawn',
        'P': 'Pawn',
        'r': 'Rook',
        'R': 'Rook',
        'n': 'Knight',
        'N': 'Knight',
        'b': 'Bishop',
        'B': 'Bishop',
        'q': 'Queen',
        'Q': 'Queen',
        'k': 'King',
        'K': 'King',
        '.': '',
        ' ': '',
    };

    private moveHistory: any[] = [];

    private currentBoardState: string[][] = [
        ['r', 'n', 'b', 'q', 'k', 'b', 'n', 'r'],
        ['p', 'p', 'p', 'p', 'p', 'p', 'p', 'p'],
        [' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '],
        [' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '],
        [' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '],
        [' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '],
        ['P', 'P', 'P', 'P', 'P', 'P', 'P', 'P'],
        ['R', 'N', 'B', 'Q', 'K', 'B', 'N', 'R']
    ];
    private readonly startingPieces: string[][] = [
        ['r', 'n', 'b', 'q', 'k', 'b', 'n', 'r'],
        ['p', 'p', 'p', 'p', 'p', 'p', 'p', 'p'],
        [' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '],
        [' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '],
        [' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '],
        [' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '],
        ['P', 'P', 'P', 'P', 'P', 'P', 'P', 'P'],
        ['R', 'N', 'B', 'Q', 'K', 'B', 'N', 'R']
    ];

    private subjects: Subject<string>[][] = [
        [null, null, null, null, null, null, null, null],
        [null, null, null, null, null, null, null, null],
        [null, null, null, null, null, null, null, null],
        [null, null, null, null, null, null, null, null],
        [null, null, null, null, null, null, null, null],
        [null, null, null, null, null, null, null, null],
        [null, null, null, null, null, null, null, null],
        [null, null, null, null, null, null, null, null]
    ];

    public static squareTooltip(rank: string, file: number, piece: string): string {

        const location = `${rank}${file}`;

        if (this.pieceNameText(piece) === '') {
            return location;
        }

        const colour = this.pieceColourText(piece);

        return `${colour} ${this.pieceNameText(piece)} at ${location}`;
    }

    public static rankCharToIndex(rank: string): number {
        return rank.charCodeAt(0) - 65;
    }

    public static pieceColourText(piece: string) {
        let colour = 'Black';
        if (this.isWhitePiece(piece)) {
            colour = 'White';
        }
        return colour;
    }

    public static isWhitePiece(piece: string): boolean {
        const p = piece.charAt(0);

        return p.toLocaleUpperCase() === p;
    }

    private static pieceNameText(piece: string): string {
        return this.pieceNames[piece];
    }

    public static isWhiteBackground(rank: string, file: number): boolean {
        const r = this.rankCharToIndex(rank) + 1;
        const sum = r + file;
        const isWhite = (sum % 2) === 0;
        return isWhite;
    }

    public dumpBoard(): void {
        for (let rankIdx = 0; rankIdx < 8; rankIdx++) {
            let row = '';
            for (let fileIdx = 0; fileIdx < 8; fileIdx++) {
                const p = this.currentBoardState[rankIdx][fileIdx];
                row = row + p;
            }
        }
    }

    public resetBoard(): void {
        for (let rankIdx = 0; rankIdx < 8; rankIdx++) {
            const row = '';
            for (let fileIdx = 0; fileIdx < 8; fileIdx++) {
                const p = this.startingPieces[rankIdx][fileIdx];
                this.currentBoardState[rankIdx][fileIdx] = p;
                const s = this.subjects[rankIdx][fileIdx];

                s.next(p);
            }
        }
    }

    constructor() {
        for (let rankIdx = 0; rankIdx < 8; rankIdx++) {
            for (let fileIdx = 0; fileIdx < 8; fileIdx++) {
                const subject = this.createBoardStateUpdateSubscription(rankIdx, fileIdx);

                this.subjects[fileIdx][rankIdx] = subject;
            }
        }
    }

    private createBoardStateUpdateSubscription(rank: number, file: number): Subject<string> {
        const subject = new Subject<string>();
        subject.subscribe(piece => {
            this.currentBoardState[file][rank] = piece;
            // console.log(`square at ${rank}${file} updated to '${piece}'`);
        });

        return subject;
    }

    public pieceAt(rank: string, file: number): string {
        const r = ChessBoard.rankCharToIndex(rank);
        const f = 8 - file;
        return this.currentBoardState[f][r];
    }

    public observableAt(rank: string, file: number): Observable<string> {
        const r = ChessBoard.rankCharToIndex(rank);
        const f = 8 - file;

        return this.subjects[f][r];
    }

    private subjectAt(rank: string, file: number): Subject<string> {
        return <Subject<string>>this.observableAt(rank, file);
    }


    public move(from: string, to: string) {
        const fr = from.charAt(0);
        const ff = Number(from.charAt(1));
        const tr = to.charAt(0);
        const tf = Number(to.charAt(1));

        const fromPiece = this.pieceAt(fr, ff);

        if (this.isCastlingMove(fromPiece, fr, tr)) {
            const f = () => this.performCastle(fr, ff, tr);
            this.moveHistory.push(f);
            f();
        } else {
            const f = () => this.performMove(fr, ff, tr, tf);
            this.moveHistory.push(f);
            f();
        }
    }

    private isCastlingMove(fromPiece: string, fromRankChar: string, toRankChar: string): boolean {
        if (fromPiece === 'k' || fromPiece === 'K') {
            // check for and handle castling
            const fromRank = ChessBoard.rankCharToIndex(fromRankChar);
            const toRank = ChessBoard.rankCharToIndex(toRankChar);
            const diff = Math.abs(fromRank - toRank);

            if (diff > 1) {
                // out work castling moves
                return true;
            }
        }
        return false;
    }
    private performCastle(fromRankChar: string, file: number, toRankChar: string): void {
        let castleFrom: string;
        let castleTo: string;
        if (toRankChar === 'G') {
            castleFrom = `H`;
            castleTo = `F`;
        } else {
            castleFrom = `A`;
            castleTo = `D`;
        }

        this.performMove(fromRankChar, file, toRankChar, file);
        this.performMove(castleFrom, file, castleTo, file);
    }

    private performMove(fromRankChar: string, fromFile: number, toRankChar: string, toFile: number): void {
        const fromPiece = this.pieceAt(fromRankChar, fromFile);

        const fromSubject = this.subjectAt(fromRankChar, fromFile);
        const toSubject = this.subjectAt(toRankChar, toFile);

        fromSubject.next(' ');
        toSubject.next(fromPiece);
    }

    public testmove(): void {
        // NOTE: This method only exists for test purposes until proper moving is implemented;
        this.move('A2', 'A4');
        this.move('D7', 'D5');
        // this.dumpBoard();
    }
}
