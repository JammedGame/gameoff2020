import { generateId } from "../../Util/Generators";

export default class PlayerInfo
{
    public id: string;
    public name: string;
    public constructor(data?: any)
    {
        this.id = generateId();
        if (data) {
            Object.assign(this, data);
        }
    }
}
