import GameController from './GameController';
import PlayerInfo from '../Model/Info/PlayerInfo';
import PlayerController from './PlayerController';
import playersData from '../Data/TempPlayers.json';
import ObjectiveController from './ObjectiveController';
import SocketServer from '../Server/Socket/SocketServer';
import ObjectiveState from '../Model/State/ObjectiveState';
import SocketConnection from '../Server/Socket/SocketConnection';

export default class HostController
{
    private games: GameController[];
    private socketServer: SocketServer;
    public constructor(socketServer: SocketServer)
    {
        this.games = [];
        this.socketServer = socketServer;
        this.socketServer.onNewConnection = (ws, gid, pid) => this.onConnection(ws, gid, pid);
        this.createTempGame();
    }
    // TODO: Temp game, remove after it is no more needed
    public createTempGame(): void
    {
        const game = this.createGame('temp');
        game.id = 'temp';
        playersData.forEach(playerData => {
            game.registerPlayerController(
                new PlayerController(
                    new PlayerInfo(playerData)
                )
            );
        });
    }
    public onConnection(socketConnection: SocketConnection, gameId: string, playerId: string): void
    {
        const game = this.findGame(gameId);
        if (game) {
            game.connected(socketConnection, playerId);
            // TODO: Temp game start hack, remove after it is no more needed
            if (gameId === 'temp') {
                game.start();
            }
        } else {
            console.info('Game with id ' + gameId + ' not found. Will disconnect.');
            socketConnection.close();
        }
    }
    public listGames(): any[]
    {
        return this.games.map(game => ({
            id: game.id,
            name: game.name,
            started: game.started,
            players: game.players.length
        }));
    }
    public findGame(id: string): GameController
    {
        const found = this.games.filter(game => game.id === id);
        if (found.length > 0) {
            return found[0];
        }
        return null;
    }
    public startGame(id: string): boolean
    {
        const game = this.findGame(id);
        if (game) {
            game.start();
            return true;
        }
        return false;
    }
    public joinGame(id: string, playerName: string): PlayerInfo | string
    {
        const game = this.findGame(id);
        if (!game) {
            return 'Game not found.';
        }
        const playerInfo = new PlayerInfo({ name: playerName });
        const status = game.registerPlayerController( new PlayerController(playerInfo) );
        return status ? playerInfo : 'Game already full.';
    }
    public createGame(name: string): GameController
    {
        const game = new GameController(name);
        game.registerObjectiveController(
            new ObjectiveController( new ObjectiveState() )
        )
        this.games.push(game);
        return game;
    }
}
