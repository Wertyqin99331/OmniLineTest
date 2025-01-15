import {ICounterpart} from "./ICounterpart.ts";

export interface IContact {
    id: string,
    email: string,
    fullName: string,
    counterpart: ICounterpart
}
