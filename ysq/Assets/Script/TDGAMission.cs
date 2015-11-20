using System;
using UnityEngine;

public class TDGAMission
{
	private static string JAVA_CLASS = "com.tendcloud.tenddata.TDGAMission";

	private static AndroidJavaClass agent = new AndroidJavaClass(TDGAMission.JAVA_CLASS);

	public static void OnBegin(string missionId)
	{
		if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
		{
			TDGAMission.agent.CallStatic("onBegin", new object[]
			{
				missionId
			});
		}
	}

	public static void OnCompleted(string missionId)
	{
		if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
		{
			TDGAMission.agent.CallStatic("onCompleted", new object[]
			{
				missionId
			});
		}
	}

	public static void OnFailed(string missionId, string failedCause)
	{
		if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
		{
			TDGAMission.agent.CallStatic("onFailed", new object[]
			{
				missionId,
				failedCause
			});
		}
	}
}
