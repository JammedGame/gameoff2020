import HostController from "./Controler/HostController";
import GameServer from "./Server/GameServer";

const server = new GameServer();
const host = new HostController(server.socket);

server.start();
