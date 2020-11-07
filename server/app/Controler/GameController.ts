import PlayerState from "../Model/State/PlayerState";
import GlobalState from "../Model/State/GlobalState";
import ObjectiveState from "../Model/State/ObjectiveState";
import ObjectiveController from "./ObjectiveController";
import PlayerController from "./PlayerController";
import SocketConnection from "../Server/SocketConnection";

const TICK_TIME = 100;

export default class GameController
{
    private state: GlobalState;
    private playerControllers: PlayerController[];
    private objectiveController: ObjectiveController;
    public constructor()
    {
        this.playerControllers = [];
    }
    public start(): void
    {
        setInterval(this.onTick.bind(this), TICK_TIME);
    }
    public connected(socketConnection: SocketConnection, playerId: string): void
    {
        const found = this.playerControllers.filter(playerController => playerController.info.Id === playerId);
        if (found.length > 0) {
            found[0].startConnection(socketConnection);
        }
        this.start(); // TEMP
    }
    public kill(): void
    {
        clearInterval(this.onTick.bind(this));
    }
    public registerObjectiveController(objectiveController: ObjectiveController): void
    {
        this.objectiveController = objectiveController;
    }
    public registerPlayerController(playerController: PlayerController): void
    {
        this.playerControllers.push(playerController);
    }
    private onTick(): void
    {
        this.formGlobalState();
        this.sendGlobalStateToClients();
    }
    private formGlobalState(): void
    {
        const playersAggregationState: PlayerState[] = [];
        this.playerControllers.forEach(playerController => {
            playersAggregationState.push(playerController.state);
        });
        const objectiveState: ObjectiveState = this.objectiveController
            .formObjectiveState(playersAggregationState);
        this.state = new GlobalState({
            players: playersAggregationState,
            objective: objectiveState
        });
    }
    private sendGlobalStateToClients(): void
    {
        this.playerControllers.forEach(playerController => {
            playerController.sendGlobalState(this.state);
        });
    }
}
