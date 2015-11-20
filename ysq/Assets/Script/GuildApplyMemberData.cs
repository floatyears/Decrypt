using Proto;
using System;

public class GuildApplyMemberData : BaseData
{
	public GuildApplication GuildApplicationData
	{
		get;
		private set;
	}

	public GuildApplyMemberData(GuildApplication aInfo)
	{
		this.GuildApplicationData = aInfo;
	}

	public override ulong GetID()
	{
		return this.GuildApplicationData.ID;
	}
}
