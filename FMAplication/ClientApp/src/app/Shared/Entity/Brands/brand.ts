import { Status } from "src/app/Shared/Enums/status";

export class Brand {
    public id: number;
    public name: string;
    public code: string;
    public details: string;
    public status: number;

    constructor() {
        this.id = 0;
        this.name = '';
        this.code = '';
        this.details = '';
        this.status = Status.Active;
    }
}