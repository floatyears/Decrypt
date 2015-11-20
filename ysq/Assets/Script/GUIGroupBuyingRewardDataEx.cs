using Proto;
using System;

public class GUIGroupBuyingRewardDataEx : BaseData
{
	public ActivityGroupBuyingScoreReward mRewardData;

	public GUIGroupBuyingRewardDataEx(ActivityGroupBuyingScoreReward data)
	{
		this.mRewardData = data;
	}

	public bool IsCanTaken()
	{
		return this.mRewardData.Score <= Globals.Instance.Player.ActivitySystem.GBScore;
	}

	public bool IsTaken()
	{
		return Globals.Instance.Player.ActivitySystem.IsGBScoreRewardTake(this.mRewardData.ID);
	}
}
