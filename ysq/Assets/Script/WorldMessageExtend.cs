using System;

public class WorldMessageExtend
{
	public WorldMessage mWM;

	public bool mIsSystem;

	public WorldMessageExtend()
	{
	}

	public WorldMessageExtend(WorldMessage wM, bool issystem)
	{
		this.mWM = wM;
		this.mIsSystem = issystem;
	}
}
