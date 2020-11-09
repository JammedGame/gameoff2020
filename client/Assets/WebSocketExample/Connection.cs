using System;
using System.Collections;
using System.Collections.Generic;
using Communication;
using UnityEngine;
using UnityEngine.Serialization;
using NativeWebSocket;

public class Connection : MonoBehaviour
{
	bool started = false;
	WebSocket websocket;

	async void Start()
	{
		// websocket = new WebSocket("ws://echo.websocket.org");
		websocket = new WebSocket("ws://localhost:8080", headers: new Dictionary<string, string>()
		{
			["gameid"] = "temp",
			["playerid"] = "nikola",
		});

		websocket.OnOpen += () =>
		{
			Debug.Log("Connection open!");
		};

		websocket.OnError += (e) =>
		{
			Debug.Log("Error! " + e);
		};

		websocket.OnClose += (e) =>
		{
			Debug.Log("Connection closed!");
		};

		websocket.OnMessage += (bytes) =>
		{
			// Reading a plain text message
			var messageData = System.Text.Encoding.UTF8.GetString(bytes);
			var message = JsonUtility.FromJson<SocketMessage>(messageData);
			if (message.type == "start")
			{
				this.started = true;
				Debug.Log("Game started");
				InvokeRepeating("SendWebSocketMessage", 0.0f, 0.3f);
			}
			else if (message.type == "state")
			{
				var state = (GlobalState) message.data;
				Debug.Log("Received Global State");
			}
		};
		// Keep sending messages at every 0.3s

		await websocket.Connect();
	}

	void Update()
	{
		#if !UNITY_WEBGL || UNITY_EDITOR
			websocket.DispatchMessageQueue();
		#endif
	}

	private async void SendWebSocketMessage()
	{
		if (websocket.State == WebSocketState.Open)
		{
			// Sending bytes
			//await websocket.Send(new byte[] { 10, 20, 30 });

			var playerState = new PlayerState()
			{
				id = 1,
				position = new Vector3(10, 20, 30),
				velocity = new Vector3()
			};

			var playerStateText = JsonUtility.ToJson(playerState);

			// Sending plain text
			await websocket.SendText(playerStateText);
		}
	}

	private async void OnApplicationQuit()
	{
		await websocket.Close();
	}
}
