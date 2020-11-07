import Vector from "../Generic/Vector";

export default class PlayerState
{
    public Id: string;
    public Position: Vector;
    public Velocity: Vector;
    public constructor(data?: any)
    {
        if (data) {
            Object.assign(this, data);
        }
    }
}
