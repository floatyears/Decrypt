using NtUniSdk.Unity3d;
using System;
using UnityEngine;

public class PlatformTools
{
	private static AndroidJavaObject activityClass;

	public static AndroidJavaObject GetActivityClass()
	{
		if (PlatformTools.activityClass == null)
		{
			PlatformTools.activityClass = new AndroidJavaClass("com.example.androidplatformtools.AndroidPlatformTools");
		}
		return PlatformTools.activityClass;
	}

	public static string GetMacAddress()
	{
		string empty = string.Empty;
		return PlatformTools.GetActivityClass().CallStatic<string>("GetMacAddress", new object[0]);
	}

	public static string GetNetType()
	{
		string text = string.Empty;
		text = PlatformTools.GetActivityClass().CallStatic<string>("GetCurNetType", new object[0]);
		if (text != null && text.Equals("Unknown Net Type"))
		{
			return string.Empty;
		}
		return text;
	}

	public static string GetNetBusiness()
	{
		string text = string.Empty;
		text = PlatformTools.GetActivityClass().CallStatic<string>("GetCurNetBusiness", new object[0]);
		if (text != null && text.Equals("unknown net business"))
		{
			return string.Empty;
		}
		return text;
	}

	public static long GetTotalSpace()
	{
		return PlatformTools.GetActivityClass().CallStatic<long>("GetTotalSpace", new object[0]);
	}

	public static long GetFreeSpace()
	{
		return PlatformTools.GetActivityClass().CallStatic<long>("GetFreeSpace", new object[0]);
	}

	public static long GetUsedSpace()
	{
		return PlatformTools.GetActivityClass().CallStatic<long>("GetUsedSpace", new object[0]);
	}

	public static void RestartApp()
	{
		PlatformTools.GetActivityClass().CallStatic("Restart", new object[0]);
		SdkU3d.exit();
	}

	public static bool IsYueYuDevice()
	{
		return PlatformTools.GetActivityClass().CallStatic<bool>("isRoot", new object[0]);
	}
}
