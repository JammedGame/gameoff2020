using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameSetupData : ScriptableObject
{
	public List<Vector2> PlayerInitialPositions;

	public GlobalState CreateInitialState()
	{
		var initialState = new GlobalState();
		initialState.AllPlayers = new PlayerState[PlayerInitialPositions.Count];

		// dummy setup players
		for(int i = 0; i < PlayerInitialPositions.Count; i++)
		{
			initialState.AllPlayers[i] = new PlayerState()
			{
				Id = i + 1,
				Position = PlayerInitialPositions[i]
			};
		}

		return initialState;
	}
}