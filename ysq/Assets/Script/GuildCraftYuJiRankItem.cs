using System;
using UnityEngine;

public class GuildCraftYuJiRankItem : UICustomGridItem
{
	private UISprite mBg;

	private UISprite mRankSp;

	private UILabel mRankTxt;

	private UISprite mOutLine;

	private UILabel mKuangShiNum;

	private UILabel mGuildName;

	public GuildCraftYuJiRankData mRecordData
	{
		get;
		private set;
	}

	public void InitWithBaseScene()
	{
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mBg = base.transform.GetComponent<UISprite>();
		this.mRankSp = base.transform.Find("rankSprite").GetComponent<UISprite>();
		this.mRankTxt = base.transform.Find("rankTxt").GetComponent<UILabel>();
		this.mOutLine = base.transform.Find("outLine").GetComponent<UISprite>();
		this.mKuangShiNum = base.transform.Find("kuangShiNum").GetComponent<UILabel>();
		this.mGuildName = base.transform.Find("name").GetComponent<UILabel>();
	}

	public override void Refresh(object data)
	{
		if (this.mRecordData == data)
		{
			return;
		}
		this.mRecordData = (GuildCraftYuJiRankData)data;
		this.Refresh();
	}

	private void Refresh()
	{
		if (this.mRecordData == null)
		{
			return;
		}
		if (Globals.Instance.Player.GuildSystem.Guild != null && this.mRecordData.mRankData.GuildID == Globals.Instance.Player.GuildSystem.Guild.ID)
		{
			this.mBg.spriteName = "Retroactive_bg";
			this.mBg.color = Tools.GetBillboardSelfBgColor();
		}
		else
		{
			this.mBg.spriteName = "teamBagBg";
			this.mBg.color = Color.white;
		}
		if (this.mRecordData.mRankNum <= 3)
		{
			this.mRankTxt.gameObject.SetActive(false);
			this.mRankSp.gameObject.SetActive(true);
			if (this.mRecordData.mRankNum == 1)
			{
				this.mRankSp.spriteName = "First";
			}
			else if (this.mRecordData.mRankNum == 2)
			{
				this.mRankSp.spriteName = "Second";
			}
			else if (this.mRecordData.mRankNum == 3)
			{
				this.mRankSp.spriteName = "Third";
			}
			this.mOutLine.gameObject.SetActive(true);
			this.mOutLine.spriteName = string.Format("{0}_bg", this.mRankSp.spriteName);
		}
		else
		{
			this.mRankTxt.gameObject.SetActive(true);
			this.mRankSp.gameObject.SetActive(false);
			this.mOutLine.gameObject.SetActive(false);
			this.mRankTxt.text = this.mRecordData.mRankNum.ToString();
		}
		this.mGuildName.text = this.mRecordData.mRankData.GuildName;
		this.mKuangShiNum.text = this.mRecordData.mRankData.ore.ToString();
	}
}
