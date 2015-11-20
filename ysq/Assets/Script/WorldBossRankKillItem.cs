using Att;
using Proto;
using System;

public class WorldBossRankKillItem : WorldBossRankItem
{
	public override void Refresh(object data)
	{
		if (this.mUserData == data)
		{
			return;
		}
		this.mUserData = (BillboardInfoData)data;
		this.mRankData = (RankData)this.mUserData.userData;
		int rank = this.GetRank();
		if (this.GetSpecialNo() == 0)
		{
			this.wbInfo = Globals.Instance.AttDB.WorldBossDict.GetInfo(5);
		}
		else
		{
			foreach (WorldBossInfo current in Globals.Instance.AttDB.WorldBossDict.Values)
			{
				if (current != null && current.LowRank <= rank && (rank <= current.HighRank || current.HighRank == 0))
				{
					this.wbInfo = current;
					break;
				}
			}
		}
		this.Refresh();
	}

	public override int GetMoney()
	{
		if (this.GetSpecialNo() == 0)
		{
			return (this.wbInfo.KillRewardType[0] != 1) ? 0 : this.wbInfo.KillRewardValue1[0];
		}
		return base.GetMoney();
	}

	public override int GetDiamond()
	{
		if (this.GetSpecialNo() == 0)
		{
			return (this.wbInfo.KillRewardType[1] != 2) ? 0 : this.wbInfo.KillRewardValue1[1];
		}
		return base.GetDiamond();
	}

	protected override string GetRankSprite()
	{
		if (this.GetSpecialNo() == 0)
		{
			this.mRankSprite.width = 76;
			this.mRankSprite.height = 76;
			return "Kill";
		}
		this.mRankSprite.width = 65;
		this.mRankSprite.height = 90;
		return base.GetRankSprite();
	}

	public override int GetSpecialNo()
	{
		int result = 0;
		int.TryParse(this.mUserData.GetID().ToString(), out result);
		return result;
	}
}
