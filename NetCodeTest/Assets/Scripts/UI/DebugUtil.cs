using System;

public static class DebugUtil
{
	public static string GetTimeDebugString(float seconds)
	{
		if (seconds < 0)
		{
			seconds = -seconds;
			var timespan = TimeSpan.FromSeconds(seconds);
			return $"-{timespan.Seconds + timespan.Minutes * 60}:{timespan.Milliseconds / 100}s";
		}
		else
		{
			var timespan = TimeSpan.FromSeconds(seconds);
			return $"{timespan.Seconds + timespan.Minutes * 60}:{timespan.Milliseconds / 100}s";
		}
	}
}