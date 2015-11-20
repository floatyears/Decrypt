using Att;
using Proto;
using System;
using System.Collections.Generic;

public class GuildBoss
{
	public GuildBossData Data
	{
		get;
		private set;
	}

	public MonsterInfo Info
	{
		get;
		private set;
	}

	public List<RankData> Damage
	{
		get;
		private set;
	}

	public GuildBoss(GuildBossData data)
	{
		this.Data = data;
		this.ResetInfo();
	}

	public void ResetInfo()
	{
		if (this.Info == null || this.Data.InfoID != this.Info.ID)
		{
			this.Info = Globals.Instance.AttDB.MonsterDict.GetInfo(this.Data.InfoID);
			if (this.Info == null)
			{
				Debug.LogError(new object[]
				{
					string.Format("MonsterDict.GetInfo error, ID = {0}", this.Data.InfoID)
				});
			}
		}
	}

	public void SetDamageRankData(List<RankData> data)
	{
		this.Damage = data;
	}

	public RankData GetDamageRankData(int index)
	{
		if (this.Damage != null && index < this.Damage.Count)
		{
			return this.Damage[index];
		}
		return null;
	}
}
