using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using NativeWebSocket;
using UnityEngine;
using UnityEngine.Networking;

public class GameServerRemote : IServer
{
	private const string ApiUrlHttp = "http://localhost:8080/";
	private const string ApiUrlWebsocket = "ws://localhost:8080";
	public event Action<ServerSyncMessage> StateBroadcast;
	private WebSocket websocket;
	GameTicker gameClient;

	public GameServerRemote(GameTicker gameClient)
	{
		this.gameClient = gameClient;

		Connect(gameClient.MyPlayerId);
	}

	public void Connect(int playerId)
	{
		websocket = new WebSocket(ApiUrlWebsocket, headers: new Dictionary<string, string>
		{
			[Handlers.PlayerId] = playerId.ToString(),
		});
		websocket.OnOpen += () => Debug.Log("Connection open!");
		websocket.OnError += e => Debug.LogError("Error! " + e);
		websocket.OnClose += e => Debug.Log("Connection closed!");
		websocket.OnMessage += WebsocketOnMessage;
		websocket.Connect();
	}

	public void SendClientInfo(ClientSyncMessage syncMessage)
	{
		if (websocket.State != WebSocketState.Open)
			return;

		var text = JsonUtility.ToJson(syncMessage);
		websocket.SendText(text);
	}

	private void WebsocketOnMessage(byte[] data)
	{
		var text = Encoding.UTF8.GetString(data);
		var globalState = JsonUtility.FromJson<ServerSyncMessage>(text);
		StateBroadcast?.Invoke(globalState);
	}

	private async void Dispose()
	{
		if (websocket.State != WebSocketState.Open) return;

		await websocket.Close();
	}
}