import { PgnJsonMove } from "./PgnJsonMove";
export class PgnJson {
    public event: string;
    public site: string;
    public date: Date;
    public round: Number;
    public white: string;
    public black: string;
    public result: string;
    public moves: PgnJsonMove[] = [];
    public toString = (): string => {
        return `${this.white} vs ${this.black} @ ${this.event} round ${this.round}`;
    };
}