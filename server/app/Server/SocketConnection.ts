const EVENT_CLOSE = 'close';
const EVENT_MESSAGE = 'message';

export default class SocketConnection
{
    public open: boolean;
    public connection: any;
    public OnReceived: Function;
    public constructor(webSocketConnection: any)
    {
        this.open = true;
        this.connection = webSocketConnection;
        this.connection.on(EVENT_CLOSE, () => this.connectionClosed())
        this.connection.on(EVENT_MESSAGE, data => this.receive(data))
    }
    public send(data: any): void
    {
        this.connection.send(JSON.stringify(data));
    }
    private connectionClosed(): void
    {
        this.open = false;
        this.connection = null;
    }
    private receive(data: string): void
    {
        if (this.OnReceived) {
            this.OnReceived(JSON.parse(data));
        }
    }
}
