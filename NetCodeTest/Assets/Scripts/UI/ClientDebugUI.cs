using System;
using UnityEngine;
using UnityEngine.UI;

public class ClientDebugUI : MonoBehaviour
{
	public GameTicker Client;
	public Text ClockText;

	void Update()
	{
		ClockText.text = DebugUtil.GetTimeDebugString(Client.Clock);
	}
}