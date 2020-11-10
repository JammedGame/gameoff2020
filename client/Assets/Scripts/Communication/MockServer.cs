using System.Collections.Generic;
using Communication;
using System;

public class MockServer : IServer
{
    readonly List<PlayerAdapter> _playerAdapters;
    readonly RandomDelayService _randomDelay;

    #region API

	public event Action<GlobalState> OnAuthoritativeStateRecieved;

    public void CreateGame(CreateGameRequest request, Action<CreateGameResponse> onResponse)
    {
        throw new NotImplementedException();
    }

    public void FindGame(FindGameRequest request, Action<FindGameResponse> onResponse)
    {
        throw new NotImplementedException();
    }

    public void JoinGame(GameJoinRequest request, Action<GameJoinResponse> response)
	{
		throw new NotImplementedException();
	}

    public void ListGames(ListGamesRequest request, Action<ListGamesResponse> onResponse)
    {
        throw new NotImplementedException();
    }

    public void SendClientState(PlayerState newTickState)
	{
	}

    public void StartGame(StartGameRequest request, Action<StartGameResponse> onResponse)
    {
        throw new NotImplementedException();
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