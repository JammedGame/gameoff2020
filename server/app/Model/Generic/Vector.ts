import { autoserialize } from "cerialize";

export default class Vector
{
    @autoserialize x: number;
    @autoserialize y: number;
    @autoserialize z: number;
    public constructor(data?: any)
    {
        Object.assign(this, data);
    }
}
