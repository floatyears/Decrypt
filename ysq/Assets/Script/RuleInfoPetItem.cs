using Proto;
using System;
using UnityEngine;

public class RuleInfoPetItem : MonoBehaviour
{
	private GUILuckyDrawRulePopUp mBaseScene;

	private UILabel mRank;

	private GameObject mInfo;

	public void InitWithBaseScene(GUILuckyDrawRulePopUp basescene, string rank, RewardData data)
	{
		if (data == null)
		{
			return;
		}
		this.CreateObjects();
		this.mBaseScene = basescene;
		this.mRank.text = Singleton<StringManager>.Instance.GetString("activityLuckyDrawRuleRank", new object[]
		{
			rank
		});
		this.mBaseScene.RefreshRewardItem(this.mInfo.transform, data, 0f, 0f);
	}

	private void CreateObjects()
	{
		this.mRank = GameUITools.FindUILabel("Rank", base.gameObject);
		this.mInfo = GameUITools.FindGameObject("Info", base.gameObject);
	}
}
