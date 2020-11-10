using System;
using System.Collections.Generic;
using UnityEngine;

namespace Communication
{
	[Serializable]
	public class ListGamesRequest {}

	[Serializable]
	public class ListGamesResponse
	{
		public string message;
		public List<GameSetupData> games;
	}

	[Serializable]
	public class FindGameRequest
	{
		public string id;
	}

	[Serializable]
	public class FindGameResponse
	{
		public string message;
		public string id;
		public string name;
		public bool started;
		public List<PlayerSetupData> players;
	}

	[Serializable]
	public class StartGameRequest
	{
		public string id;
	}

	[Serializable]
	public class StartGameResponse
	{
		public string message;
	}

	[Serializable]
	public class GameJoinRequest
	{
		public string id;
		public string playerName;
	}

	[Serializable]
	public class GameJoinResponse
	{
		public string message;
		public string id;
		public string name;
	}

	[Serializable]
	public class CreateGameRequest
	{
		public string name;
	}

	[Serializable]
	public class CreateGameResponse
	{
		public string message;
		public string id;
		public string name;
		public bool started;
		public List<PlayerSetupData> players;
	}

	/// <summary>
	/// Sent server->clients when joining an active game.
	/// </summary>
	[Serializable]
	public class GameSetupData
	{
		public string id;
		public string name;
		public bool started;
		public int players;
	}

	/// <summary>
	/// General player data.
	/// </summary>
	[Serializable]
	public class PlayerSetupData
	{
		public PlayerInfo info;
	}

	[Serializable]
	public class PlayerInfo
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
		public List<ProjectileState> projectiles;
	}

	/// <summary>
	/// State of a projectile for one tick.
	/// </summary>
	[Serializable]
	public struct ProjectileState
	{
		public Vector3 position;
		public Quaternion rotation;
		public Vector3 velocity;
	}

	/// <summary>
	/// State of the player ship for one tick.
	/// </summary>
	[Serializable]
	public struct SocketMessage
	{
		public string type;
		public object data;
	}
}