using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public static class Debug
{
	public enum LogLevel
	{
		Trace,
		Log,
		Warning,
		Error,
		Fatal,
		Exception
	}

	public static readonly bool EnableDebugMode = false;

	private static FileStream sm_logFileStream = null;

	private static StreamWriter sm_logFileWriteStream = null;

	private static StringBuilder bd = new StringBuilder();

	private static bool _initialized = false;

	private static Dictionary<global::Debug.LogLevel, bool> _logLevelEnabled = new Dictionary<global::Debug.LogLevel, bool>();

	private static List<ILogOutputListener> _logOutputListeners = new List<ILogOutputListener>();

	public static void Initialize()
	{
		if (global::Debug._initialized)
		{
			return;
		}
		global::Debug._logLevelEnabled[global::Debug.LogLevel.Trace] = global::Debug.EnableDebugMode;
		global::Debug._logLevelEnabled[global::Debug.LogLevel.Log] = global::Debug.EnableDebugMode;
		global::Debug._logLevelEnabled[global::Debug.LogLevel.Warning] = global::Debug.EnableDebugMode;
		global::Debug._logLevelEnabled[global::Debug.LogLevel.Error] = global::Debug.EnableDebugMode;
		global::Debug._logLevelEnabled[global::Debug.LogLevel.Fatal] = global::Debug.EnableDebugMode;
		global::Debug._logLevelEnabled[global::Debug.LogLevel.Exception] = global::Debug.EnableDebugMode;
		global::Debug._initialized = true;
		try
		{
			global::Debug.sm_logFileStream = new FileStream(global::Debug.GetWriteLogFilename(), FileMode.Create, FileAccess.Write, FileShare.Read);
			global::Debug.sm_logFileWriteStream = new StreamWriter(global::Debug.sm_logFileStream);
		}
		catch (Exception ex)
		{
			global::Debug.LogError(new object[]
			{
				"Debug.sm_logFileStream Initialize Failed Exception:" + ex.Message
			});
		}
	}

	public static string GetWriteLogFilename()
	{
		return Application.persistentDataPath + "/" + SystemInfo.deviceUniqueIdentifier + "_log.txt";
	}

	public static void Release()
	{
		global::Debug.UnregisterAllLogOutputListener();
		global::Debug.bd.Remove(0, global::Debug.bd.Length);
		if (global::Debug.sm_logFileWriteStream != null)
		{
			global::Debug.sm_logFileWriteStream.Close();
			global::Debug.sm_logFileWriteStream = null;
		}
		if (global::Debug.sm_logFileStream != null)
		{
			global::Debug.sm_logFileStream.Close();
			global::Debug.sm_logFileStream = null;
		}
	}

	public static void EnableLogLevel(global::Debug.LogLevel logLevel, bool enable)
	{
		global::Debug.Initialize();
		global::Debug._logLevelEnabled[logLevel] = enable;
	}

	public static bool IsLogLevelEnabled(global::Debug.LogLevel logLevel)
	{
		global::Debug.Initialize();
		return global::Debug._logLevelEnabled[logLevel];
	}

	public static void LogTrace(params object[] msg)
	{
		global::Debug.LogObject(global::Debug.LogLevel.Trace, msg);
	}

	public static void LogTraceFormat(string format, params object[] msg)
	{
		global::Debug.LogFormat(global::Debug.LogLevel.Trace, format, msg);
	}

	public static void Log(params object[] msg)
	{
		global::Debug.LogObject(global::Debug.LogLevel.Log, msg);
	}

	public static void LogFormat(string format, params object[] msg)
	{
		global::Debug.LogFormat(global::Debug.LogLevel.Log, format, msg);
	}

	public static void LogWarning(params object[] msg)
	{
		global::Debug.LogObject(global::Debug.LogLevel.Warning, msg);
	}

	public static void LogWarningFormat(string format, params object[] msg)
	{
		global::Debug.LogFormat(global::Debug.LogLevel.Warning, format, msg);
	}

	public static void LogError(params object[] msg)
	{
		global::Debug.LogObject(global::Debug.LogLevel.Error, msg);
	}

	public static void LogErrorFormat(string format, params object[] msg)
	{
		global::Debug.LogFormat(global::Debug.LogLevel.Error, format, msg);
	}

	public static void LogFatal(params object[] msg)
	{
		global::Debug.LogObject(global::Debug.LogLevel.Fatal, msg);
	}

	public static void LogFatalFormat(string format, params object[] msg)
	{
		global::Debug.LogFormat(global::Debug.LogLevel.Fatal, format, msg);
	}

	public static void LogException(string message, Exception ex)
	{
		global::Debug.LogFormat(global::Debug.LogLevel.Exception, string.Format("{0} : {1} \n {2}", message, ex.Message, ex.StackTrace), new object[0]);
	}

	public static void Assert(bool condition, string msg)
	{
		if (!condition)
		{
			throw new AssertExpeption(msg);
		}
	}

	public static void RegisterLogOutputListener(ILogOutputListener listener)
	{
		if (!global::Debug._logOutputListeners.Contains(listener))
		{
			global::Debug._logOutputListeners.Add(listener);
		}
		if (global::Debug._logOutputListeners.Count != 0)
		{
			Application.RegisterLogCallback(new Application.LogCallback(global::Debug.LogCallback));
		}
	}

	public static void UnregisterLogOutputListener(ILogOutputListener listener)
	{
		global::Debug._logOutputListeners.Remove(listener);
		if (global::Debug._logOutputListeners.Count == 0)
		{
			Application.RegisterLogCallback(null);
		}
	}

	public static void UnregisterAllLogOutputListener()
	{
		global::Debug._logOutputListeners.Clear();
		Application.RegisterLogCallback(null);
	}

	private static void LogFormat(global::Debug.LogLevel logLevel, string format, params object[] msg)
	{
		if (!global::Debug.IsLogLevelEnabled(logLevel))
		{
			return;
		}
		global::Debug.bd.Remove(0, global::Debug.bd.Length);
		global::Debug.bd.AppendFormat(format, msg);
		global::Debug.Log(logLevel, global::Debug.bd.ToString());
	}

	private static void LogObject(global::Debug.LogLevel logLevel, params object[] msg)
	{
		if (!global::Debug.IsLogLevelEnabled(logLevel))
		{
			return;
		}
		global::Debug.bd.Remove(0, global::Debug.bd.Length);
		for (int i = 0; i < msg.Length; i++)
		{
			object obj = msg[i];
			global::Debug.bd.AppendFormat(" {0}", (obj != null) ? obj.ToString() : "null");
		}
		global::Debug.Log(logLevel, global::Debug.bd.ToString());
	}

	private static void Log(global::Debug.LogLevel logLevel, string msg)
	{
		if (global::Debug.IsLogLevelEnabled(logLevel))
		{
			global::Debug.bd.Remove(0, global::Debug.bd.Length);
			global::Debug.bd.AppendFormat("[{2}] [{1}] {0}", msg, logLevel, global::Debug.GetCurrTimeStamp());
			string text = global::Debug.bd.ToString();
			switch (logLevel)
			{
			case global::Debug.LogLevel.Trace:
			case global::Debug.LogLevel.Log:
				UnityEngine.Debug.Log(text);
				break;
			case global::Debug.LogLevel.Warning:
				UnityEngine.Debug.LogWarning(text);
				break;
			case global::Debug.LogLevel.Error:
				UnityEngine.Debug.LogError(text);
				break;
			case global::Debug.LogLevel.Fatal:
				UnityEngine.Debug.LogError(text);
				UnityEngine.Debug.Break();
				break;
			case global::Debug.LogLevel.Exception:
				UnityEngine.Debug.LogError(text);
				break;
			}
			if (global::Debug.sm_logFileWriteStream != null)
			{
				global::Debug.sm_logFileWriteStream.WriteLine(text);
				global::Debug.sm_logFileWriteStream.Flush();
			}
		}
	}

	private static long GetCurrTimeStamp()
	{
		return (DateTime.UtcNow.Ticks - 621355968000000000L) / 10000L;
	}

	private static void LogCallback(string condition, string stackTrace, LogType type)
	{
		foreach (ILogOutputListener current in global::Debug._logOutputListeners)
		{
			current.OnLog(condition, stackTrace, type);
		}
	}
}
