import { PgnJsonLocation } from './pgn';
export class PgnJsonMove {
    public moveIndex: number;
    public from: PgnJsonLocation;
    public to: PgnJsonLocation;
    public pgn: string;
    public comment: string;
    public toString = (): string => {
        return `${this.from}-${this.to}`;
    }

    public get isWhitemove(): boolean {
        return this.moveIndex % 2 === 0;
    }
    public get isBlackmove(): boolean {
        return !this.isWhitemove;
    }

    public get moveNumber(): number {
        return Math.floor(this.moveIndex / 2 + 1);
    }

    public get colour(): string {
        return this.isWhitemove ? 'White' : 'Black';
    }
}
