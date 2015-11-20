using Att;
using System;

public class GUIGuildSchoolItemData : BaseData
{
	private int mSchoolId;

	private GuildInfo mGuildInfo;

	public int SchoolId
	{
		get
		{
			return this.mSchoolId;
		}
	}

	public GuildInfo GuildInFo
	{
		get
		{
			return this.mGuildInfo;
		}
	}

	public GUIGuildSchoolItemData(int sId, GuildInfo guildInfo)
	{
		this.mSchoolId = sId;
		this.mGuildInfo = guildInfo;
	}

	public override ulong GetID()
	{
		return (ulong)((long)this.mSchoolId);
	}
}
