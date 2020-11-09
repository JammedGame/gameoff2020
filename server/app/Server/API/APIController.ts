import bodyParser from "body-parser";
import { Deserialize, Serialize } from "cerialize";
import { APIResponse, APIRoute, APIRouteType } from "./APIRoute";
import HostController from "../../Controler/HostController";

export default class APIController
{
    private expressApp: any;
    private host: HostController;
    private routes: APIRoute[];
    public constructor(expressApp: any, host: HostController)
    {
        this.host = host;
        this.expressApp = expressApp;
        this.expressApp.use(bodyParser.json());
        this.routes = [];
        this.registerRoutes();
    }
    private registerRoutes(): void
    {
        this.routes.push(new APIRoute(this.expressApp, APIRouteType.GET, '/game/all', () => this.listGames()));
        this.routes.push(new APIRoute(this.expressApp, APIRouteType.GET, '/game', params => this.findGame(params)));
        this.routes.push(new APIRoute(this.expressApp, APIRouteType.PUT, '/game/start', params => this.startGame(params)));
        this.routes.push(new APIRoute(this.expressApp, APIRouteType.PUT, '/game/join', params => this.joinGame(params)));
        this.routes.push(new APIRoute(this.expressApp, APIRouteType.POST, '/game', params => this.createGame(params)));
    }
    private listGames(): APIResponse
    {
        // TODO: Should add filter parameters
        const games = this.host.listGames();
        return { status: 200, body: { games } }
    }
    private findGame(params): APIResponse
    {
        if (!params.id) {
            return { status: 422, body: { message: 'Game id not defined.' } }
        }
        const game = this.host.findGame(params.id);
        return game
        ? { status: 200, body: Serialize(game) }
        : { status: 404, body: { message: 'Game not found.' } }
    }
    private startGame(params): APIResponse
    {
        if (!params.id) {
            return { status: 422, body: { message: 'Game id not defined.' } }
        }
        const started = this.host.startGame(params.id);
        return started
        ? { status: 200, body: { message: 'Started' } }
        : { status: 404, body: { message: 'Game not found.' } }
    }
    private joinGame(params): APIResponse
    {
        if (!params.id) {
            return { status: 422, body: { message: 'Game id not defined.' } }
        }
        if (!params.playerName) {
            return { status: 422, body: { message: 'Player name not defined.' } }
        }
        const result = this.host.joinGame(params.id, params.playerName);
        return typeof(result) != 'string'
        ? { status: 200, body: Deserialize(result) }
        : { status: 404, body: { message: result } }
    }
    private createGame(params): APIResponse
    {
        if (!params.name) {
            return { status: 422, body: { message: 'Game name not defined.' } }
        }
        const game = this.host.createGame(params.name);
        return (game)
        ? { status: 200, body: Serialize(game) }
        : { status: 400, body: { message: 'Unknown error.' } }
    }
}
