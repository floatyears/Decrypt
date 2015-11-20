using Proto;
using System;

public class GuildMemberItemData : BaseData
{
	public GuildMember MemberData
	{
		get;
		private set;
	}

	public GuildMemberItemData(GuildMember gdm)
	{
		this.MemberData = gdm;
	}

	public override ulong GetID()
	{
		return this.MemberData.ID;
	}
}
