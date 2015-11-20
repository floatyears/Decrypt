using Att;
using System;

public class GUIGuildMinesRewardDescData : BaseData
{
	public delegate void TakeRewardCallback(GUIGuildMinesRewardDescData data);

	public GUIGuildMinesRewardDescData.TakeRewardCallback TakeRewardEvent;

	public OreInfo mInfo;

	public bool isTarget;

	public GUIGuildMinesRewardDescData(bool isTarget, OreInfo info, GUIGuildMinesRewardDescData.TakeRewardCallback cb)
	{
		this.isTarget = isTarget;
		this.mInfo = info;
		this.TakeRewardEvent = cb;
	}

	public override ulong GetID()
	{
		return (ulong)((long)this.mInfo.ID);
	}

	public bool CanTake()
	{
		return this.isTarget && Globals.Instance.Player.GuildSystem.CanTakeMinesReward(this.mInfo.OreAmount);
	}

	public bool IsTaken()
	{
		return this.isTarget && Globals.Instance.Player.GuildSystem.IsTakenMinesReward(this.mInfo.ID);
	}

	public void OnTake()
	{
		if (this.TakeRewardEvent != null)
		{
			this.TakeRewardEvent(this);
		}
	}
}
