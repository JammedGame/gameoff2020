import PlayerState from '../Model/State/PlayerState';
import ObjectiveState from '../Model/State/ObjectiveState';

export default class ObjectiveController
{
    private state: ObjectiveState;
    public constructor(objectiveState: ObjectiveState)
    {
        this.state = objectiveState;
    }
    public formObjectiveState(playerAggregationState: PlayerState[]): ObjectiveState
    {
        return this.state;
    }
}
