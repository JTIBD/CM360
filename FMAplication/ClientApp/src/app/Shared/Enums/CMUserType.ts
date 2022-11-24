import { MapObject } from "./mapObject";

export enum CMUserType {
    CMR,
    TMS
}

export class UserType {
    

    public static UserTypes: MapObject[] = [
        { id: 0, label: "CMR" },
        { id: 1, label: "TMS" },
    ];
}