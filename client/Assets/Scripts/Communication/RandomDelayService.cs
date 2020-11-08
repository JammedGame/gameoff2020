using System;
using System.Collections;
using UnityEngine;

public class RandomDelayService : MonoBehaviour
{
	public static RandomDelayService Create()
	{
		var gameObject = new GameObject("RandomDelayService");
		DontDestroyOnLoad(gameObject);
		return gameObject.AddComponent<RandomDelayService>();
	}

	public void Delay(float delay, Action action)
	{
		if (action == null)
			return;

		StartCoroutine(DelayedCallCoroutine());

		IEnumerator DelayedCallCoroutine()
		{
			yield return new WaitForSeconds(delay);
			action.Invoke();
		}
	}

	public void Dispose()
	{
		Destroy(gameObject);
	}
}