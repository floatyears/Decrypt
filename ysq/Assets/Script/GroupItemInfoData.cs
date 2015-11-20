using System;

public class GroupItemInfoData : BaseData
{
	public int StartNum;

	public int EndNum;

	public GroupItemInfoData(int start, int end)
	{
		this.StartNum = start;
		this.EndNum = end;
	}

	public override ulong GetID()
	{
		return (ulong)((long)this.StartNum);
	}
}
