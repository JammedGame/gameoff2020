using System;

public interface IServer
{
	void Connect(int playerId);

	/// <summary>
	/// Called by clients to send their latest state.
	/// </summary>
	void SendClientInfo(ClientSyncMessage message);

	/// <summary>
	/// Broadcast by server when new authorithative state is recieved.
	/// </summary>
	event Action<ServerSyncMessage> StateBroadcast;
}