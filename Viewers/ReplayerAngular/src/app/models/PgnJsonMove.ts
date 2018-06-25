import { PgnJsonLocation } from "./pgn";
export class PgnJsonMove {
    public from: PgnJsonLocation;
    public to: PgnJsonLocation;
    public pgn: string;
    public comment: string;
    public toString = (): string => {
        return `${this.from}-${this.to}`;
    };
}