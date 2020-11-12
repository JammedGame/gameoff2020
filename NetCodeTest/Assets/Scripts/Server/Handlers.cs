using System;

[Serializable]
public class ClientSyncMessage
{
	public int playerId;
	public GlobalState globalState;
}

[Serializable]
public class ServerSyncMessage
{
	public GlobalState globalState;
}

public static class Handlers
{
	public const string PlayerId = "playerId";
}