import HostController from "./Controler/HostController";
import APIController from "./Server/API/APIController";
import GameServer from "./Server/GameServer";

const server = new GameServer();
const host = new HostController(server.socket);
const api = new APIController(server.expressApp, host);

server.start();
