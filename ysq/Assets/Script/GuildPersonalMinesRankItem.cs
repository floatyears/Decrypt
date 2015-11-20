using Proto;
using System;
using UnityEngine;

public class GuildPersonalMinesRankItem : CommonRankItemBase
{
	private RankData mRankData;

	public override void Refresh(object data)
	{
		if (this.mUserData == data)
		{
			return;
		}
		this.mUserData = (BillboardInfoData)data;
		this.mRankData = (RankData)this.mUserData.userData;
		this.Refresh();
	}

	private ulong GetPlayerID()
	{
		return this.mRankData.Data.GUID;
	}

	public override int GetRank()
	{
		return this.mRankData.Rank;
	}

	public override string GetLvlName()
	{
		return string.Format("Lv{0} {1}", this.mRankData.Data.Level, this.mRankData.Data.Name);
	}

	public override string GetScore()
	{
		return string.Format("{0}{1}", Singleton<StringManager>.Instance.GetString("guildMines14"), this.mRankData.Value);
	}

	public override void Refresh()
	{
		base.Refresh();
		this.mGoldNum.gameObject.SetActive(false);
		this.mDiamondNum.gameObject.SetActive(false);
		if (this.GetPlayerID() == Globals.Instance.Player.Data.ID)
		{
			this.mBg.spriteName = "Retroactive_bg";
			this.mBg.color = Tools.GetBillboardSelfBgColor();
		}
		else
		{
			this.mBg.spriteName = "teamBagBg";
			this.mBg.color = Color.white;
		}
	}
}
