using Att;
using System;
using UnityEngine;

public class RewardCurrency : MonoBehaviour
{
	private UISprite iconSprite;

	private UILabel numText;

	public static RewardCurrency CreateReward(ERewardType RewardType, int num)
	{
		GameObject gameObject = Tools.InstantiateGUIPrefab("GUI/QuestRewardMoney");
		if (gameObject != null)
		{
			RewardCurrency rewardCurrency = gameObject.AddComponent<RewardCurrency>();
			if (rewardCurrency != null)
			{
				rewardCurrency.Init(RewardType, num);
				return rewardCurrency;
			}
		}
		return null;
	}

	private void Init(ERewardType RewardType, int num)
	{
		this.iconSprite = base.transform.GetComponent<UISprite>();
		this.numText = base.transform.Find("num").GetComponent<UILabel>();
		this.iconSprite.spriteName = Tools.GetRewardTypeIcon(RewardType);
		if (RewardType == ERewardType.EReward_VipExp)
		{
			this.numText.text = Tools.GetRewardTypeName(RewardType, 0) + "x" + Tools.FormatCurrency(num);
		}
		else
		{
			this.numText.text = "x" + Tools.FormatCurrency(num);
		}
	}
}
