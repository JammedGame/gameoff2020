import SocketServer from "../Server/SocketServer";
import GameController from "./GameController";
import PlayerInfo from "../Model/Info/PlayerInfo";
import PlayerController from "./PlayerController";
import playersData from "../Data/TempPlayers.json";
import SocketConnection from "../Server/SocketConnection";

export default class HostController
{
    private games: GameController[];
    private socketServer: SocketServer;
    public constructor(socketServer: SocketServer)
    {
        this.games = [];
        this.socketServer = socketServer;
        this.socketServer.onNewConnection = (ws, id) => this.onConnection(ws, id);
        this.createTempGame();
    }
    public createTempGame(): void
    {
        const game = new GameController();
        playersData.forEach(playerData => {
            game.registerPlayerController(
                new PlayerController(
                    new PlayerInfo(playerData)
                )
            );
        });
        this.games.push(game);
    }
    public onConnection(socketConnection: SocketConnection, id: string): void
    {
        // TODO: In future has to look for game id as well
        this.games[0].connected(socketConnection, id);
    }
}
