import { autoserializeAs } from 'cerialize';
import PlayerState from './PlayerState'
import ObjectiveState from './ObjectiveState';

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
