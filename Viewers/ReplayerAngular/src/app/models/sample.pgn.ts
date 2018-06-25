export class PgnJson {
    public event : string;
    public site: string;
    public date: Date;
    public round: Number;
    public white: string;
    public black: string;
    public result: string;

    public moves: PgnJsonMove[] = [];
}
export class PgnJsonMove {
    public from : PgnJsonLocation;
    public to: PgnJsonLocation;
    public pgn: string;
}
export class PgnJsonLocation {
    public rank: string;
    public file: number;
    public rankAsNumber: number;
}
export class ExamplePgnJson extends PgnJson {

    constructor() {
        super();
        this.event = this.game.Event;
        this.site = this.game.Site;
        this.date = new Date(this.game.Date);
        this.round = new Number(this.round);
        this.white = this.game.White;
        this.black = this.game.Black;
        this.result = this.game.Result;

        this.game.Moves.forEach(element => {
            let jsonMove = new PgnJsonMove();
            jsonMove.from = element.From;
            jsonMove.to = element.To;
            jsonMove.pgn = element.PgnText;

            this.moves.push(jsonMove);
        });
        

    }
    public game : any = {
        "Event": "F/S Return Match",
        "Site": "Belgrade, Serbia JUG",
        "Date": "1992.11.04",
        "Round": "29",
        "White": "Fischer, Robert J.",
        "Black": "Spassky, Boris V.",
        "Result": "Draw",
        "Moves": [
            {
            "From": "E2",
            "To": "E4",
            "PgnText": "e4"
            },
            {
            "From": "E7",
            "To": "E5",
            "PgnText": "e5"
            },
            {
            "From": "G1",
            "To": "F3",
            "PgnText": "Nf3"
            },
            {
            "From": "B8",
            "To": "C6",
            "PgnText": "Nc6"
            },
            {
            "From": "F1",
            "To": "B5",
            "PgnText": "Bb5"
            },
            {
            "From": "A7",
            "To": "A6",
            "PgnText": "a6"
            },
            {
            "From": "B5",
            "To": "A4",
            "PgnText": "Ba4"
            },
            {
            "From": "G8",
            "To": "F6",
            "PgnText": "Nf6"
            },
            {
            "From": "E1",
            "To": "G1",
            "PgnText": "O-O"
            },
            {
            "From": "F8",
            "To": "E7",
            "PgnText": "Be7"
            },
            {
            "From": "F1",
            "To": "E1",
            "PgnText": "Re1"
            },
            {
            "From": "B7",
            "To": "B5",
            "PgnText": "b5"
            },
            {
            "From": "A4",
            "To": "B3",
            "PgnText": "Bb3"
            },
            {
            "From": "D7",
            "To": "D6",
            "PgnText": "d6"
            },
            {
            "From": "C2",
            "To": "C3",
            "PgnText": "c3"
            },
            {
            "From": "E8",
            "To": "G8",
            "PgnText": "O-O"
            },
            {
            "From": "H2",
            "To": "H3",
            "PgnText": "h3"
            },
            {
            "From": "C6",
            "To": "B8",
            "PgnText": "Nb8"
            },
            {
            "From": "D2",
            "To": "D4",
            "PgnText": "d4"
            },
            {
            "From": "B8",
            "To": "D7",
            "PgnText": "Nbd7"
            },
            {
            "From": "C3",
            "To": "C4",
            "PgnText": "c4"
            },
            {
            "From": "C7",
            "To": "C6",
            "PgnText": "c6"
            },
            {
            "From": "C4",
            "To": "B5",
            "PgnText": "cxb5"
            },
            {
            "From": "A6",
            "To": "B5",
            "PgnText": "axb5"
            },
            {
            "From": "B1",
            "To": "C3",
            "PgnText": "Nc3"
            },
            {
            "From": "C8",
            "To": "B7",
            "PgnText": "Bb7"
            },
            {
            "From": "C1",
            "To": "G5",
            "PgnText": "Bg5"
            },
            {
            "From": "B5",
            "To": "B4",
            "PgnText": "b4"
            },
            {
            "From": "C3",
            "To": "B1",
            "PgnText": "Nb1"
            },
            {
            "From": "H7",
            "To": "H6",
            "PgnText": "h6"
            },
            {
            "From": "G5",
            "To": "H4",
            "PgnText": "Bh4"
            },
            {
            "From": "C6",
            "To": "C5",
            "PgnText": "c5"
            },
            {
            "From": "D4",
            "To": "E5",
            "PgnText": "dxe5"
            },
            {
            "From": "F6",
            "To": "E4",
            "PgnText": "Nxe4"
            },
            {
            "From": "H4",
            "To": "E7",
            "PgnText": "Bxe7"
            },
            {
            "From": "D8",
            "To": "E7",
            "PgnText": "Qxe7"
            },
            {
            "From": "E5",
            "To": "D6",
            "PgnText": "exd6"
            },
            {
            "From": "E7",
            "To": "F6",
            "PgnText": "Qf6"
            },
            {
            "From": "B1",
            "To": "D2",
            "PgnText": "Nbd2"
            },
            {
            "From": "E4",
            "To": "D6",
            "PgnText": "Nxd6"
            },
            {
            "From": "D2",
            "To": "C4",
            "PgnText": "Nc4"
            },
            {
            "From": "D6",
            "To": "C4",
            "PgnText": "Nxc4"
            },
            {
            "From": "B3",
            "To": "C4",
            "PgnText": "Bxc4"
            },
            {
            "From": "D7",
            "To": "B6",
            "PgnText": "Nb6"
            },
            {
            "From": "F3",
            "To": "E5",
            "PgnText": "Ne5"
            },
            {
            "From": "A8",
            "To": "E8",
            "PgnText": "Rae8"
            },
            {
            "From": "C4",
            "To": "F7",
            "PgnText": "Bxf7"
            },
            {
            "From": "F8",
            "To": "F7",
            "PgnText": "Rxf7"
            },
            {
            "From": "E5",
            "To": "F7",
            "PgnText": "Nxf7"
            },
            {
            "From": "E8",
            "To": "E1",
            "PgnText": "Rxe1"
            },
            {
            "From": "D1",
            "To": "E1",
            "PgnText": "Qxe1"
            },
            {
            "From": "G8",
            "To": "F7",
            "PgnText": "Kxf7"
            },
            {
            "From": "E1",
            "To": "E3",
            "PgnText": "Qe3"
            },
            {
            "From": "F6",
            "To": "G5",
            "PgnText": "Qg5"
            },
            {
            "From": "E3",
            "To": "G5",
            "PgnText": "Qxg5"
            },
            {
            "From": "H6",
            "To": "G5",
            "PgnText": "hxg5"
            },
            {
            "From": "B2",
            "To": "B3",
            "PgnText": "b3"
            },
            {
            "From": "F7",
            "To": "E6",
            "PgnText": "Ke6"
            },
            {
            "From": "A2",
            "To": "A3",
            "PgnText": "a3"
            },
            {
            "From": "E6",
            "To": "D6",
            "PgnText": "Kd6"
            },
            {
            "From": "A3",
            "To": "B4",
            "PgnText": "axb4"
            },
            {
            "From": "C5",
            "To": "B4",
            "PgnText": "cxb4"
            },
            {
            "From": "A1",
            "To": "A5",
            "PgnText": "Ra5"
            },
            {
            "From": "B6",
            "To": "D5",
            "PgnText": "Nd5"
            },
            {
            "From": "F2",
            "To": "F3",
            "PgnText": "f3"
            },
            {
            "From": "B7",
            "To": "C8",
            "PgnText": "Bc8"
            },
            {
            "From": "G1",
            "To": "F2",
            "PgnText": "Kf2"
            },
            {
            "From": "C8",
            "To": "F5",
            "PgnText": "Bf5"
            },
            {
            "From": "A5",
            "To": "A7",
            "PgnText": "Ra7"
            },
            {
            "From": "G7",
            "To": "G6",
            "PgnText": "g6"
            },
            {
            "From": "A7",
            "To": "A6",
            "PgnText": "Ra6"
            },
            {
            "From": "D6",
            "To": "C5",
            "PgnText": "Kc5"
            },
            {
            "From": "F2",
            "To": "E1",
            "PgnText": "Ke1"
            },
            {
            "From": "D5",
            "To": "F4",
            "PgnText": "Nf4"
            },
            {
            "From": "G2",
            "To": "G3",
            "PgnText": "g3"
            },
            {
            "From": "F4",
            "To": "H3",
            "PgnText": "Nxh3"
            },
            {
            "From": "E1",
            "To": "D2",
            "PgnText": "Kd2"
            },
            {
            "From": "C5",
            "To": "B5",
            "PgnText": "Kb5"
            },
            {
            "From": "A6",
            "To": "D6",
            "PgnText": "Rd6"
            },
            {
            "From": "B5",
            "To": "C5",
            "PgnText": "Kc5"
            },
            {
            "From": "D6",
            "To": "A6",
            "PgnText": "Ra6"
            },
            {
            "From": "H3",
            "To": "F2",
            "PgnText": "Nf2"
            },
            {
            "From": "G3",
            "To": "G4",
            "PgnText": "g4"
            },
            {
            "From": "F5",
            "To": "D3",
            "PgnText": "Bd3"
            },
            {
            "From": "A6",
            "To": "E6",
            "PgnText": "Re6"
            }
        ]
    };

}