import { ChessBoardService } from './chess-board.service';
export class MockChessBoardService extends ChessBoardService {
  constructor() {
    super();
    this.generateSubscriberBoard();
  }
}