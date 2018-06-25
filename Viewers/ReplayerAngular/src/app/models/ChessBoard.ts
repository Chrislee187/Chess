import { Observable, Subject } from 'rxjs';
export class ChessBoard {


    private pieces: string[][] =[
        ['r','n','b','q','k','b','n','r'],
        ['p','p','p','p','p','p','p','p'],
        ['.','.','.','.','.','.','.','.'],
        ['.','.','.','.','.','.','.','.'],
        ['.','.','.','.','.','.','.','.'],
        ['.','.','.','.','.','.','.','.'],
        ['P','P','P','P','P','P','P','P'],
        ['R','N','B','Q','K','B','N','R']
    ];

    public dumpBoard(): void {
        for (let rankIdx = 0; rankIdx < 8; rankIdx++) {
            let row = "";
            for (let fileIdx = 0; fileIdx < 8; fileIdx++) {
                let p = this.pieces[rankIdx][fileIdx];
                row = row + p;
            }
            console.log(row);
        }
    }

    private subjects: Subject<string>[][] =[
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
                var subject = this.createObservable(rankIdx, fileIdx);

                this.subjects[fileIdx][rankIdx] = subject;
            }
        }
    }
    
    private createObservable(rank: number, file: number) : Subject<string> {
        var subject = new Subject<string>();
        subject.subscribe(piece => {
            this.pieces[file][rank] = piece;
            // console.log(`square at ${rank}${file} updated to '${piece}'`);
        });

        return subject;
    }

    public pieceAt(rank: string, file: number) : string {
        let r = this.rankCharToIndex(rank);
        let f = 8-file;
        return this.pieces[f][r];
    }

    public observableAt(rank: string, file: number): Observable<string> {
        let r = this.rankCharToIndex(rank);
        let f = 8-file;

        return this.subjects[f][r];
    }

    private subjectAt(rank: string, file: number): Subject<string> {
        let r = this.rankCharToIndex(rank);
        let f = 8-file;

        return this.subjects[f][r];
    }

    public move(from: string, to: string) {
        let fr = from.charAt(0);
        let ff = Number(from.charAt(1))
        let tr = to.charAt(0);
        let tf = Number(to.charAt(1));
        this.innerMove(fr,ff,tr,tf);
    }

    public innerMove(fromRankChar: string, fromFile: number, toRankChar: string, toFile: number): void {
        let fromPiece = this.pieceAt(fromRankChar, fromFile);
        // console.log(`PieceAt(${fromRankChar}${fromFile}) = ${fromPiece}`);
        // let toPiece = this.pieceAt(toRankChar, toFile);
        // console.log(`PieceAt(${toRankChar}${toFile}) = ${toPiece}`);

        let fromSubject = this.subjectAt(fromRankChar, fromFile);
        let toSubject = this.subjectAt(toRankChar, toFile);

        fromSubject.next('.');
        toSubject.next(fromPiece);
    }

    public testmove() : void {
        // NOTE: This method only exists for test purposes until proper moving is implemented;
        this.move("A2","A4");
        this.move("D7","D5");
        // this.dumpBoard();
    }

    public squareTooltip(rank: string, file: number, piece: string): string {

        const location = `${rank}${file}`;

        if(this.pieceNameText(piece) === '') {
            return location;
        }

        let colour = this.pieceColourText(piece);

        return `${colour} ${this.pieceNameText(piece)} at ${location}`;
    }
    
    private rankCharToIndex(rank: string) : number {
        return rank.charCodeAt(0) - 65;
    }

    private pieceColourText(piece: string) {
        let colour = "Black";
        if (this.isWhitePiece(piece)) {
            colour = "White";
        }
        return colour;
    }

    private isWhitePiece(piece: string) : boolean {
        let p = piece.charAt(0);

        return p.toLocaleUpperCase() === p;
    }

    private pieceNames: { [key:string]: string} = {
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
        ".": ""
    };
    private pieceNameText(piece: string) : string {
        return this.pieceNames[piece];
    }
}