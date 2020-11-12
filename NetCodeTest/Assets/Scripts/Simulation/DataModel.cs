using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GlobalState
{
	public int TickId;
	public PlayerState[] AllPlayers;
	public GlobalInputState InputState;
}

public class GlobalInputState
{
	public readonly List<PlayerInputState> PlayerInputs = new List<PlayerInputState>();

	public PlayerInputState GetInputForPlayer(int playerid)
	{
		foreach(var input in PlayerInputs)
			if (input.Id == playerid)
				return input;

		return default;
	}

	public void AddInputForPlayer(PlayerInputState playerInput)
	{
		PlayerInputs.RemoveAll(x => x.Id == playerInput.Id);
		PlayerInputs.Add(playerInput);
	}
}

public struct PlayerInputState
{
	public int Id;
	public float HorizontalAxis;
	public float VerticalAxis;
}

public struct PlayerState
{
	public int Id;
	public Vector3 Position;
	public Vector3 Velocity;
}