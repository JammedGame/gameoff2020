using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Communication;
using System;

public class MockServer : IServer
{
    readonly List<PlayerAdapter> _playerAdapters;
    readonly RandomDelayService _randomDelay;

    #region API

	public event Action<GlobalState> OnAuthoritativeStateRecieved;

	public void JoinGame(GameJoinRequest request, Action<GameJoinResponse> response)
	{
		throw new NotImplementedException();
	}

	public void SendClientState(int playerId, GlobalState newTickState)
	{
	}

    #endregion

    public class PlayerAdapter
    {
        public readonly int PlayerId;

        public PlayerAdapter(int playerId)
        {
            PlayerId = playerId;
        }
    }
}