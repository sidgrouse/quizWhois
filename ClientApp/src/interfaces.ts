export enum Answer {
    default,
    right,
    wrong
}
export interface IQuestion {
    id: number;
    text: string;
    key: number;
    cssClass: Answer;
}
