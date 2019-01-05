using UnityEngine;
using System.Collections;
using System;

public class MyTime
{
	static System.DateTime baseTime = System.DateTime.UtcNow;
	static float offsetTime = Time.realtimeSinceStartup;

	public static long serverZoneSecondsOffset;
	static long serverFirstStartUtc;
	public static int serverLaunchedDays;

	// UTC in seconds
	public static void SetBaseTime(long seconds)
	{
		baseTime = FromSeconds(seconds);
		offsetTime = Time.realtimeSinceStartup;
	}

	public static void SetServerLaunchedDays(int day)
	{
		serverLaunchedDays = day;
	}

	public static void SetSeverZone(long seconds)
	{
		serverZoneSecondsOffset = seconds;
	}

	public static void SetSeverFirstStartUtc(long seconds)
	{
		serverFirstStartUtc = seconds;
	}

	public static bool IsServerLaunchedDurFirstDay()
	{
		System.DateTime startDate = MyTime.FromSecondsSince1970(serverFirstStartUtc).AddSeconds(MyTime.serverZoneSecondsOffset);
		System.DateTime startDate0 = new System.DateTime(startDate.Year, startDate.Month, startDate.Day, 0, 0, 0, 0, System.DateTimeKind.Unspecified);

		return ServerNow.Ticks - startDate0.AddDays(1).Ticks < 0;
	}

	public static double DaysSinceLaunchServer()
	{
		return (UtcNow - FromSeconds(serverFirstStartUtc)).TotalDays;

	}

	public static System.DateTime ServerNow
	{
		get
		{
			return UtcNowSince1970.AddSeconds(serverZoneSecondsOffset);
		}
	}

	public static System.DateTime UtcNow
	{
		get
		{
			return baseTime.AddSeconds(Time.realtimeSinceStartup - offsetTime);
		}
	}

	public static System.DateTime Now
	{
		get
		{
			return UtcNow.ToLocalTime();
		}
	}

	public static System.DateTime Utc1970 = new System.DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

	public static System.DateTime UtcNowSince1970
	{
		get
		{
			return Utc1970.Add(UtcNow - System.DateTime.MinValue);
		}
	}

	public static System.DateTime NowSince1970
	{
		get
		{
			return UtcNowSince1970.ToLocalTime();
		}
	}

	public static long UtcNowInSecondsSince1970
	{
		get
		{
			return (UtcNowSince1970.Ticks - 621355968000000000) / 10000000;
		}
	}

	public static System.DateTime FromSeconds(double seconds)
	{
		try
		{
			return System.DateTime.MinValue.AddSeconds(seconds);
		}
		catch(System.Exception)
		{
			return System.DateTime.MinValue;
		}
	}

	public static System.DateTime FromSecondsSince1970(double seconds)
	{
		return Utc1970.AddSeconds(seconds);
	}

	public static System.DateTime DateTimeInServerZone(System.DateTime dateTime)
	{
		return dateTime.AddSeconds(serverZoneSecondsOffset);
	}

	public static System.DateTime FromTicks(long ticks)
	{
		try
		{
			return System.DateTime.MinValue.AddTicks(ticks);
		}
		catch(System.Exception)
		{
			return System.DateTime.MinValue;
		}
	}
	public static System.DateTime FromTicksSince1970(long ticks)
	{
		return Utc1970.AddTicks(ticks);
	}

	public static string FormatTimeSpan(System.TimeSpan timeSpan, bool hourFormat = false, bool clockFormat = false)
	{
		int days = timeSpan.Days;
		int hours = timeSpan.Hours;
		int mins = timeSpan.Minutes;
		int secs = timeSpan.Seconds;

		string format = "";
		if(clockFormat)
		{
			hours += days * 24;
			format = string.Format("{0}:{1:D2}:{2:D2}", hours, mins, secs);
		}
		else if(hourFormat)
		{
			if(days > 0)
			{
				//format = localization.Strings.instance.GetString("UI", "Common_Time3", days, hours, mins);
			}
			else
			{
				//format = localization.Strings.instance.GetString("UI", "Common_Time4", hours, mins);
			}
		}
		else
		{
			if(days > 0)
			{
				//format = localization.Strings.instance.GetString("UI", "Common_Time3", days, hours, mins);
			}
			else if(hours > 0)
			{
				//format = localization.Strings.instance.GetString("UI", "Common_Time2", hours, mins, secs);
			}
			else
			{
				//format = localization.Strings.instance.GetString("UI", "Common_Time1", mins, secs);
			}
		}
		return format;
	}

	public static string GetTimeTo(long utcTicksSinceZero, bool hourFormat = false, bool clockFormat = false)
	{
		var timeToRecharge = utcTicksSinceZero - UtcNow.Ticks;
		if(timeToRecharge < 0)
			timeToRecharge = 0;
		var timeSpan = new System.TimeSpan(timeToRecharge);
		return FormatTimeSpan(timeSpan, hourFormat, clockFormat);
	}
	public static string GetTimeToSince70(long utcTicksSince70)
	{
		var timeToRecharge = utcTicksSince70 - UtcNowSince1970.Ticks;
		if(timeToRecharge < 0)
			timeToRecharge = 0;
		var timeSpan = new System.TimeSpan(timeToRecharge);
		return FormatTimeSpan(timeSpan);
	}
	public static string GetTimeToServerNow(long svrTicksSince)
	{
		var timeToRecharge = svrTicksSince - ServerNow.Ticks;
		if(timeToRecharge < 0)
			timeToRecharge = 0;
		var timeSpan = new System.TimeSpan(timeToRecharge);
		return FormatTimeSpan(timeSpan);
	}
	public static string GetTimeToBySeconds(int second,bool hourFormat = false, bool clockFormat = false)
	{
		var timeSpan = new System.TimeSpan(0, 0, second);
		return FormatTimeSpan(timeSpan,hourFormat,clockFormat);
	}

	public static int GetDayNumFromNow(long utcTicks)
	{
		var timeToRecharge = UtcNow.Ticks - utcTicks;
		var timeSpan = new System.TimeSpan(timeToRecharge);

		return timeSpan.Days;
	}

	public static double GetHourNumFromNow(long utcTicks)
	{
		var timeToRecharge = UtcNow.Ticks - utcTicks;
		var timeSpan = new System.TimeSpan(timeToRecharge);

		return timeSpan.TotalHours;
	}

	public static double GetMinuteNumFromNow(long utcTicks)
	{
		var timeToRecharge = UtcNow.Ticks - utcTicks;
		var timeSpan = new System.TimeSpan(timeToRecharge);

		return timeSpan.TotalMinutes;
	}
	public static float DaysFromSeconds(float seconds)
	{
		return seconds / (3600 * 24);
	}
}

class MyTimeTest
{
	[common.FloatingDebug.Item("Utils", "ServerUtc", common.FloatingDebug.ItemAttribute.Type.TextInfo)]
	static string UtcNow()
	{
		return MyTime.UtcNow.ToString("MM/dd HH:mm:ss");
	}
}