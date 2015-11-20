using Proto;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GUILuckyDrawRulePopUp : GameUIBasePopup
{
	private UILabel mTitle;

	private UITable mContentsTable;

	private UILabel mRuleContent;

	private List<RuleInfoPetItem> mRewardItems = new List<RuleInfoPetItem>();

	private void Awake()
	{
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		GameObject gameObject = base.transform.Find("winBG").gameObject;
		GameObject gameObject2 = gameObject.transform.Find("closeBtn").gameObject;
		UIEventListener expr_32 = UIEventListener.Get(gameObject2);
		expr_32.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_32.onClick, new UIEventListener.VoidDelegate(this.OnCloseClick));
		this.mTitle = gameObject.transform.Find("title").GetComponent<UILabel>();
		this.mContentsTable = gameObject.transform.Find("contentsPanel/contents").gameObject.AddComponent<UITable>();
		this.mContentsTable.columns = 1;
		this.mContentsTable.direction = UITable.Direction.Down;
		this.mContentsTable.sorting = UITable.Sorting.None;
		this.mContentsTable.hideInactive = true;
		this.mContentsTable.keepWithinPanel = true;
		this.mContentsTable.padding = new Vector2(0f, 5f);
		this.mRuleContent = GameUITools.FindUILabel("rule", this.mContentsTable.gameObject);
	}

	private void OnCloseClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_002");
		GameUIPopupManager.GetInstance().PopState(false, null);
	}

	public override void InitPopUp(string rules, List<RewardData> datas)
	{
		this.mTitle.text = Singleton<StringManager>.Instance.GetString("activityLuckyDrawRule");
		this.mRuleContent.text = rules;
		if (datas == null)
		{
			return;
		}
		int num = 0;
		RewardData rewardData = null;
		int i;
		for (i = 0; i < datas.Count; i++)
		{
			if (datas[i] != null)
			{
				if (rewardData == null)
				{
					rewardData = datas[i];
					num = i + 1;
				}
				else if (rewardData.RewardType != datas[i].RewardType || rewardData.RewardValue1 != datas[i].RewardValue1 || rewardData.RewardValue2 != datas[i].RewardValue2)
				{
					this.mRewardItems.Add(Tools.InstantiateGUIPrefab("GUI/RuleInfoLine").AddComponent<RuleInfoPetItem>());
					GameUITools.AddChild(this.mContentsTable.gameObject, this.mRewardItems[this.mRewardItems.Count - 1].gameObject);
					this.mRewardItems[this.mRewardItems.Count - 1].InitWithBaseScene(this, (num != i) ? Singleton<StringManager>.Instance.GetString("activityLuckyDrawRuleRankNum", new object[]
					{
						num,
						i
					}) : num.ToString(), rewardData);
					rewardData = datas[i];
					num = i + 1;
				}
			}
		}
		if (rewardData != null)
		{
			this.mRewardItems.Add(Tools.InstantiateGUIPrefab("GUI/RuleInfoLine").AddComponent<RuleInfoPetItem>());
			GameUITools.AddChild(this.mContentsTable.gameObject, this.mRewardItems[this.mRewardItems.Count - 1].gameObject);
			this.mRewardItems[this.mRewardItems.Count - 1].InitWithBaseScene(this, (num != i) ? Singleton<StringManager>.Instance.GetString("activityLuckyDrawRuleRankNum", new object[]
			{
				num,
				i
			}) : num.ToString(), rewardData);
		}
		this.mContentsTable.repositionNow = true;
	}

	public GameObject RefreshRewardItem(Transform parent, RewardData data, float x, float y)
	{
		if (data == null)
		{
			return null;
		}
		GameObject gameObject = GameUITools.CreateMinReward(data.RewardType, data.RewardValue1, data.RewardValue2, parent);
		if (gameObject != null)
		{
			if (data.RewardType == 3 || data.RewardType == 4)
			{
				gameObject.transform.localPosition = new Vector3(x, y, 0f);
			}
			else
			{
				gameObject.transform.localPosition = new Vector3(x + 2f, y, 0f);
			}
		}
		else
		{
			global::Debug.LogError(new object[]
			{
				"Create reward error."
			});
		}
		return gameObject;
	}
}
