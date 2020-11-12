using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameTicker : MonoBehaviour
{
	// setup data
	public GameViewController ViewController;
	public GameSetupData GameSetup;
	public int MyPlayerId;
	public float Clock => clientClock;

	// simulator
	IServer server;
	GameSimulator simulator;
	float clientClock;

	public void Start()
	{
		server = FindObjectOfType<GameServer>();
		var initialState = GameSetup.CreateInitialState();
		simulator = new GameSimulator(initialState, 6);
		server.StateBroadcast += OnStateRecieved;
	}

	private void OnStateRecieved(ServerSyncMessage msg)
	{
		simulator.RollbackToState(msg.globalState, MyPlayerId);
		ViewController.UpdateViews(simulator);
	}

	public void Update()
	{
		clientClock += Time.deltaTime;
		var tickId = GameSimulator.TimeToTickId(clientClock);
		if (tickId <= simulator.LastTickId)
			return;

		// gather input
		var inputState = new GlobalInputState();
		inputState.AddInputForPlayer(new PlayerInputState()
		{
			Id = MyPlayerId,
			HorizontalAxis = Input.GetAxis("Horizontal" + MyPlayerId),
			VerticalAxis = Input.GetAxis("Vertical" + MyPlayerId),
		});

		// tick
		simulator.Tick(inputState);
		server.SendClientInfo(new ClientSyncMessage()
		{
			playerId = MyPlayerId,
			globalState = simulator.LastTickState,
		});

		// update views.
		ViewController.UpdateViews(simulator);
	}
}