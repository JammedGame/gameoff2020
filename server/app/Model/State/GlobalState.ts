import PlayerState from "./PlayerState"
import ObjectiveState from "./ObjectiveState";

export default class GlobalState
{
    private players: PlayerState[];
    private objective: ObjectiveState;
    public constructor(data?: any)
    {
        this.players = [];
        if (data) {
            Object.assign(this, data);
        }
    }
}
