using System;

public class AssertExpeption : Exception
{
	public AssertExpeption()
	{
	}

	public AssertExpeption(string inMsg) : base(inMsg)
	{
	}
}
