import { TestBed, inject } from '@angular/core/testing';

import { Services\ChessBoardService } from './services\chess-board.service';

describe('Services\ChessBoardService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [Services\ChessBoardService]
    });
  });

  it('should be created', inject([Services\ChessBoardService], (service: Services\ChessBoardService) => {
    expect(service).toBeTruthy();
  }));
});
