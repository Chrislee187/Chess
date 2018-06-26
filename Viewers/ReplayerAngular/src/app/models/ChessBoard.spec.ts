import { ChessBoard } from './ChessBoard';

describe('ChessBoard', () => {

    let board: ChessBoard;

    beforeEach(() => {
        board = new ChessBoard();
    });

    it('should create', () => {
    expect(board).toBeTruthy();
    });

    it('squareTooltip - should generate correctly for empty square', () => {
        expect(ChessBoard.squareTooltip('A', 5, ' ')).toEqual('A5');
    });

    it('squareTooltip - should generate correctly for non-empty square', () => {
        expect(ChessBoard.squareTooltip('A', 5, 'P')).toEqual('White Pawn at A5');
    });

    it('rankCharToIndex - should return index value for every rank', () => {
        expect(ChessBoard.rankCharToIndex('A')).toEqual(0);
        expect(ChessBoard.rankCharToIndex('B')).toEqual(1);
        expect(ChessBoard.rankCharToIndex('C')).toEqual(2);
        expect(ChessBoard.rankCharToIndex('D')).toEqual(3);
        expect(ChessBoard.rankCharToIndex('E')).toEqual(4);
        expect(ChessBoard.rankCharToIndex('F')).toEqual(5);
        expect(ChessBoard.rankCharToIndex('G')).toEqual(6);
        expect(ChessBoard.rankCharToIndex('H')).toEqual(7);
    });

    it('pieceColourText - should return white for upper case pieces', () => {
        expect(ChessBoard.pieceColourText('P')).toEqual('White');
        expect(ChessBoard.pieceColourText('R')).toEqual('White');
        expect(ChessBoard.pieceColourText('N')).toEqual('White');
        expect(ChessBoard.pieceColourText('B')).toEqual('White');
        expect(ChessBoard.pieceColourText('Q')).toEqual('White');
        expect(ChessBoard.pieceColourText('K')).toEqual('White');
    });

    it('pieceColourText - should return black for lower case pieces', () => {
        expect(ChessBoard.pieceColourText('p')).toEqual('Black');
        expect(ChessBoard.pieceColourText('r')).toEqual('Black');
        expect(ChessBoard.pieceColourText('n')).toEqual('Black');
        expect(ChessBoard.pieceColourText('b')).toEqual('Black');
        expect(ChessBoard.pieceColourText('q')).toEqual('Black');
        expect(ChessBoard.pieceColourText('k')).toEqual('Black');
    });

    it('isWhitePiece - should return true for white piece', () => {
        expect(ChessBoard.isWhitePiece('P')).toBeTruthy();
    });

    it('isWhitePiece - should return false for black piece', () => {
        expect(ChessBoard.isWhitePiece('p')).toBeFalsy();
    });

    it('isWhiteBackground - should return true for "even" squares', () => {
        expect(ChessBoard.isWhiteBackground('A', 1)).toBeTruthy();
    });

    it('isWhiteBackground - should return false for "odd" squares', () => {
        expect(ChessBoard.isWhiteBackground('A', 2)).toBeFalsy();
    });

    it('resetBoard - updates current board state and sends next value to subjects', () => {
        let sourceReset: boolean;
        let destReset: boolean;

        board.observableAt('A', 2).subscribe(piece => sourceReset = piece === 'P');
        board.observableAt('A', 4).subscribe(piece => destReset = piece === ' ');
        board.move('A2', 'A4');
        board.resetBoard();

        expect(sourceReset).toBeTruthy();
        expect(destReset).toBeTruthy();
    });

    it('pieceAt - returns correctly for Queen positions', () => {
        expect(board.pieceAt('D', 1)).toEqual('Q');
        expect(board.pieceAt('D', 8)).toEqual('q');
    });

    it('observableAt - returns correctly for Queen positions', () => {
        expect(board.observableAt('D', 1)).toBeTruthy();
        expect(board.observableAt('D', 8)).toBeTruthy();
    });

    it('move - correctly updates from and to locations', () => {
        let pieceAtA2: string;
        let pieceAtA4: string;

        board.observableAt('A', 2).subscribe(piece => pieceAtA2 = piece);
        board.observableAt('A', 4).subscribe(piece => pieceAtA4 = piece);
        board.move('A2', 'A4');

        expect(pieceAtA2).toEqual(' ');
        expect(pieceAtA4).toBeTruthy('P');
    });

    it('move - correctly updates for white castling moves', () => {
        // TODO: Need to set up a custom board to test this;
        expect(true).toBeFalsy();
    });

    it('move - correctly updates for black castling moves', () => {
        // TODO: Need to set up a custom board to test this;
        expect(true).toBeFalsy();
    });
});
