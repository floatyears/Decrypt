using System;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

namespace com.tencent.bugly.unity3d
{
	public abstract class ExceptionHandler
	{
		public LogSeverity _logLevel = LogSeverity.Exception;

		private bool _LogCallbackRegister = false;

		public abstract void OnUncaughtExceptionReport(string type, string message, string stack);

		public void OnExceptionCaught(Exception e)
		{
			this.OnExceptionHandle(e);
		}

		public void UnregisterExceptionHandler()
		{
			Application.RegisterLogCallback(null);
		}

		public void RegisterExceptionHandler()
		{
			if (!this._LogCallbackRegister)
			{
				this._LogCallbackRegister = true;
				AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(this.OnUncaughtExceptionHandler);
				Application.RegisterLogCallback(new Application.LogCallback(this.OnLogCallback));
			}
		}

		private void OnUncaughtExceptionHandler(object sender, UnhandledExceptionEventArgs args)
		{
			if (args != null && args.ExceptionObject != null)
			{
				if (args.ExceptionObject.GetType() == typeof(Exception))
				{
					this.OnExceptionHandle((Exception)args.ExceptionObject);
				}
			}
		}

		private void OnLogCallback(string log, string stackTrace, LogType type)
		{
			LogSeverity logSeverity = LogSeverity.Exception;
			switch (type)
			{
			case LogType.Error:
				logSeverity = LogSeverity.Error;
				break;
			case LogType.Assert:
				logSeverity = LogSeverity.Assert;
				break;
			case LogType.Warning:
				logSeverity = LogSeverity.Warning;
				break;
			case LogType.Log:
				logSeverity = LogSeverity.Log;
				break;
			case LogType.Exception:
				logSeverity = LogSeverity.Exception;
				break;
			}
			if (logSeverity >= this._logLevel)
			{
				Debugger.Error(string.Format("{0}\n{1}", log, stackTrace));
				string message = null;
				Regex regex = new Regex("^(?<errorClass>\\S+):\\s*(?<message>.*)");
				Match match = regex.Match(log);
				string type2;
				if (match.Success)
				{
					type2 = match.Groups["errorClass"].Value;
					message = match.Groups["message"].Value.Trim();
				}
				else
				{
					type2 = log;
				}
				if (stackTrace != null)
				{
					try
					{
						string[] array = stackTrace.Split(new char[]
						{
							'\n'
						});
						if (array != null && array.Length > 0)
						{
							StringBuilder stringBuilder = new StringBuilder("");
							string[] array2 = array;
							for (int i = 0; i < array2.Length; i++)
							{
								string text = array2[i];
								int num = text.ToLower().IndexOf("(at");
								int num2 = text.ToLower().IndexOf("/assets/");
								if (num > 0 && num2 > 0)
								{
									stringBuilder.AppendFormat("{0}(at {1}\n", text.Substring(0, num), text.Substring(num2));
								}
							}
							if (stringBuilder.Length > 0)
							{
								stackTrace = stringBuilder.ToString();
							}
						}
					}
					catch (Exception ex)
					{
						Debugger.Error(string.Format("SDK occur exception: {0}\n when parse the stack trace : {1}", ex.ToString(), stackTrace));
					}
				}
				this.OnUncaughtExceptionReport(type2, message, stackTrace);
			}
		}

		private void OnExceptionHandle(Exception e)
		{
			if (e != null)
			{
				StackTrace stackTrace = new StackTrace(e, true);
				int frameCount = stackTrace.FrameCount;
				StringBuilder stringBuilder = new StringBuilder("");
				for (int i = 0; i < frameCount; i++)
				{
					StackFrame frame = stackTrace.GetFrame(i);
					stringBuilder.AppendFormat("{0}.{1}", frame.GetMethod().DeclaringType.Name, frame.GetMethod().Name);
					ParameterInfo[] parameters = frame.GetMethod().GetParameters();
					if (parameters == null || parameters.Length == 0)
					{
						stringBuilder.Append("()");
					}
					else
					{
						stringBuilder.Append("(");
						int num = parameters.Length;
						for (int j = 0; j < num; j++)
						{
							ParameterInfo parameterInfo = parameters[j];
							stringBuilder.AppendFormat("{0} {1}", parameterInfo.ParameterType.FullName, parameterInfo.Name);
							if (j != num - 1)
							{
								stringBuilder.Append(", ");
							}
						}
						stringBuilder.Append(")");
					}
					string text = frame.GetFileName();
					if (text != null)
					{
						text = text.Replace('\\', '/');
						int num2 = text.ToLower().IndexOf("/assets/");
						if (num2 < 0)
						{
							num2 = text.ToLower().IndexOf("assets/");
						}
						Debugger.Debug(string.Format("location {0} in {1}", num2, text));
						if (num2 > 0)
						{
							text = text.Substring(num2);
						}
					}
					else
					{
						text = "unknown";
					}
					stringBuilder.AppendFormat(" (at {0}:{1})", text, frame.GetFileLineNumber());
					if (i != frameCount - 1)
					{
						stringBuilder.AppendLine();
					}
				}
				this.OnUncaughtExceptionReport(e.GetType().Name, e.Message, stringBuilder.ToString());
			}
		}
	}
}
