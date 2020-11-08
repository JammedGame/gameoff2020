using System;
using System.Collections;
using System.Collections.Generic;
using Communication;
using UnityEngine;
using UnityEngine.Serialization;
using NativeWebSocket;

public class Connection : MonoBehaviour
{
	WebSocket websocket;

	async void Start()
	{
		// websocket = new WebSocket("ws://echo.websocket.org");
		websocket = new WebSocket("ws://localhost:8080", headers: new Dictionary<string, string>()
		{
			["id"] = "nikola"
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
			var message = System.Text.Encoding.UTF8.GetString(bytes);
			var globalState = JsonUtility.FromJson<GameTickState>(message);
			Debug.Log("Received Global State " + JsonUtility.ToJson(globalState));
		};

		// Keep sending messages at every 0.3s
		InvokeRepeating("SendWebSocketMessage", 0.0f, 0.3f);

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
				Id = 1,
				Position = new Vector3(10, 20, 30),
				Velocity = new Vector3()
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
