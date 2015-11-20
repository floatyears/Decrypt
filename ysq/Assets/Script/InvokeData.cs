using System;
using UnityEngine;

public class InvokeData
{
	public Component cmp;

	public string funcName;

	public float delay;

	public InvokeData(Component _cmp, string _funcName, float _delay)
	{
		this.cmp = _cmp;
		this.funcName = _funcName;
		this.delay = _delay;
	}

	public void Invoke()
	{
		if (this.cmp != null)
		{
			this.cmp.SendMessage(this.funcName);
			this.cmp = null;
		}
	}
}
