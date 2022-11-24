export class APIResponse<T=any>{
    public data: T;
    public errors: any [];
    public msg: string;
    public status: string;
    public statusCode: number;
}
export class APIResponsePage{
    public data: APIModel;
    public errors: any [];
    public msg: string;
    public status: string;
    public statusCode: number;
}

export class APIModel<T=any[]> {
    public firstItemOnPage: number;
    public hasNextPage: boolean;
    public isLastPage: boolean;
    public lastItemOnPage: number;
    public model: T;
    public pageCount: number;
    public pageNumber: number;
    public pageSize: number;
    public totalItemCount: number;
}