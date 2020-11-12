using System;
using System.Collections.Generic;
using UnityEngine;

public class GameSimulator
{
	GlobalState currentState;
	readonly List<GlobalInputState> inputStatesBuffers = new List<GlobalInputState>();
	readonly List<GlobalState> stateHistoryBuffer = new List<GlobalState>();

	public float levelBounds;
	public int LastTickId => stateHistoryBuffer.Count;
	public GlobalState LastTickState => currentState;

	public GameSimulator(GlobalState initialState, float levelBounds)
	{
		this.currentState = initialState;
		this.levelBounds = levelBounds;
	}

	public void Tick(GlobalInputState input)
	{
		inputStatesBuffers.Add(input);
		SimulateStatesWithBufferedInputs();
	}

	public void RollbackToState(GlobalState serverState, int playerId)
	{
		// edge case if client is not ready yet - tick will not be acked and server will include it in the next broadcast?
		if (serverState.TickId >= stateHistoryBuffer.Count)
			return;

		stateHistoryBuffer.RemoveRange(serverState.TickId, stateHistoryBuffer.Count - serverState.TickId);
		currentState = serverState;
		SimulateStatesWithBufferedInputs();
	}

	void SimulateStatesWithBufferedInputs()
	{
		while(stateHistoryBuffer.Count < inputStatesBuffers.Count)
		{
			var input = inputStatesBuffers[stateHistoryBuffer.Count];
			var newState = Simulate(currentState, input);
			stateHistoryBuffer.Add(currentState);
			currentState = newState;
		}
	}

	#region Game simulation

	public const float DeltaTime = 1f / 60f;

	public GlobalState Simulate(GlobalState previousGlobalState, GlobalInputState inputState)
	{
		// prepare state
		var newState = new GlobalState()
		{
			TickId = previousGlobalState.TickId + 1,
			AllPlayers = (PlayerState[])previousGlobalState.AllPlayers.Clone(),
			InputState = inputState
		};

		// simulate player states
		for (int i = 0; i < newState.AllPlayers.Length; i++)
		{
			ref var playerState = ref newState.AllPlayers[i];
			var playerInput = newState.InputState.GetInputForPlayer(playerState.Id);
			playerState = Simulate(playerState, playerInput);
		}

		// return new state
		return newState;
	}

	public PlayerState Simulate(PlayerState previousState, PlayerInputState inputState)
	{
		var newState = previousState;
		newState.Velocity += new Vector3(inputState.HorizontalAxis, inputState.VerticalAxis, 0);
		newState.Position += newState.Velocity * DeltaTime;
		newState.Position = ClampToBounds(newState.Position);
		newState.Velocity *= 0.7f;

		return newState;
	}

	private Vector3 ClampToBounds(Vector3 position)
	{
		if (position.x < -levelBounds) position.x = -levelBounds;
		if (position.x > levelBounds) position.x = levelBounds;
		if (position.y < -levelBounds) position.y = -levelBounds;
		if (position.y >  levelBounds) position.y = levelBounds;
		return position;
	}

	public static int TimeToTickId(float time)
	{
		if (time < 0)
			return 0;

		return Mathf.FloorToInt(time / DeltaTime);
	}

	#endregion
}