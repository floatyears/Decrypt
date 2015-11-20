using Proto;
using System;

public class PVPTargetDaata : BaseData
{
	public RankData RankData
	{
		get;
		private set;
	}

	public bool IsFarmRebot
	{
		get;
		private set;
	}

	public PVPTargetDaata(RankData _data)
	{
		this.RankData = _data;
		this.CheckFarm();
	}

	public override ulong GetID()
	{
		return this.RankData.Data.GUID;
	}

	private void CheckFarm()
	{
		if (Tools.CanPlay(GameConst.GetInt32(123), true) && Globals.Instance.Player.PvpSystem.ArenaTargets[Globals.Instance.Player.PvpSystem.ArenaTargets.Count - 1].Data.GUID == this.RankData.Data.GUID && Tools.IsRebot(this.RankData.Data.GUID))
		{
			this.IsFarmRebot = true;
		}
		else
		{
			this.IsFarmRebot = false;
		}
	}
}
