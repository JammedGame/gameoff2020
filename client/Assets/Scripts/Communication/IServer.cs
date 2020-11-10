using System;

namespace Communication
{
	public interface IServer
	{
		/// GET
		void ListGames(ListGamesRequest request, Action<ListGamesResponse> onResponse);

		/// GET
		void FindGame(FindGameRequest request, Action<FindGameResponse> onResponse);

		/// PUT
		void StartGame(StartGameRequest request, Action<StartGameResponse> onResponse);
	
		/// <summary>
		/// PUT
		/// Called by clients when they want to join. Response will be returned via callback later.
		/// </summary>
		void JoinGame(GameJoinRequest request, Action<GameJoinResponse> onResponse);

		/// POST
		void CreateGame(CreateGameRequest request, Action<CreateGameResponse> onResponse);

		/// <summary>
		/// Called by clients to send preliminary state.
		/// </summary>
		void SendClientState(PlayerState newTickState);

		/// <summary>
		/// Event sent from server when authoritative state is ready.
		/// </summary>
		event Action<GlobalState> OnAuthoritativeStateRecieved;
 	}
}
