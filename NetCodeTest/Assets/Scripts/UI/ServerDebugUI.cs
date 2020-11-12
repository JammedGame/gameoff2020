using System;
using UnityEngine;
using UnityEngine.UI;

public class ServerDebugUI : MonoBehaviour
{
	public GameServer Server;
	public Text ClockText;
	public Slider LatencySlider;

	void Start()
	{
		LatencySlider.value = Server.Latency;
		LatencySlider.minValue = 0;
		LatencySlider.maxValue = 1;
		LatencySlider.onValueChanged.AddListener(OnValueChanged);
	}

	void OnValueChanged(float newValue)
	{
		Server.Latency = newValue;
	}

	void Update()
	{
		ClockText.text = DebugUtil.GetTimeDebugString(Server.Clock);
	}
}