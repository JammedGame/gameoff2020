import PlayerInfo from '../Model/Info/PlayerInfo';
import PlayerState from '../Model/State/PlayerState';
import GlobalState from '../Model/State/GlobalState';
import SocketConnection from '../Server/Socket/SocketConnection';
import { autoserialize, autoserializeAs, Deserialize, Serialize } from 'cerialize';

export default class PlayerController
{
    @autoserialize connected: boolean;
    @autoserializeAs(PlayerInfo) info: PlayerInfo;
    public state: PlayerState;
    private socketConnection: SocketConnection;
    public constructor(playerInfo: PlayerInfo)
    {
        this.info = playerInfo;
    }
    public startConnection(socketConnection: SocketConnection): void
    {
        this.connected = true;
        if (!this.socketConnection) {
            console.info('Player ' + this.info.name + ' has connected.');
        } else {
            console.info('Player ' + this.info.name + ' has reconnected.');
        }
        this.socketConnection = socketConnection;
        this.socketConnection.OnClosed = () => this.lostConnection();
        this.socketConnection.OnReceived = data => this.receiveState(data);
    }
    public lostConnection(): void
    {
        this.connected = false;
        console.info('Player ' + this.info.name + ' has disconnected.');
    }
    public sendStart(): void
    {
        if (this.socketConnection?.open) {
            this.socketConnection.send({ type: 'start' });
        }
    }
    public sendGlobalState(globalState: GlobalState): void
    {
        if (this.socketConnection?.open) {
            this.socketConnection.send({ type: 'state', data: Serialize(globalState) });
        }
    }
    private receiveState(data: any): void
    {
        this.state = Deserialize(data, PlayerState);
    }
}
