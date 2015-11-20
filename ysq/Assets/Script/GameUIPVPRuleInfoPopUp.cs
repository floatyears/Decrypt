using Att;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GameUIPVPRuleInfoPopUp : GameUIBasePopup
{
	private UILabel mTitle;

	private UITable mContentsTable;

	private UILabel mC1;

	private RuleInfoRewardItem mC2;

	private GameObject mC3;

	private UILabel mSelfRank;

	private UILabel mC4;

	private RuleInfoRewardItem[] mRewardItems = new RuleInfoRewardItem[14];

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
		this.mContentsTable = gameObject.transform.Find("contentsBg/contentsPanel/contents").GetComponent<UITable>();
		this.mContentsTable.sorting = UITable.Sorting.None;
		this.mC1 = this.mContentsTable.transform.Find("c1").GetComponent<UILabel>();
		this.mC2 = this.mContentsTable.transform.Find("c2").gameObject.AddComponent<RuleInfoRewardItem>();
		this.mC2.Init();
		this.mC3 = this.mContentsTable.transform.Find("c3").gameObject;
		this.mSelfRank = this.mC3.transform.Find("rank").GetComponent<UILabel>();
		this.mC4 = this.mContentsTable.transform.Find("c4").GetComponent<UILabel>();
		for (int i = 0; i < this.mRewardItems.Length; i++)
		{
			this.mRewardItems[i] = this.mContentsTable.transform.Find(string.Format("ruleInfoLine{0}", i)).gameObject.AddComponent<RuleInfoRewardItem>();
			this.mRewardItems[i].Init();
		}
	}

	private void OnCloseClick(GameObject go)
	{
		GameUIPopupManager.GetInstance().PopState(false, null);
	}

	private int SortPvpInfo(PvpInfo info1, PvpInfo info2)
	{
		return info1.ID.CompareTo(info2.ID);
	}

	public override void InitPopUp()
	{
		LocalPlayer player = Globals.Instance.Player;
		this.mTitle.text = Singleton<StringManager>.Instance.GetString("pvp4Txt13");
		if (player.Data.ArenaHighestRank > 0 && Globals.Instance.Player.PvpSystem.ShowSelfRank())
		{
			this.mSelfRank.text = Singleton<StringManager>.Instance.GetString("pvp4Txt19", new object[]
			{
				player.Data.ArenaHighestRank
			});
			this.mC3.gameObject.SetActive(true);
		}
		else
		{
			this.mC3.gameObject.SetActive(false);
		}
		List<PvpInfo> list = new List<PvpInfo>();
		list.AddRange(Globals.Instance.AttDB.PvpDict.Values);
		list.Sort(new Comparison<PvpInfo>(this.SortPvpInfo));
		int rank = player.PvpSystem.Rank;
		if (rank <= 0 || !Globals.Instance.Player.PvpSystem.ShowSelfRank())
		{
			this.mC1.gameObject.SetActive(false);
			this.mC2.gameObject.SetActive(false);
		}
		else
		{
			bool flag = false;
			for (int i = 0; i < list.Count; i++)
			{
				PvpInfo pvpInfo = list[i];
				if (pvpInfo.ArenaHighRank <= rank && rank <= pvpInfo.ArenaLowRank)
				{
					this.mC1.text = ((pvpInfo.ArenaHighRank != pvpInfo.ArenaLowRank) ? Singleton<StringManager>.Instance.GetString("pvp4Txt15", new object[]
					{
						pvpInfo.ArenaHighRank,
						pvpInfo.ArenaLowRank
					}) : Singleton<StringManager>.Instance.GetString("pvp4Txt16", new object[]
					{
						pvpInfo.ArenaHighRank
					}));
					this.mC2.Refresh(pvpInfo);
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				this.mC1.text = Singleton<StringManager>.Instance.GetString("pvp4Txt17", new object[]
				{
					player.PvpSystem.Rank,
					list[list.Count - 1].ArenaLowRank
				});
				this.mC2.Refresh(list[list.Count - 1]);
			}
			this.mC1.gameObject.SetActive(true);
			this.mC2.gameObject.SetActive(true);
		}
		this.mC4.text = Singleton<StringManager>.Instance.GetString("pvp4Txt14");
		for (int j = 0; j < this.mRewardItems.Length; j++)
		{
			PvpInfo info = list[j];
			this.mRewardItems[j].Refresh(info);
		}
		this.mContentsTable.repositionNow = true;
	}
}
