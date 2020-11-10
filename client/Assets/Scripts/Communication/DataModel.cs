using System;
using System.Collections.Generic;
using UnityEngine;

namespace Communication
{
	[Serializable]
	public enum ResponseStatus
	{
		success = 0,
		fail = 1,
	}

	[Serializable]
	public enum WebsocketHeaderKey
    {
        gameid,
        playerid,
    }

	[Serializable]
    public enum WebsocketMessageType
    {
        start,
        state,
    }

	[Serializable]
	public class ListGamesRequest {}

	[Serializable]
	public class ListGamesResponse
	{
		public ResponseStatus status;
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
		public ResponseStatus status;
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
		public ResponseStatus status;
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
		public ResponseStatus status;
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
		public ResponseStatus status;
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
		public string id;
		public string playerName;
	}

	/// <summary>
	/// State of the whole game for one tick.
	/// </summary>
	[Serializable]
	public class GlobalState
	{
		public List<PlayerState> players;
		public ObjectiveState objective;

		public void CopyFrom(GlobalState newState, string excludePlayerId)
		{
			foreach (var newPlayer in newState.players)
			{
				var existingPlayerIndex = players.FindIndex(p => p.id == newPlayer.id);
                if (existingPlayerIndex < 0) players.Add(newPlayer);
                else if (newPlayer.id != excludePlayerId) players[existingPlayerIndex].CopyFrom(newPlayer);
            }
		}
	}

	/// <summary>
	/// State of the player ship for one tick.
	/// </summary>
	[Serializable]
	public struct PlayerState
	{
		public string id;
		public Vector3 position;
		public Quaternion rotation;
		public Vector3 velocity;
		public List<ProjectileState> projectiles;

		public void CopyFrom(PlayerState newState)
		{
			id = newState.id;
			position = newState.position;
			rotation = newState.rotation;
			velocity = newState.velocity;
			foreach (var newProjectile in newState.projectiles)
			{
				var existingPlayerIndex = projectiles.FindIndex(p => p.id == newProjectile.id);
				if (existingPlayerIndex >= 0) projectiles[existingPlayerIndex].CopyFrom(newProjectile);
				else projectiles.Add(newProjectile);
			}
		}
	}

	/// <summary>
	/// State of a projectile for one tick.
	/// </summary>
	[Serializable]
	public struct ProjectileState
	{
		public string id;
		public Vector3 position;
		public Quaternion rotation;
		public Vector3 velocity;

		public void CopyFrom(ProjectileState newState)
		{
			id = newState.id;
			position = newState.position;
			rotation = newState.rotation;
			velocity = newState.velocity;
		}
	}

	[Serializable]
	public struct ObjectiveState
	{
		public float affinity;
	}

	/// <summary>
	/// State of the player ship for one tick.
	/// </summary>
	[Serializable]
	public struct SocketMessage
	{
		public string type;
	}
	
	[Serializable]
	public struct StateSocketMessage
	{
		public string type;
		public GlobalState data;
	}
}