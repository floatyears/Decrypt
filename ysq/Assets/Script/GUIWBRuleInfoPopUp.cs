using Att;
using System;
using UnityEngine;

public class GUIWBRuleInfoPopUp : GameUIBasePopup
{
	private UITable mContentsTable;

	private UILabel mC1;

	private GUIWBRuleInfoRewardItem[] mRewardItems = new GUIWBRuleInfoRewardItem[13];

	public static void ShowMe()
	{
		GameUIPopupManager.GetInstance().PushState(GameUIPopupManager.eSTATE.GUIWBRuleInfoPopUp, false, null, null);
	}

	private void Awake()
	{
		this.CreateObjects();
		this.Refresh();
	}

	private void CreateObjects()
	{
		GameObject gameObject = base.transform.Find("winBG").gameObject;
		GameObject gameObject2 = gameObject.transform.Find("closeBtn").gameObject;
		UIEventListener expr_32 = UIEventListener.Get(gameObject2);
		expr_32.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_32.onClick, new UIEventListener.VoidDelegate(this.OnCloseClick));
		this.mContentsTable = gameObject.transform.Find("contentsBg/contentsPanel/contents").GetComponent<UITable>();
		this.mContentsTable.sorting = UITable.Sorting.None;
		this.mC1 = this.mContentsTable.transform.Find("c1").GetComponent<UILabel>();
		for (int i = 0; i < this.mRewardItems.Length; i++)
		{
			this.mRewardItems[i] = this.mContentsTable.transform.Find(string.Format("ruleInfoLine{0}", i)).gameObject.AddComponent<GUIWBRuleInfoRewardItem>();
			this.mRewardItems[i].InitWithBaseScene();
		}
	}

	private void OnCloseClick(GameObject go)
	{
		GameUIPopupManager.GetInstance().PopState(false, null);
	}

	private void Refresh()
	{
		this.mC1.text = Singleton<StringManager>.Instance.GetString("worldBossTxt20");
		int i = 0;
		foreach (WorldBossInfo current in Globals.Instance.AttDB.WorldBossDict.Values)
		{
			if (current != null)
			{
				this.mRewardItems[i].Refresh(current);
				i++;
			}
		}
		while (i < 13)
		{
			this.mRewardItems[i].gameObject.SetActive(false);
			i++;
		}
	}
}
