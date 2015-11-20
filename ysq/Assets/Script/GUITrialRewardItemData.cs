using Att;
using System;

public class GUITrialRewardItemData : BaseData
{
	public ERewardType mERewardType;

	public ItemInfo mItemInfo;

	public int mItemNum;

	public int mDaiBiNum;

	public GUITrialRewardItemData(ERewardType et, ItemInfo iInfo, int num, int dNum)
	{
		this.mERewardType = et;
		this.mItemInfo = iInfo;
		this.mItemNum = num;
		this.mDaiBiNum = dNum;
	}

	public override ulong GetID()
	{
		return (ulong)((this.mItemInfo == null) ? ((long)this.mDaiBiNum) : ((long)this.mItemInfo.ID));
	}
}
