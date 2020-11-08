using System;
using System.Collections.Generic;
using UnityEngine;

namespace Communication
{
	[Serializable]
	public enum ResponseStatus
	{
		undefined = 0,
		success = 1,
		fail = 2
	}

	[Serializable]
	public class GameJoinRequest
	{
		public string playerName;
	}

	[Serializable]
	public class GameJoinResponse
	{
		public ResponseStatus status;
		public GameSetupData gameData;
		public int myPlayerId; // contains id assigned by the server.
	}

	/// <summary>
	/// Sent server->clients when joining an active game.
	/// </summary>
	[Serializable]
	public class GameSetupData
	{
		public string mapName;
		public List<PlayerSetupData> allPlayers;
	}

	/// <summary>
	/// General player data.
	/// </summary>
	[Serializable]
	public class PlayerSetupData
	{
		public int id;
		public string playerName;
	}

	/// <summary>
	/// State of the whole game for one tick.
	/// </summary>
	[Serializable]
	public class GlobalState
	{
		public int tickId;
		public float tickTime;
		public List<PlayerState> players;
	}

	/// <summary>
	/// State of the player ship for one tick.
	/// </summary>
	[Serializable]
	public struct PlayerState
	{
		public int id;
		public Vector3 position;
		public Quaternion rotation;
		public Vector3 velocity;
	}
}