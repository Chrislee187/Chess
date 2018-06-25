export class PgnJsonLocation {
    public rank: string;
    public file: number;
    public rankAsNumber: number;
    public toString = (): string => {
        return `${this.rank}${this.file}`;
    };
}