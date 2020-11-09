import { generateId } from '../Util/Generators';
import { autoserialize, autoserializeAs } from 'cerialize';
import PlayerState from '../Model/State/PlayerState';
import GlobalState from '../Model/State/GlobalState';
import ObjectiveState from '../Model/State/ObjectiveState';
import ObjectiveController from './ObjectiveController';
import PlayerController from './PlayerController';
import SocketConnection from '../Server/Socket/SocketConnection';

const TICK_TIME = 100;
const PLAYERS_PER_GAME = 4;

export default class GameController
{
    @autoserialize id: string;
    @autoserialize name: string;
    @autoserialize started: boolean;
    @autoserializeAs(PlayerController) players: PlayerController[];
    public state: GlobalState;
    private objectiveController: ObjectiveController;
    public constructor(name: string)
    {
        this.id = generateId();
        this.name = name;
        this.started = false;
        this.players = [];
    }
    public start(): void
    {
        if (this.started) {
            return;
        }
        this.started = true;
        console.info('Game ' + this.name + ' has started.');
        this.sendStartMessageToClients();
        setInterval(this.onTick.bind(this), TICK_TIME);
    }
    public connected(socketConnection: SocketConnection, playerId: string): void
    {
        const found = this.players.filter(playerController => playerController.info.id === playerId);
        if (found.length > 0) {
            found[0].startConnection(socketConnection);
        } else {
            console.info('Player with id ' + playerId + ' is not assigned to game ' + this.name + '. Will disconnect.');
            socketConnection.close();
        }
    }
    public kill(): void
    {
        clearInterval(this.onTick.bind(this));
    }
    public registerObjectiveController(objectiveController: ObjectiveController): void
    {
        this.objectiveController = objectiveController;
    }
    public registerPlayerController(playerController: PlayerController): boolean
    {
        if (this.players.length < PLAYERS_PER_GAME) {
            this.players.push(playerController);
            return true;
        }
        return false;
    }
    private onTick(): void
    {
        this.formGlobalState();
        this.sendGlobalStateToClients();
    }
    private formGlobalState(): void
    {
        const playersAggregationState: PlayerState[] = [];
        this.players.forEach(playerController => {
            playersAggregationState.push(playerController.state);
        });
        const objectiveState: ObjectiveState = this.objectiveController
            .formObjectiveState(playersAggregationState);
        this.state = new GlobalState({
            players: playersAggregationState,
            objective: objectiveState
        });
    }
    private sendStartMessageToClients(): void
    {
        this.players.forEach(playerController => {
            playerController.sendStart();
        });
    }
    private sendGlobalStateToClients(): void
    {
        this.players.forEach(playerController => {
            playerController.sendGlobalState(this.state);
        });
    }
}
