using Att;
using System;
using System.Text;
using UnityEngine;

public class RuleInfoRewardItem : MonoBehaviour
{
	private UILabel mRank;

	private UILabel mGemNum;

	private UILabel mHonorNum;

	private StringBuilder mStringBuilder = new StringBuilder();

	public void Init()
	{
		Transform transform = base.transform.Find("Label");
		if (transform != null)
		{
			this.mRank = transform.GetComponent<UILabel>();
		}
		this.mGemNum = base.transform.Find("Gem/num").GetComponent<UILabel>();
		this.mHonorNum = base.transform.Find("Honor/num").GetComponent<UILabel>();
	}

	public void Refresh(PvpInfo info)
	{
		if (info == null)
		{
			base.gameObject.SetActive(false);
			return;
		}
		base.gameObject.SetActive(true);
		if (this.mRank != null)
		{
			if (info.ArenaLowRank == info.ArenaHighRank)
			{
				this.mRank.text = Singleton<StringManager>.Instance.GetString("activityLuckyDrawRuleRank", new object[]
				{
					info.ArenaHighRank
				});
			}
			else
			{
				this.mRank.text = Singleton<StringManager>.Instance.GetString("activityLuckyDrawRuleRank", new object[]
				{
					string.Format("{0}~{1}", info.ArenaHighRank, info.ArenaLowRank)
				});
			}
		}
		this.mStringBuilder.Remove(0, this.mStringBuilder.Length).Append("x").Append(info.ArenaRewardDiamond);
		this.mGemNum.text = this.mStringBuilder.ToString();
		this.mStringBuilder.Remove(0, this.mStringBuilder.Length).Append("x").Append(info.ArenaRewardHonor);
		this.mHonorNum.text = this.mStringBuilder.ToString();
	}
}
