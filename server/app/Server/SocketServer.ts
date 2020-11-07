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
        /*function (ws, msg) {
            console.log("Client joined: " + msg.headers['id']);
        
            // send "hello world" interval
            const textInterval = setInterval(() => ws.send("hello world!"), 100);
        
            ws.on('message', function (data) {
        
                if (typeof (data) === "string") {
                    // client sent a string
                    console.log("string received from client -> '" + data + "'");
        
                }
            });
        
            ws.on('close', function () {
                console.log("Client left.");
                clearInterval(textInterval);
            });
        });*/
    }
    private newConnection(ws, playerId): void
    {
        if (this.onNewConnection) {
            this.onNewConnection(new SocketConnection(ws), playerId)
        }
    }
}
