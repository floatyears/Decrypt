using Att;
using Proto;
using System;

public class GuildActivityItemData : BaseData
{
	public enum EGAIType
	{
		EGAIGuildShop,
		EGAIGuildMagic,
		EGAIGuildSchool,
		EGAIGuildKuangShi,
		EGAIGuildCraft
	}

	public GuildActivityItemData.EGAIType mActivityType
	{
		get;
		private set;
	}

	public bool TipMarkIsShow
	{
		get;
		set;
	}

	public GuildActivityItemData(GuildActivityItemData.EGAIType tp)
	{
		this.mActivityType = tp;
		this.RefreshRedMark();
	}

	public void RefreshRedMark()
	{
		if (this.mActivityType == GuildActivityItemData.EGAIType.EGAIGuildShop)
		{
			this.TipMarkIsShow = false;
		}
		else if (this.mActivityType == GuildActivityItemData.EGAIType.EGAIGuildMagic)
		{
			this.TipMarkIsShow = (this.IsCanGuildSign(false) || this.IsCanGuildSignReward());
		}
		else if (this.mActivityType == GuildActivityItemData.EGAIType.EGAIGuildSchool)
		{
			this.TipMarkIsShow = this.IsGuildSchoolRed();
		}
		else if (this.mActivityType == GuildActivityItemData.EGAIType.EGAIGuildKuangShi)
		{
			this.TipMarkIsShow = this.IsGuildMinesRed();
		}
		else if (this.mActivityType == GuildActivityItemData.EGAIType.EGAIGuildCraft)
		{
			this.TipMarkIsShow = this.IsGuildCraftRed();
		}
	}

	private bool IsCanGuildSign(bool showMsg = false)
	{
		bool result = false;
		for (int i = 0; i < Globals.Instance.Player.GuildSystem.Members.Count; i++)
		{
			GuildMember guildMember = Globals.Instance.Player.GuildSystem.Members[i];
			if (guildMember.ID == Globals.Instance.Player.Data.ID)
			{
				result = ((guildMember.Flag & 4) == 0);
			}
		}
		return result;
	}

	private bool IsBoxRewardTaken(int index)
	{
		return (Globals.Instance.Player.Data.GuildScoreRewardFlag & 1 << index) != 0;
	}

	private bool IsCanGuildSignReward()
	{
		bool result = false;
		GuildInfo info = Globals.Instance.AttDB.GuildDict.GetInfo(Globals.Instance.Player.GuildSystem.Guild.Level);
		if (info != null)
		{
			for (int i = 0; i < 4; i++)
			{
				if (info.Score * (i + 1) <= Globals.Instance.Player.GuildSystem.Guild.Score && !this.IsBoxRewardTaken(i))
				{
					result = true;
					break;
				}
			}
		}
		return result;
	}

	private bool IsGuildSchoolRed()
	{
		return (Globals.Instance.Player.Data.RedFlag & 2) != 0;
	}

	private bool IsGuildMinesRed()
	{
		return (Globals.Instance.Player.Data.RedFlag & 32768) != 0;
	}

	private bool IsGuildCraftRed()
	{
		return (Globals.Instance.Player.Data.RedFlag & 8192) != 0;
	}

	public override ulong GetID()
	{
		return (ulong)((long)this.mActivityType);
	}
}
