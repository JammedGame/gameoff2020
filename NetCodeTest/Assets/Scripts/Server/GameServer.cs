using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameServer : MonoBehaviour, IServer
{
	public GameSetupData GameSetup;
	public GameViewController ViewController;
	public float InitialBuffer = 0.1f; // smoothing buffer
	public float Latency = 0.25f; // one-way latency (server->client or client->server)
	public event Action<ServerSyncMessage> StateBroadcast;
	public float Clock => serverClock;

	GameSimulator gameSimulator;
	readonly List<GlobalInputState> bufferedInputs = new List<GlobalInputState>();
	float serverClock;

	void Awake()
	{
		var initialState = GameSetup.CreateInitialState();
		gameSimulator = new GameSimulator(initialState, 5);
		serverClock = -InitialBuffer - Latency;
	}

	public void Connect(int playerId)
	{
	}

	GlobalInputState GetInputStateForTick(int tickId)
	{
		while(bufferedInputs.Count <= tickId)
		{
			bufferedInputs.Add(new GlobalInputState());
		}

		return bufferedInputs[tickId];
	}

	public void SendClientInfo(ClientSyncMessage syncMessage)
	{
		Invoke(Latency, () =>
		{
			var newState = syncMessage.globalState;
			var tickId = newState.TickId;
			if (tickId < gameSimulator.LastTickId)
				return; // too late

			// update given state for given player.
			var inputState = GetInputStateForTick(tickId);
			var clientInput = newState.InputState.GetInputForPlayer(syncMessage.playerId);
			inputState.AddInputForPlayer(clientInput);
		});
	}

	void Update()
	{
		serverClock += Time.deltaTime;
		var tickId = GameSimulator.TimeToTickId(serverClock);

		while(gameSimulator.LastTickId < tickId)
		{
			var inputState = GetInputStateForTick(gameSimulator.LastTickId);
			gameSimulator.Tick(inputState);
			var serverSyncMessage = new ServerSyncMessage()
			{
				globalState = gameSimulator.LastTickState
			};

			Invoke(Latency, () =>
			{
				StateBroadcast?.Invoke(serverSyncMessage);
			});
		}

		ViewController.UpdateViews(gameSimulator);
	}

	/// <summary>
	/// Utility to simulate delay.
	/// </summary>
	void Invoke(float delay, Action action)
	{
		StartCoroutine(InvokeFlow());

		IEnumerator InvokeFlow()
		{
			yield return new WaitForSeconds(delay);
			action?.Invoke();
		}
	}
}