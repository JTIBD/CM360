import { Status } from "src/app/Shared/Enums/status";
import { Brand } from './brand';

export class SubBrand {
    public id: number;
    public brandId: number;
    public name: string;
    public code: string;
    public details: string;
    public status: number;
    public brand: Brand;

    constructor() {
        this.id = 0;
        // this.brandId = 0;
        this.name = '';
        this.code = '';
        this.details = '';
        this.status = Status.Active;
        // this.brand = new Brand();
    }
}