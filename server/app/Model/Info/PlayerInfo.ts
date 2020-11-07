export default class PlayerInfo
{
    public Id: string;
    public Name: string;
    public constructor(data?: any)
    {
        if (data) {
            Object.assign(this, data);
        }
    }
}
