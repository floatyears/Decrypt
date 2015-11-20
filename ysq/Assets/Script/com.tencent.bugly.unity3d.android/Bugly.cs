using System;
using UnityEngine;
namespace com.tencent.bugly.unity3d.android
{
	public static class Bugly
	{
		private sealed class BuglyAgent : ExceptionHandler
		{
			public static readonly Bugly.BuglyAgent instance = new Bugly.BuglyAgent();

			private AndroidJavaObject _bugly;

			private string _gameObjectForCallback = "Main Camera";

			private BuglyAgent()
			{
				AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.tencent.bugly.unity.UnityAgent");
				this._bugly = androidJavaClass.CallStatic<AndroidJavaObject>("getInstance", new object[0]);
			}

			public static Bugly.BuglyAgent GetInstance()
			{
				return Bugly.BuglyAgent.instance;
			}

			public void SetLogLevel(LogSeverity level)
			{
				this._logLevel = level;
			}

			public void EnableLog(bool enable)
			{
				this._bugly.Call("setLogEnable", new object[]
				{
					enable
				});
			}

			public void SetDelayReportTime(long delay)
			{
				this._bugly.Call("setDelay", new object[]
				{
					delay
				});
			}

			public void SetChannel(string channel)
			{
				this._bugly.Call("setChannel", new object[]
				{
					channel
				});
			}

			public void SetVersion(string version)
			{
				this._bugly.Call("setVersion", new object[]
				{
					version
				});
			}

			public void InitWithAppId(string appId)
			{
				base.RegisterExceptionHandler();
				this._bugly.Call("initSDK", new object[]
				{
					appId
				});
			}

			public void SetUserId(string userId)
			{
				this._bugly.Call("setUserId", new object[]
				{
					userId
				});
			}

			public void SetCallbackObject(string gameObject)
			{
				if (gameObject == null)
				{
					return;
				}
				this._gameObjectForCallback = gameObject;
			}

			public void SetCrashUploadCallback(string callbackName)
			{
				if (callbackName == null)
				{
					return;
				}
				this._bugly.Call("setCrashUploadListener", new object[]
				{
					this._gameObjectForCallback,
					callbackName
				});
			}

			public void SetCrashHappenCallback(string callbackName)
			{
				if (callbackName == null)
				{
					return;
				}
				this._bugly.Call("setCrashHappenListener", new object[]
				{
					this._gameObjectForCallback,
					callbackName
				});
			}

			private void ReportException(string errorClass, string errorMessage, string callStack)
			{
				this._bugly.Call("traceException", new object[]
				{
					errorClass,
					errorMessage,
					callStack
				});
			}

			public override void OnUncaughtExceptionReport(string type, string message, string stack)
			{
				this.ReportException(type, message, stack);
			}
		}

		public static void EnableLog(bool enable)
		{
			Bugly.BuglyAgent.GetInstance().EnableLog(enable);
		}

		public static void SetDelayReportTime(long delay)
		{
			Bugly.BuglyAgent.GetInstance().SetDelayReportTime(delay);
		}

		public static void SetChannel(string channel)
		{
			Bugly.BuglyAgent.GetInstance().SetChannel(channel);
		}

		public static void SetVersion(string version)
		{
			Bugly.BuglyAgent.GetInstance().SetVersion(version);
		}

		public static void InitWithAppId(string appId)
		{
			Bugly.BuglyAgent.GetInstance().InitWithAppId(appId);
		}

		public static void SetUserId(string userId)
		{
			Bugly.BuglyAgent.GetInstance().SetUserId(userId);
		}

		public static void OnExceptionCaught(Exception e)
		{
			Bugly.BuglyAgent.GetInstance().OnExceptionCaught(e);
		}

		public static void SetCallbackObject(string gameObject)
		{
			Bugly.BuglyAgent.GetInstance().SetCallbackObject(gameObject);
		}

		public static void SetCrashUploadCallback(string callbackName)
		{
			Bugly.BuglyAgent.GetInstance().SetCrashUploadCallback(callbackName);
		}

		public static void SetCrashHappenCallback(string callbackName)
		{
			if (callbackName == null)
			{
				return;
			}
			Bugly.BuglyAgent.GetInstance().SetCrashHappenCallback(callbackName);
		}

		public static void UnregisterExceptionHandler()
		{
		}

		public static void RegisterExceptionHandler(LogSeverity level)
		{
			Bugly.BuglyAgent.GetInstance().SetLogLevel(level);
		}

		public static void EnableExceptionHandler()
		{
			Bugly.BuglyAgent.GetInstance().RegisterExceptionHandler();
		}
	} 
}
