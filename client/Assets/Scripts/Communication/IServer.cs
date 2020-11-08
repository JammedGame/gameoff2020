using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Communication
{
	public interface IServer
	{
		/// <summary>
		/// Called by clients when they want to join. Response will be returned by the callback later.
		/// </summary>
		void JoinGame(GameJoinRequest request, Action<GameJoinResponse> response);

		/// <summary>
		/// Called by clients to send prelimiray state.
		/// </summary>
		void SendClientState(int playerId, GameTickState newTickState);

		/// <summary>
		/// Event sent from server when authoritative state is ready.
		/// </summary>
		event Action<GameTickState> OnAuthoritativeStateRecieved;
 	}
}
