using System;

public class GUIGameNewData
{
	public string MsgContent
	{
		get;
		set;
	}

	public int MoveSpeed
	{
		get;
		set;
	}

	public int Priority
	{
		get;
		set;
	}

	public GUIGameNewData(string msg, int speed, int prio)
	{
		this.MsgContent = msg;
		this.MoveSpeed = speed;
		this.Priority = prio;
	}
}
