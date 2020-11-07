import * as WebSocket from 'ws';
import GameServer from './GameServer';
import SocketConnection from './SocketConnection';

export default class SocketServer
{
    public wss: WebSocket.Server;
    public onNewConnection: Function;
    public constructor(server: GameServer)
    {
        this.wss = new WebSocket.Server({ server: server.server });
        this.connect();
    }
    private connect(): void
    {
        console.info('Socket events ready');
        this.wss.on('connection', (ws, msg) => this.newConnection(ws, msg.headers['id']));
        
    }
    private newConnection(ws, playerId): void
    {
        if (this.onNewConnection) {
            this.onNewConnection(new SocketConnection(ws), playerId)
        }
    }
}
