using System;

public class GUITrailBranchItemData : BaseData
{
	private int mStartIndex;

	public int StartIndex
	{
		get
		{
			return this.mStartIndex;
		}
	}

	public GUITrailBranchItemData(int index)
	{
		this.mStartIndex = index;
	}

	public override ulong GetID()
	{
		return (ulong)((long)this.mStartIndex);
	}
}
