using System;

public class GUIAwakePageItemData : BaseData
{
	public int mPageIndex
	{
		get;
		private set;
	}

	public bool mIsChecked
	{
		get;
		set;
	}

	public GUIAwakePageItemData(int index)
	{
		this.mPageIndex = index;
	}

	public override ulong GetID()
	{
		return (ulong)((long)this.mPageIndex);
	}
}
