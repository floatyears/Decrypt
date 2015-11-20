using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using com.tencent.bugly.unity3d;

public sealed class BuglyAgent
{
	public delegate void LogCallbackDelegate(string condition, string stackTrace, LogType type);

	private static readonly string CLASS_UNITYAGENT = "com.tencent.bugly.unity.UnityAgent";

	private static AndroidJavaObject _unityAgent;

	private static string _configChannel;

	private static string _configVersion;

	private static string _configUser;

	private static long _configDelayTime;

	private static bool _isInitialized;

	private static LogSeverity _autoReportLogLevel = LogSeverity.LogError;

	private static bool _debugMode;

	private static bool _autoQuitApplicationAfterReport;

	private static readonly int EXCEPTION_TYPE_UNCAUGHT = 1;

	private static readonly int EXCEPTION_TYPE_CAUGHT = 2;

	private static readonly string _pluginVersion = "1.2.5";

	private static bool _uncaughtAutoReportOnce;

    private static event BuglyAgent.LogCallbackDelegate LogCallbackEventHandler;

	public static AndroidJavaObject UnityAgent
	{
		get
		{
			if (BuglyAgent._unityAgent == null)
			{
				using (AndroidJavaClass androidJavaClass = new AndroidJavaClass(BuglyAgent.CLASS_UNITYAGENT))
				{
					BuglyAgent._unityAgent = androidJavaClass.CallStatic<AndroidJavaObject>("getInstance", new object[0]);
				}
			}
			return BuglyAgent._unityAgent;
		}
	}

	public static string PluginVersion
	{
		get
		{
			return BuglyAgent._pluginVersion;
		}
	}

	public static bool IsInitialized
	{
		get
		{
			return BuglyAgent._isInitialized;
		}
	}

	public static bool AutoQuitApplicationAfterReport
	{
		get
		{
			return BuglyAgent._autoQuitApplicationAfterReport;
		}
	}

	public static void InitWithAppId(string appId)
	{
		if (BuglyAgent.IsInitialized)
		{
			BuglyAgent.DebugLog(null, "BuglyAgent has already been initialized.", new object[0]);
			return;
		}
		if (string.IsNullOrEmpty(appId))
		{
			return;
		}
		BuglyAgent.InitUnityAgent(appId);
		BuglyAgent._RegisterExceptionHandler();
	}

	public static void EnableExceptionHandler()
	{
		if (BuglyAgent.IsInitialized)
		{
			BuglyAgent.DebugLog(null, "BuglyAgent has already been initialized.", new object[0]);
			return;
		}
		BuglyAgent.PrintLog(LogSeverity.Log, "Only enable the exception handler, please make sure you has initialized the sdk in the native code in associated Android or iOS project.", new object[0]);
		BuglyAgent._RegisterExceptionHandler();
	}

	public static void RegisterLogCallback(BuglyAgent.LogCallbackDelegate handler)
	{
		if (handler != null)
		{
			BuglyAgent.DebugLog(null, "Add log callback handler", new object[0]);
			BuglyAgent.LogCallbackEventHandler = (BuglyAgent.LogCallbackDelegate)Delegate.Combine(BuglyAgent.LogCallbackEventHandler, handler);
		}
	}

	public static void ReportException(Exception e, string message)
	{
		if (!BuglyAgent.IsInitialized)
		{
			return;
		}
		BuglyAgent.DebugLog(null, "Report exception: {0}\n------------\n{1}\n------------", new object[]
		{
			message,
			e
		});
		BuglyAgent._HandleException(e, message, false);
	}

	public static void ReportException(string name, string message, string stackTrace)
	{
		if (!BuglyAgent.IsInitialized)
		{
			return;
		}
		BuglyAgent.DebugLog(null, "Report exception: {0} {1} \n{2}", new object[]
		{
			name,
			message,
			stackTrace
		});
		BuglyAgent._HandleException(LogSeverity.LogException, name, message, stackTrace, false);
	}

	public static void UnregisterLogCallback(BuglyAgent.LogCallbackDelegate handler)
	{
		if (handler != null)
		{
			BuglyAgent.DebugLog(null, "Remove log callback handler", new object[0]);
			BuglyAgent.LogCallbackEventHandler = (BuglyAgent.LogCallbackDelegate)Delegate.Remove(BuglyAgent.LogCallbackEventHandler, handler);
		}
	}

	public static void SetUserId(string userId)
	{
		if (!BuglyAgent.IsInitialized)
		{
			return;
		}
		BuglyAgent.DebugLog(null, "Set user id: {0}", new object[]
		{
			userId
		});
		BuglyAgent.SetUserInfo(userId);
	}

	public static void SetScene(int sceneId)
	{
		if (!BuglyAgent.IsInitialized)
		{
			return;
		}
		BuglyAgent.DebugLog(null, "Set scene: {0}", new object[]
		{
			sceneId
		});
		BuglyAgent.SetCurrentScene(sceneId);
	}

	public static void AddSceneData(string key, string value)
	{
		if (!BuglyAgent.IsInitialized)
		{
			return;
		}
		BuglyAgent.DebugLog(null, "Add scene data: [{0}, {1}]", new object[]
		{
			key,
			value
		});
		BuglyAgent.AddKeyAndValueInScene(key, value);
	}

	public static void ConfigDebugMode(bool enable)
	{
		BuglyAgent.EnableDebugMode(enable);
	}

	public static void ConfigAutoQuitApplication(bool autoQuit)
	{
		BuglyAgent._autoQuitApplicationAfterReport = autoQuit;
	}

	public static void ConfigAutoReportLogLevel(LogSeverity level)
	{
		BuglyAgent._autoReportLogLevel = level;
	}

	public static void ConfigDefault(string channel, string version, string user, long delay)
	{
		BuglyAgent.ConfigDefaultBeforeInit(channel, version, user, delay);
	}

	public static void DebugLog(string tag, string format, params object[] args)
	{
		if (string.IsNullOrEmpty(format))
		{
			return;
		}
		Console.WriteLine("[BuglyAgent] <{0}> - {1}", tag, string.Format(format, args));
	}

	public static void PrintLog(LogSeverity level, string format, params object[] args)
	{
		if (string.IsNullOrEmpty(format))
		{
			return;
		}
		BuglyAgent.LogToConsole(level, string.Format(format, args));
	}

	private static void ConfigDefaultBeforeInit(string channel, string version, string user, long delay)
	{
		BuglyAgent._configChannel = channel;
		BuglyAgent._configVersion = version;
		BuglyAgent._configUser = user;
		BuglyAgent._configDelayTime = delay;
	}

	private static void InitUnityAgent(string appId)
	{
		if (BuglyAgent.IsInitialized)
		{
			return;
		}
		try
		{
			BuglyAgent.UnityAgent.Call("initWithConfiguration", new object[]
			{
				appId,
				BuglyAgent._configChannel,
				BuglyAgent._configVersion,
				BuglyAgent._configUser,
				BuglyAgent._configDelayTime
			});
			BuglyAgent._isInitialized = true;
		}
		catch
		{
		}
	}

	private static void EnableDebugMode(bool enable)
	{
		BuglyAgent._debugMode = enable;
		try
		{
			BuglyAgent.UnityAgent.Call("setLogEnable", new object[]
			{
				enable
			});
		}
		catch
		{
		}
	}

	private static void SetUserInfo(string userInfo)
	{
		try
		{
			BuglyAgent.UnityAgent.Call("setUserId", new object[]
			{
				userInfo
			});
		}
		catch
		{
		}
	}

	private static void ReportException(int type, string name, string reason, string stackTrace, bool quitProgram)
	{
		try
		{
			BuglyAgent.UnityAgent.Call("traceException", new object[]
			{
				name,
				reason,
				stackTrace,
				quitProgram
			});
		}
		catch
		{
		}
	}

	private static void SetCurrentScene(int sceneId)
	{
		try
		{
			BuglyAgent.UnityAgent.Call("setScene", new object[]
			{
				sceneId
			});
		}
		catch
		{
		}
	}

	private static void AddKeyAndValueInScene(string key, string value)
	{
		try
		{
			BuglyAgent.UnityAgent.Call("addSceneValue", new object[]
			{
				key,
				value
			});
		}
		catch
		{
		}
	}

	private static void ReportAttachmentWithException(string log)
	{
	}

	private static void ReportExtrasWithException(string key, string value)
	{
	}

	private static void LogToConsole(LogSeverity level, string message)
	{
		if (!BuglyAgent._debugMode && level != LogSeverity.Log && level < LogSeverity.LogInfo)
		{
			return;
		}
		try
		{
			BuglyAgent.UnityAgent.Call("printLog", new object[]
			{
				string.Format("[BuglyAgent] <{0}> - {1}", level.ToString(), message)
			});
		}
		catch
		{
		}
	}

	private static void _RegisterExceptionHandler()
	{
		try
		{
			AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(BuglyAgent._OnUncaughtExceptionHandler);
			Application.RegisterLogCallback(new Application.LogCallback(BuglyAgent._OnLogCallbackHandler));
			BuglyAgent._isInitialized = true;
			BuglyAgent.PrintLog(LogSeverity.Log, "Register the log callback", new object[0]);
		}
		catch
		{
		}
	}

	private static void _UnregisterExceptionHandler()
	{
		try
		{
			AppDomain.CurrentDomain.UnhandledException -= new UnhandledExceptionEventHandler(BuglyAgent._OnUncaughtExceptionHandler);
			Application.RegisterLogCallback(null);
			BuglyAgent.PrintLog(LogSeverity.Log, "Unregister the log callback", new object[0]);
		}
		catch
		{
		}
	}

	private static void _OnLogCallbackHandler(string condition, string stackTrace, LogType type)
	{
		if (BuglyAgent.LogCallbackEventHandler != null)
		{
			BuglyAgent.LogCallbackEventHandler(condition, stackTrace, type);
		}
		if (!BuglyAgent.IsInitialized)
		{
			return;
		}
		if (!string.IsNullOrEmpty(condition) && condition.Contains("[BuglyAgent] <Log>"))
		{
			return;
		}
		if (BuglyAgent._uncaughtAutoReportOnce)
		{
			return;
		}
		LogSeverity logSeverity = LogSeverity.Log;
		switch (type)
		{
		case LogType.Error:
			logSeverity = LogSeverity.LogError;
			break;
		case LogType.Assert:
			logSeverity = LogSeverity.LogAssert;
			break;
		case LogType.Warning:
			logSeverity = LogSeverity.LogWarning;
			break;
		case LogType.Log:
			logSeverity = LogSeverity.LogDebug;
			break;
		case LogType.Exception:
			logSeverity = LogSeverity.LogException;
			break;
		}
		if (logSeverity == LogSeverity.Log)
		{
			return;
		}
		BuglyAgent.PrintLog(LogSeverity.Log, "[{0}] {1}\n{2}", new object[]
		{
			type.ToString(),
			condition,
			stackTrace
		});
		BuglyAgent._HandleException(logSeverity, null, condition, stackTrace, true);
	}

	private static void _OnUncaughtExceptionHandler(object sender, UnhandledExceptionEventArgs args)
	{
		if (args == null || args.ExceptionObject == null)
		{
			return;
		}
		try
		{
			if (args.ExceptionObject.GetType() != typeof(Exception))
			{
				return;
			}
		}
		catch
		{
			if (UnityEngine.Debug.isDebugBuild)
			{
				UnityEngine.Debug.Log("BuglyAgent: Failed to report uncaught exception");
			}
			return;
		}
		if (!BuglyAgent.IsInitialized)
		{
			return;
		}
		if (BuglyAgent._uncaughtAutoReportOnce)
		{
			return;
		}
		BuglyAgent._HandleException((Exception)args.ExceptionObject, null, true);
	}

	private static void _HandleException(Exception e, string message, bool uncaught)
	{
		if (e == null)
		{
			return;
		}
		if (!BuglyAgent.IsInitialized)
		{
			return;
		}
		string name = e.GetType().Name;
		string text = e.Message;
		if (!string.IsNullOrEmpty(message))
		{
			text = string.Format("{0}{1}***{2}", text, Environment.NewLine, message);
		}
		StringBuilder stringBuilder = new StringBuilder(string.Empty);
		StackTrace stackTrace = new StackTrace(e, true);
		int frameCount = stackTrace.FrameCount;
		for (int i = 0; i < frameCount; i++)
		{
			StackFrame frame = stackTrace.GetFrame(i);
			stringBuilder.AppendFormat("{0}.{1}", frame.GetMethod().DeclaringType.Name, frame.GetMethod().Name);
			ParameterInfo[] parameters = frame.GetMethod().GetParameters();
			if (parameters == null || parameters.Length == 0)
			{
				stringBuilder.Append(" () ");
			}
			else
			{
				stringBuilder.Append(" (");
				int num = parameters.Length;
				for (int j = 0; j < num; j++)
				{
					ParameterInfo parameterInfo = parameters[j];
					stringBuilder.AppendFormat("{0} {1}", parameterInfo.ParameterType.Name, parameterInfo.Name);
					if (j != num - 1)
					{
						stringBuilder.Append(", ");
					}
				}
				stringBuilder.Append(") ");
			}
			string text2 = frame.GetFileName();
			if (!string.IsNullOrEmpty(text2) && !text2.ToLower().Equals("unknown"))
			{
				text2 = text2.Replace("\\", "/");
				int num2 = text2.ToLower().IndexOf("/assets/");
				if (num2 < 0)
				{
					num2 = text2.ToLower().IndexOf("assets/");
				}
				if (num2 > 0)
				{
					text2 = text2.Substring(num2);
				}
				stringBuilder.AppendFormat("(at {0}:{1})", text2, frame.GetFileLineNumber());
			}
			stringBuilder.AppendLine();
		}
		BuglyAgent._reportException(uncaught, name, text, stringBuilder.ToString());
	}

	private static void _reportException(bool uncaught, string name, string reason, string stackTrace)
	{
		if (string.IsNullOrEmpty(name))
		{
			return;
		}
		if (string.IsNullOrEmpty(stackTrace))
		{
			stackTrace = StackTraceUtility.ExtractStackTrace();
		}
		if (string.IsNullOrEmpty(stackTrace))
		{
			stackTrace = "Empty";
		}
		else
		{
			try
			{
				string[] array = stackTrace.Split(new char[]
				{
					'\n'
				});
				if (array != null && array.Length > 0)
				{
					StringBuilder stringBuilder = new StringBuilder();
					int num = array.Length;
					for (int i = 0; i < num; i++)
					{
						string text = array[i];
						if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(text.Trim()))
						{
							text = text.Trim();
							if (!text.StartsWith("System.Collections.Generic.") && !text.StartsWith("ShimEnumerator"))
							{
								if (!text.Contains("Bugly"))
								{
									if (!text.Contains("..ctor"))
									{
										text = text.Replace(":", ".");
										int num2 = text.ToLower().IndexOf("(at");
										int num3 = text.ToLower().IndexOf("/assets/");
										if (num2 > 0 && num3 > 0)
										{
											stringBuilder.AppendFormat("{0}(at {1}", text.Substring(0, num2), text.Substring(num3));
										}
										else
										{
											stringBuilder.Append(text);
										}
										stringBuilder.AppendLine();
									}
								}
							}
						}
					}
					stackTrace = stringBuilder.ToString();
				}
			}
			catch
			{
			}
		}
		BuglyAgent.PrintLog(LogSeverity.Log, "\n*********\n{0} {1}\n{2}\n*********", new object[]
		{
			name,
			reason,
			stackTrace
		});
		BuglyAgent._uncaughtAutoReportOnce = (uncaught && BuglyAgent._autoQuitApplicationAfterReport);
		BuglyAgent.ReportException((!uncaught) ? BuglyAgent.EXCEPTION_TYPE_CAUGHT : BuglyAgent.EXCEPTION_TYPE_UNCAUGHT, name, reason, stackTrace, uncaught && BuglyAgent._autoQuitApplicationAfterReport);
	}

	private static void _HandleException(LogSeverity logLevel, string name, string message, string stackTrace, bool uncaught)
	{
		if (!BuglyAgent.IsInitialized)
		{
			BuglyAgent.PrintLog(LogSeverity.Log, "It has not been initialized.", new object[0]);
			return;
		}
		if (logLevel == LogSeverity.Log)
		{
			return;
		}
		if (uncaught && logLevel < BuglyAgent._autoReportLogLevel)
		{
			BuglyAgent.PrintLog(LogSeverity.Log, "Not report exception for level {0}", new object[]
			{
				logLevel.ToString()
			});
			return;
		}
		string text = null;
		string text2 = null;
		if (!string.IsNullOrEmpty(message))
		{
			try
			{
				if (logLevel == LogSeverity.LogException && message.Contains("Exception"))
				{
					Match match = new Regex("^(?<errorType>\\S+):\\s*(?<errorMessage>.*)").Match(message);
					if (match.Success)
					{
						text = match.Groups["errorType"].Value;
						text2 = match.Groups["errorMessage"].Value.Trim();
					}
				}
			}
			catch
			{
			}
			if (string.IsNullOrEmpty(text2))
			{
				text2 = message;
			}
		}
		if (string.IsNullOrEmpty(name))
		{
			if (string.IsNullOrEmpty(text))
			{
				text = string.Format("Unity{0}", logLevel.ToString());
			}
		}
		else
		{
			text = name;
		}
		BuglyAgent._reportException(uncaught, text, text2, stackTrace);
	}
}
