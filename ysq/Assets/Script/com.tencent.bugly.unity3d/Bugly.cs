using com.tencent.bugly.unity3d.android;
using System;
using UnityEngine;

namespace com.tencent.bugly.unity3d
{
	public class Bugly
	{
		public static void SetGameObjectForCallback(string gameObject)
		{
			if (gameObject == null || gameObject.Trim().Length == 0)
			{
				gameObject = "Main Camera";
			}
			if (Application.platform != RuntimePlatform.IPhonePlayer)
			{
				if (Application.platform == RuntimePlatform.Android)
				{
					com.tencent.bugly.unity3d.android.Bugly.SetCallbackObject(gameObject);
				}
			}
		}

		public static void EnableLog(bool enable)
		{
			if (Application.platform != RuntimePlatform.IPhonePlayer)
			{
				if (Application.platform == RuntimePlatform.Android)
				{
					com.tencent.bugly.unity3d.android.Bugly.EnableLog(enable);
				}
			}
		}

		public static void InitSDK(string appId)
		{
			if (Application.platform != RuntimePlatform.IPhonePlayer)
			{
				if (Application.platform == RuntimePlatform.Android)
				{
					com.tencent.bugly.unity3d.android.Bugly.InitWithAppId(appId);
				}
			}
		}

		public static void EnableExceptionHandler()
		{
			if (Application.platform != RuntimePlatform.IPhonePlayer)
			{
				if (Application.platform == RuntimePlatform.Android)
				{
					com.tencent.bugly.unity3d.android.Bugly.EnableExceptionHandler();
				}
			}
		}

		public static void HandleException(Exception e)
		{
			if (Application.platform != RuntimePlatform.IPhonePlayer)
			{
				if (Application.platform == RuntimePlatform.Android)
				{
					com.tencent.bugly.unity3d.android.Bugly.OnExceptionCaught(e);
				}
			}
		}

		public static void SetUserId(string userId)
		{
			if (Application.platform != RuntimePlatform.IPhonePlayer)
			{
				if (Application.platform == RuntimePlatform.Android)
				{
					com.tencent.bugly.unity3d.android.Bugly.SetUserId(userId);
				}
			}
		}

		public static void SetAppVersion(string version)
		{
			if (Application.platform != RuntimePlatform.IPhonePlayer)
			{
				if (Application.platform == RuntimePlatform.Android)
				{
					com.tencent.bugly.unity3d.android.Bugly.SetVersion(version);
				}
			}
		}

		public static void SetChannel(string channel)
		{
			if (Application.platform != RuntimePlatform.IPhonePlayer)
			{
				if (Application.platform == RuntimePlatform.Android)
				{
					com.tencent.bugly.unity3d.android.Bugly.SetChannel(channel);
				}
			}
		}

		public static void RegisterHandler(LogSeverity level)
		{
			if (Application.platform != RuntimePlatform.IPhonePlayer)
			{
				if (Application.platform == RuntimePlatform.Android)
				{
					com.tencent.bugly.unity3d.android.Bugly.RegisterExceptionHandler(level);
				}
			}
		}

		public static void SetReportDelayTime(string delay)
		{
			if (Application.platform == RuntimePlatform.Android)
			{
				long delayReportTime = 0L;
				try
				{
					if (delay != null)
					{
						delay = delay.Trim();
						if (delay.Length > 0)
						{
							delayReportTime = Convert.ToInt64(delay);
						}
					}
				}
				catch (Exception ex)
				{
					Debugger.Error(string.Format("Fail to set report delay time cause by {0}", ex.ToString()));
					delayReportTime = 0L;
				}
				com.tencent.bugly.unity3d.android.Bugly.SetDelayReportTime(delayReportTime);
			}
		}
	}
}
