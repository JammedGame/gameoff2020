import express from 'express';
import {
    createServer,
    RequestListener
} from 'http';
import SocketServer from './Socket/SocketServer';

const DEFAULT_PORT = '8080';

export default class GameServer
{
    public server: any;
    public expressApp: any;
    public socket: SocketServer;
    public constructor()
    {
        this.expressApp = express();
        this.server = createServer(this.expressApp as RequestListener);
        this.socket = new SocketServer(this);
    }
    public start(): void
    {
        this.server.listen(DEFAULT_PORT, function () {
            console.info('Listening on http://localhost:' + DEFAULT_PORT);
        });
    }
}
