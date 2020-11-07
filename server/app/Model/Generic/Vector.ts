export default class Vector
{
    public x: number;
    public y: number;
    public z: number;
    public constructor(data?: any)
    {
        Object.assign(this, data);
    }
}
