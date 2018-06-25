import { TestBed, inject } from '@angular/core/testing';

import { ChessBoardService } from './chess-board.service';

describe('ChessBoardService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [ChessBoardService]
    });
  });

  it('should be created', inject([ChessBoardService], (service: ChessBoardService) => {
    expect(service).toBeTruthy();
  }));

  it('should generate unique key for each new board', inject([ChessBoardService], (service: ChessBoardService) => {
    let key1 = service.generateSubscriberBoard();
    let key2 = service.generateSubscriberBoard();
    expect(key1).not.toEqual(key2);
  }));

  it('should store board changes', inject([ChessBoardService], (service: ChessBoardService) => {
    let key1 = service.generateSubscriberBoard();
    let board = service.get(key1);
    let expectedPiece = '';
    board.observableAt("A",4).subscribe( p => expectedPiece = p);

    let board2 = service.get(key1);
    board2.testmove();

    expect(expectedPiece).toBe('P')
  }));

});
