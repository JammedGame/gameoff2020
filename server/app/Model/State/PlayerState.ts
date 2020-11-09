import Vector from '../Generic/Vector';
import { autoserialize, autoserializeAs } from 'cerialize';

export default class PlayerState
{
    @autoserialize id: string;
    @autoserializeAs(Vector) position: Vector;
    @autoserializeAs(Vector) velocity: Vector;
    public constructor(data?: any)
    {
        this.position = new Vector({ x: 0, y: 0, z: 0 });
        this.velocity = new Vector({ x: 0, y: 0, z: 0 });
        if (data) {
            Object.assign(this, data);
        }
    }
}
