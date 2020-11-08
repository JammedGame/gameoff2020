using System;
using System.Collections.Generic;
using UnityEngine;

namespace Communication
{
	[Serializable]
	public enum ResponseStatus
	{
		Undefined = 0,
		Success = 1,
		Fail = 2
	}

	[Serializable]
	public class GameJoinRequest
	{
		public string PlayerName;
	}

	[Serializable]
	public class GameJoinResponse
	{
		public ResponseStatus Status;
		public GameSetupData GameData;
		public int MyPlayerId; // contains id assigned by the server.
	}

	/// <summary>
	/// Sent server->clients when joining an active game.
	/// </summary>
	[Serializable]
	public class GameSetupData
	{
		public string MapName;
		public List<PlayerSetupData> AllPlayers;
	}

	/// <summary>
	/// General player data.
	/// </summary>
	[Serializable]
	public class PlayerSetupData
	{
		public int Id;

		public string PlayerName;
	}

	/// <summary>
	/// State of the whole game for one tick.
	/// </summary>
	[Serializable]
	public class GlobalState
	{
		public int TickId;
		public float TickTime;
		public List<PlayerState> players;
	}

	/// <summary>
	/// State of the player ship for one tick.
	/// </summary>
	[Serializable]
	public struct PlayerState
	{
		public int Id;
		public Vector3 Position;
		public Vector3 Velocity;
	}
}