import {IContact} from "./IContact.ts";

export interface ICounterpart {
    id: string,
    name: string
    contacts: IContact[]
}
