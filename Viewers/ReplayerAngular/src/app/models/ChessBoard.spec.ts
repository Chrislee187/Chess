import { ChessBoard } from './ChessBoard';
import { Subject } from 'rxjs';

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

    it('resetBoard - updates current board state and sends new value to subjects', () => {
        board.move('A2', 'A4');
        board.resetBoard();

        expect(board.pieceAt('A', 2)).toEqual('P');
        expect(board.pieceAt('A', 4)).toEqual(' ');
    });

    it('resetBoard - sends new value to subjects', () => {
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

    it('observableAt - returns correct object', () => {
        const d8 = board.observableAt('D', 8);
        const d8sub = <Subject<string>>d8;

        d8sub.next('x');

        expect(board.pieceAt('D', 8)).toEqual('x');
    });

    it('move - correctly updates from and to locations', () => {
        board.move('A2', 'A4');

        expect(board.pieceAt('A', 2)).toEqual(' ');
        expect(board.pieceAt('A', 4)).toEqual('P');
    });

    it('move - correctly updates for white castling moves', () => {
        // NOTE: This only works because the board does NO actual checking for valid moves, but detects a castling movee
        // on the king and moves the rook accordingly
        board.move('E1', 'G1');
        expect(board.pieceAt('G', 1)).toEqual('K');
        expect(board.pieceAt('F', 1)).toEqual('R');

        board.resetBoard();
        board.move('E1', 'C1');
        expect(board.pieceAt('C', 1)).toEqual('K');
        expect(board.pieceAt('D', 1)).toEqual('R');
    });

    it('move - correctly updates for black castling moves', () => {
        // NOTE: This only works because the board does NO actual checking for valid moves, but detects a castling movee
        // on the king and moves the rook accordingly
        board.move('E8', 'G8');
        expect(board.pieceAt('G', 8)).toEqual('k');
        expect(board.pieceAt('F', 8)).toEqual('r');

        board.resetBoard();
        board.move('E8', 'C8');
        expect(board.pieceAt('C', 8)).toEqual('k');
        expect(board.pieceAt('D', 8)).toEqual('r');    });
});
