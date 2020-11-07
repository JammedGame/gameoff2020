export default class ObjectiveState
{
    public affinity: number;
    public constructor(data?: any)
    {
        this.affinity = 0;
        if (data) {
            Object.assign(this, data);
        }
    }
}
