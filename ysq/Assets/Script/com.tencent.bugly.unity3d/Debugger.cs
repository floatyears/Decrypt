using System;

namespace com.tencent.bugly.unity3d
{
	public static class Debugger
	{
		private static LogSeverity _logLevel = LogSeverity.Warning;

		public static void setDebugLevel(LogSeverity level)
		{
			Debugger._logLevel = level;
		}

		public static bool isLoggable(LogSeverity level)
		{
			return Debugger._logLevel <= level;
		}

		public static void Debug(string message)
		{
			if (Debugger.isLoggable(LogSeverity.Log))
			{
				Console.WriteLine("{0} : [IBugly] {1}", "Debug", message);
			}
		}

		public static void Warn(string message)
		{
			if (Debugger.isLoggable(LogSeverity.Warning))
			{
				Console.WriteLine("{0} : [IBugly] {1}", "Warn", message);
			}
		}

		public static void Info(string message)
		{
			if (Debugger.isLoggable(LogSeverity.Info))
			{
				Console.WriteLine("{0} : [IBugly] {1}", "Info", message);
			}
		}

		public static void Error(string message)
		{
			if (Debugger.isLoggable(LogSeverity.Error))
			{
				Console.WriteLine("{0} : [IBugly] {1}", "Error", message);
			}
		}
	}
}
