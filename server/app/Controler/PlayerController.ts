import PlayerInfo from "../Model/Info/PlayerInfo";
import PlayerState from "../Model/State/PlayerState";
import GlobalState from "../Model/State/GlobalState";
import SocketConnection from "../Server/SocketConnection";

export default class PlayerController
{
    public ready: boolean;
    public info: PlayerInfo;
    public state: PlayerState;
    private socketConnection: SocketConnection;
    public constructor(playerInfo: PlayerInfo)
    {
        this.ready = false;
        this.info = playerInfo;
    }
    public startConnection(socketConnection: SocketConnection): void
    {
        this.ready = true;
        this.socketConnection = socketConnection;
        this.socketConnection.OnReceived = data => this.receiveState(data);
        console.info('Player ' + this.info.Name + ' has connected.');
    }
    public sendGlobalState(globalState: GlobalState): void
    {
        this.socketConnection.send(globalState);
    }
    private receiveState(data: any): void
    {
        this.state = new PlayerState(data);
        console.info('Received state - ', data);
    }
}
