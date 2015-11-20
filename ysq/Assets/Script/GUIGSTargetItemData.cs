using Att;
using System;

public class GUIGSTargetItemData : BaseData
{
	private int mSchoolId;

	private GuildInfo mGuildInfo;

	private bool mIsSelected;

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

	public bool IsSelected
	{
		get
		{
			return this.mIsSelected;
		}
		set
		{
			this.mIsSelected = value;
		}
	}

	public GUIGSTargetItemData(int sId, GuildInfo guildInfo, bool isSel)
	{
		this.mSchoolId = sId;
		this.mGuildInfo = guildInfo;
		this.mIsSelected = isSel;
	}

	public override ulong GetID()
	{
		return (ulong)((long)this.mSchoolId);
	}
}
