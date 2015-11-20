using Att;
using System;

public class GUILongLinRewardData : BaseData
{
	public FDSInfo mFDSInfo
	{
		get;
		private set;
	}

	public WorldRespawnInfo mWorldRespawnInfo
	{
		get;
		private set;
	}

	public GUILongLinRewardData(FDSInfo fInfo, WorldRespawnInfo wInfo)
	{
		this.mFDSInfo = fInfo;
		this.mWorldRespawnInfo = wInfo;
	}

	public bool IsCanTaken()
	{
		WorldBossSubSystem worldBossSystem = Globals.Instance.Player.WorldBossSystem;
		if (worldBossSystem != null)
		{
			if (this.mFDSInfo != null)
			{
				if (!worldBossSystem.IsFDSRewardTaken(this.mFDSInfo.ID))
				{
					return this.mFDSInfo.FireDragonScale <= Globals.Instance.Player.Data.FireDragonScale;
				}
			}
			else if (this.mWorldRespawnInfo != null)
			{
				return worldBossSystem.IsWBRewrdCanTaken(this.mWorldRespawnInfo.ID);
			}
		}
		return false;
	}

	public bool IsRewardTakened()
	{
		WorldBossSubSystem worldBossSystem = Globals.Instance.Player.WorldBossSystem;
		if (worldBossSystem != null && this.mFDSInfo != null)
		{
			if (this.mFDSInfo != null)
			{
				return worldBossSystem.IsFDSRewardTaken(this.mFDSInfo.ID);
			}
			if (this.mWorldRespawnInfo != null)
			{
				return worldBossSystem.IsWBRewardTaken(this.mWorldRespawnInfo.ID);
			}
		}
		return false;
	}

	public override ulong GetID()
	{
		if (this.mFDSInfo != null)
		{
			return (ulong)((long)this.mFDSInfo.ID);
		}
		if (this.mWorldRespawnInfo != null)
		{
			return (ulong)((long)this.mWorldRespawnInfo.ID);
		}
		return 0uL;
	}
}
