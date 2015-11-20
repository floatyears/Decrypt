using Att;
using Proto;
using System;
using UnityEngine;

public class WorldBossRankItem : CommonRankItemBase
{
	protected RankData mRankData;

	protected WorldBossInfo wbInfo;

	public override void Refresh(object data)
	{
		if (this.mUserData == data)
		{
			return;
		}
		this.mUserData = (BillboardInfoData)data;
		this.mRankData = (RankData)this.mUserData.userData;
		int rank = this.GetRank();
		foreach (WorldBossInfo current in Globals.Instance.AttDB.WorldBossDict.Values)
		{
			if (current != null && current.LowRank <= rank && (rank <= current.HighRank || current.HighRank == 0))
			{
				this.wbInfo = current;
				break;
			}
		}
		this.Refresh();
	}

	private ulong GetPlayerID()
	{
		return this.mRankData.Data.GUID;
	}

	public override int GetMoney()
	{
		return (this.wbInfo.RewardType[0] != 1) ? 0 : this.wbInfo.RewardValue1[0];
	}

	public override int GetDiamond()
	{
		return (this.wbInfo.RewardType[1] != 2) ? 0 : this.wbInfo.RewardValue1[1];
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
		return string.Format("{0}{1}", Singleton<StringManager>.Instance.GetString("worldBossTxt3"), Tools.FormatValue(this.mRankData.Value));
	}

	public override void Refresh()
	{
		base.Refresh();
		int money = this.GetMoney();
		if (money > 0)
		{
			this.mGoldNum.text = money.ToString();
			this.mGoldIcon.spriteName = "Gold_1";
			this.mGoldNum.transform.localPosition = new Vector3(this.mGoldNum.transform.localPosition.x, 30f, 0f);
			this.mGoldNum.gameObject.SetActive(true);
		}
		else
		{
			this.mGoldNum.gameObject.SetActive(false);
		}
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
