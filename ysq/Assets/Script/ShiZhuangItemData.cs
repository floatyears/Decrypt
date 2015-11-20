using Att;
using System;

public class ShiZhuangItemData : BaseData
{
	public FashionInfo mFashionInfo;

	public bool mIsSelected
	{
		get;
		set;
	}

	public ShiZhuangItemData(FashionInfo fi)
	{
		this.mFashionInfo = fi;
	}

	public override ulong GetID()
	{
		return (ulong)((long)this.mFashionInfo.ID);
	}
}
