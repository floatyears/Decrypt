using Proto;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GUIGuildCraftYuJiPopUp : GameUIBasePopup
{
	private List<GuildWarRankData> mRankDatas;

	private UILabel mGuildRank;

	private UILabel mGuildName;

	private UILabel mKuangShiNum;

	private UILabel mTipTxt;

	private GuildCraftYuJiRankTable mRankTable;

	public static void ShowMe(List<GuildWarRankData> rDatas)
	{
		GameUIPopupManager.GetInstance().PushState(GameUIPopupManager.eSTATE.GUIGuildCraftYuJiPopUp, false, null, null);
		GUIGuildCraftYuJiPopUp gUIGuildCraftYuJiPopUp = (GUIGuildCraftYuJiPopUp)GameUIPopupManager.GetInstance().GetCurrentPopup();
		if (gUIGuildCraftYuJiPopUp != null)
		{
			gUIGuildCraftYuJiPopUp.InitPopUp(rDatas);
		}
	}

	private void Awake()
	{
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		Transform transform = base.transform.Find("winBg");
		GameObject gameObject = transform.transform.Find("closeBtn").gameObject;
		UIEventListener expr_2D = UIEventListener.Get(gameObject);
		expr_2D.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_2D.onClick, new UIEventListener.VoidDelegate(this.OnCloseClick));
		this.mGuildRank = transform.Find("txt0/num").GetComponent<UILabel>();
		this.mGuildName = transform.Find("txt1").GetComponent<UILabel>();
		this.mKuangShiNum = this.mGuildName.transform.Find("num").GetComponent<UILabel>();
		this.mTipTxt = transform.Find("tipTxt").GetComponent<UILabel>();
		this.mRankTable = transform.transform.Find("bg/contentsPanel/contents").gameObject.AddComponent<GuildCraftYuJiRankTable>();
		this.mRankTable.maxPerLine = 1;
		this.mRankTable.arrangement = UICustomGrid.Arrangement.Vertical;
		this.mRankTable.cellWidth = 626f;
		this.mRankTable.cellHeight = 88f;
	}

	public void InitPopUp(List<GuildWarRankData> rDatas)
	{
		this.mRankDatas = rDatas;
		this.Refresh();
	}

	private void OnCloseClick(GameObject go)
	{
		GameUIPopupManager.GetInstance().PopState(false, null);
	}

	private int GetSelfRank()
	{
		if (Globals.Instance.Player.GuildSystem.Guild == null)
		{
			return 0;
		}
		for (int i = 0; i < this.mRankDatas.Count; i++)
		{
			if (this.mRankDatas[i].GuildID == Globals.Instance.Player.GuildSystem.Guild.ID)
			{
				return i + 1;
			}
		}
		return 0;
	}

	private void Refresh()
	{
		if (this.mRankDatas == null)
		{
			return;
		}
		int selfRank = this.GetSelfRank();
		this.mGuildRank.text = ((selfRank != 0) ? selfRank.ToString() : Singleton<StringManager>.Instance.GetString("Billboard0"));
		if (Globals.Instance.Player.GuildSystem.HasGuild())
		{
			this.mGuildName.text = Globals.Instance.Player.GuildSystem.Guild.Name;
			this.mKuangShiNum.text = Globals.Instance.Player.GuildSystem.Guild.Ore.ToString();
		}
		else
		{
			this.mGuildName.text = Singleton<StringManager>.Instance.GetString("guildCraft7");
			this.mKuangShiNum.text = "0";
		}
		this.mTipTxt.text = Singleton<StringManager>.Instance.GetString("guildCraft6");
		this.mRankTable.ClearData();
		for (int i = 0; i < this.mRankDatas.Count; i++)
		{
			this.mRankTable.AddData(new GuildCraftYuJiRankData(this.mRankDatas[i], i + 1));
		}
	}
}
