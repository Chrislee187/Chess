import { ChessBoardService } from './chess-board.service';

// Simple override mock that generates a new board automatically for testing
export class MockChessBoardService extends ChessBoardService {
  constructor() {
    super();
    this.generateSubscriberBoard();
  }
}