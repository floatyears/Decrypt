using Proto;
using System;
using UnityEngine;

public class GuildWeekMinesRankItem : CommonRankItemBase
{
	private GuildRank mGuildRankData;

	public override void Refresh(object data)
	{
		if (this.mUserData == data)
		{
			return;
		}
		this.mUserData = (BillboardInfoData)data;
		this.mGuildRankData = (GuildRank)this.mUserData.userData;
		this.Refresh();
	}

	public override int GetRank()
	{
		return Globals.Instance.Player.BillboardSystem.GetGGOreRank(this.mGuildRankData);
	}

	public override string GetLvlName()
	{
		return string.Format("Lv{0} {1}", this.mGuildRankData.Level, this.mGuildRankData.Name);
	}

	public override string GetScore()
	{
		return string.Format("{0}{1}", Singleton<StringManager>.Instance.GetString("guildMines13"), this.mGuildRankData.Value);
	}

	public override void Refresh()
	{
		base.Refresh();
		this.mRankIcon.gameObject.SetActive(false);
		this.mLvlName.transform.localPosition = new Vector3(this.mLvlName.transform.localPosition.x - 270f - (float)this.nameXValue, this.mLvlName.transform.localPosition.y, this.mLvlName.transform.localPosition.z);
		this.mScore.transform.localPosition = new Vector3(this.mScore.transform.localPosition.x - 270f - (float)this.nameXValue, this.mScore.transform.localPosition.y, this.mScore.transform.localPosition.z);
		this.mGoldNum.gameObject.SetActive(false);
		if (Globals.Instance.Player.BillboardSystem.GGOreRank > 0u && (long)Globals.Instance.Player.BillboardSystem.GetGGOreRank(this.mGuildRankData) == (long)((ulong)Globals.Instance.Player.BillboardSystem.GGOreRank))
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
