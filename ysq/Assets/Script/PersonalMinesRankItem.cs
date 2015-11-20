using Att;
using Proto;
using System;
using UnityEngine;

public class PersonalMinesRankItem : CommonRankItemBase
{
	private RankData mRankData;

	private OreInfo mInfo;

	public override void Refresh(object data)
	{
		if (this.mUserData == data)
		{
			return;
		}
		this.mUserData = (BillboardInfoData)data;
		this.mRankData = (RankData)this.mUserData.userData;
		int rank = this.GetRank();
		foreach (OreInfo current in Globals.Instance.AttDB.OreDict.Values)
		{
			if (current != null && current.DayRankMin <= rank && (rank <= current.DayRankMax || current.DayRankMax == 0))
			{
				this.mInfo = current;
				break;
			}
		}
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

	public override int GetMoney()
	{
		if (this.mInfo.DayRewardType[0] <= 0 || this.mInfo.DayRewardType[0] >= 20)
		{
			return 0;
		}
		return this.mInfo.DayRewardValue1[0];
	}

	public override int GetDiamond()
	{
		if (this.mInfo.DayRewardType[1] <= 0 || this.mInfo.DayRewardType[1] >= 20)
		{
			return 0;
		}
		return this.mInfo.DayRewardValue1[1];
	}

	public override void Refresh()
	{
		base.Refresh();
		int money = this.GetMoney();
		if (money > 0)
		{
			if (this.mInfo.DayRewardType[0] == 3)
			{
				this.mGoldNum.text = this.mInfo.DayRewardValue2[0].ToString();
				this.mGoldIcon.gameObject.SetActive(false);
				base.CreateItemIcon(true);
				this.mGoldItemIcon.Refresh(new RewardData
				{
					RewardType = this.mInfo.DayRewardType[0],
					RewardValue1 = this.mInfo.DayRewardValue1[0],
					RewardValue2 = this.mInfo.DayRewardValue2[0]
				}, false, false, false);
				this.mGoldItemIcon.gameObject.SetActive(true);
			}
			else
			{
				if (this.mGoldItemIcon != null)
				{
					this.mGoldItemIcon.gameObject.SetActive(false);
				}
				this.mGoldNum.text = money.ToString();
				this.mGoldIcon.spriteName = Tools.GetRewardTypeIcon((ERewardType)this.mInfo.DayRewardType[0]);
				this.mGoldIcon.gameObject.SetActive(true);
			}
			this.mGoldNum.transform.localPosition = new Vector3(this.mGoldNum.transform.localPosition.x, 30f, 0f);
			this.mGoldNum.gameObject.SetActive(true);
		}
		else
		{
			this.mGoldNum.gameObject.SetActive(false);
		}
		int diamond = this.GetDiamond();
		if (diamond > 0)
		{
			if (this.mInfo.DayRewardType[1] == 3)
			{
				this.mDiamondNum.text = this.mInfo.DayRewardValue2[1].ToString();
				this.mDiamondIcon.gameObject.SetActive(false);
				base.CreateItemIcon(false);
				this.mDiamondItemIcon.Refresh(new RewardData
				{
					RewardType = this.mInfo.DayRewardType[1],
					RewardValue1 = this.mInfo.DayRewardValue1[1],
					RewardValue2 = this.mInfo.DayRewardValue2[1]
				}, false, false, false);
				this.mDiamondItemIcon.gameObject.SetActive(true);
			}
			else
			{
				if (this.mDiamondItemIcon != null)
				{
					this.mDiamondItemIcon.gameObject.SetActive(false);
				}
				this.mDiamondNum.text = diamond.ToString();
				this.mDiamondIcon.spriteName = Tools.GetRewardTypeIcon((ERewardType)this.mInfo.DayRewardType[1]);
				this.mDiamondIcon.gameObject.SetActive(true);
			}
			this.mDiamondNum.transform.localPosition = new Vector3(this.mDiamondNum.transform.localPosition.x, -7f, 0f);
			this.mDiamondNum.gameObject.SetActive(true);
		}
		else
		{
			this.mDiamondNum.gameObject.SetActive(false);
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
