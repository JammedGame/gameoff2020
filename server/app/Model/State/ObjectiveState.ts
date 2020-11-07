import { autoserialize } from "cerialize";

export default class ObjectiveState
{
    @autoserialize affinity: number;
    public constructor(data?: any)
    {
        this.affinity = 0;
        if (data) {
            Object.assign(this, data);
        }
    }
}
