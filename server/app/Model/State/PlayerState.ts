import Vector from '../Generic/Vector';
import { autoserialize, autoserializeAs } from 'cerialize';

export default class PlayerState
{
    @autoserialize Id: string;
    @autoserializeAs(Vector) Position: Vector;
    @autoserializeAs(Vector) Velocity: Vector;
    public constructor(data?: any)
    {
        this.Position = new Vector({ x: 0, y: 0, z: 0 });
        this.Velocity = new Vector({ x: 0, y: 0, z: 0 });
        if (data) {
            Object.assign(this, data);
        }
    }
}
