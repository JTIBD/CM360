
export interface IPaginator<T> {
    pageIndex: number;
    pageSize: number;
    count: number;
    data: T[];
  }