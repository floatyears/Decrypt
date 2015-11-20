using Att;
using System;
using UnityEngine;

public class GUIDayEnergyEvent : GameUIBasePopup
{
	private UILabel Content;

	private Transform Reward;

	private GameObject RewardStamina;

	private UISprite Icon;

	private UILabel Stamina;

	private void Awake()
	{
		this.Reward = base.transform.Find("Reward");
		this.Content = base.transform.Find("Content").GetComponent<UILabel>();
		this.RewardStamina = this.Reward.Find("RewardStamina").gameObject;
		this.Stamina = this.RewardStamina.transform.Find("num").GetComponent<UILabel>();
		this.Icon = this.RewardStamina.transform.Find("Stamina").GetComponent<UISprite>();
	}

	public override void InitPopUp(int type, int value)
	{
		base.InitPopUp();
		this.Content.text = Singleton<StringManager>.Instance.GetString(string.Format("EDE_{0}", type), new object[]
		{
			value
		});
		if (this.Reward != null)
		{
			ERewardType eRewardType = ERewardType.EReward_Null;
			switch (type)
			{
			case 1:
				eRewardType = ERewardType.EReward_Diamond;
				break;
			case 2:
				eRewardType = ERewardType.EReward_Money;
				break;
			case 3:
				this.RewardStamina.gameObject.SetActive(true);
				this.Stamina.text = value.ToString();
				this.Icon.spriteName = "key_1";
				break;
			case 4:
				this.RewardStamina.gameObject.SetActive(true);
				this.Stamina.text = value.ToString();
				this.Icon.spriteName = "heart";
				break;
			}
			if (eRewardType != ERewardType.EReward_Null)
			{
				this.RewardStamina.gameObject.SetActive(false);
				GameUITools.CreateReward((int)eRewardType, value, value, this.Reward, true, false, 36f, -7f, -2000f, 20f, 13f, 7f, 0);
			}
		}
	}
}
