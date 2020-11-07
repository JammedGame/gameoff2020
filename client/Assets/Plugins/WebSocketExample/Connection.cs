using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
			Debug.Log("Received Global State " + message);
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

	async void SendWebSocketMessage()
	{
		if (websocket.State == WebSocketState.Open)
		{
			// Sending bytes
			//await websocket.Send(new byte[] { 10, 20, 30 });

			// Sending plain text
			await websocket.SendText("{ \"Id\":\"nikola\", \"Position\":{ \"x\":0, \"y\":1, \"z\":0 }, \"Velocity\":{ \"x\":1, \"y\":0, \"z\":0 } }");
		}
	}

	private async void OnApplicationQuit()
	{
		await websocket.Close();
	}
}
