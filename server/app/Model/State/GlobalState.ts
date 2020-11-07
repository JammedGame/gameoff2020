import PlayerState from "./PlayerState"
import ObjectiveState from "./ObjectiveState";
import { autoserializeAs } from "cerialize";

export default class GlobalState
{
    @autoserializeAs(PlayerState) players: PlayerState[];
    @autoserializeAs(ObjectiveState) objective: ObjectiveState;
    public constructor(data?: any)
    {
        this.players = [];
        if (data) {
            Object.assign(this, data);
        }
    }
}
