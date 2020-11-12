using System;
using System.Collections.Generic;
using UnityEngine;

public class GameViewController : MonoBehaviour
{
	public GameObject[] PlayerPrefabs;

	// state
	readonly Dictionary<int, GameObject> playerViews = new Dictionary<int, GameObject>();

	/// <summary>
	/// Update views with simulation state.
	/// </summary>
	public void UpdateViews(GameSimulator simulator)
	{
		foreach(var player in simulator.LastTickState.AllPlayers)
		{
			UpdatePlayerView(player);
		}
	}

	private void UpdatePlayerView(PlayerState playerState)
	{
		if (!playerViews.TryGetValue(playerState.Id, out GameObject view))
		{
			var prefab = PlayerPrefabs[playerState.Id - 1];
			view = GameObject.Instantiate(prefab, transform);
			playerViews.Add(playerState.Id, view);
		}

		view.transform.localPosition = playerState.Position;
	}
}