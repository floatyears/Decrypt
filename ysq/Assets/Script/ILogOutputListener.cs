using System;
using UnityEngine;

public interface ILogOutputListener
{
	void OnLog(string condition, string stackTrace, LogType type);
}
