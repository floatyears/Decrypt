using Proto;
using System;

public class ObscuredStats
{
	private Stats hiddenValue;

	private int currentCryptoKey;

	private uint Exp2;

	private uint Level2;

	private int Energy2;

	private int Stamina2;

	private int Money2;

	private int Diamond2;

	private uint VipLevel2;

	private uint TotalPay2;

	private int D2MCount2;

	private int Honor2;

	private int Reputation2;

	private int MagicCrystal2;

	private int GuildBossCount2;

	private int KingMedal2;

	private int ConstellationLevel2;

	private int FurtherLevel2;

	private int ArenaHighestRank2;

	private int AwakeLevel2;

	private int AwakeItemFlag2;

	public Stats Stats
	{
		private get
		{
			return this.hiddenValue;
		}
		set
		{
			this.hiddenValue = value;
			this.Exp = value.Exp;
			this.Level = value.Level;
			this.Energy = value.Energy;
			this.Stamina = value.Stamina;
			this.Money = value.Money;
			this.Diamond = value.Diamond;
			this.VipLevel = value.VipLevel;
			this.TotalPay = value.TotalPay;
			this.D2MCount = value.D2MCount;
			this.Honor = value.Honor;
			this.Reputation = value.Reputation;
			this.MagicCrystal = value.MagicCrystal;
			this.GuildBossCount = value.GuildBossCount;
			this.KingMedal = value.KingMedal;
			this.ConstellationLevel = value.ConstellationLevel;
			this.FurtherLevel = value.FurtherLevel;
			this.ArenaHighestRank = value.ArenaHighestRank;
			this.AwakeLevel = value.AwakeLevel;
			this.AwakeItemFlag = value.AwakeItemFlag;
		}
	}

	public uint Exp
	{
		get
		{
			return (uint)((ulong)(this.hiddenValue.Exp | this.Exp2 << 16) ^ (ulong)((long)this.currentCryptoKey));
		}
		set
		{
			uint num = (uint)((ulong)value ^ (ulong)((long)this.currentCryptoKey));
			this.hiddenValue.Exp = (num & 65535u);
			this.Exp2 = (num & 4294901760u) >> 16;
		}
	}

	public uint Level
	{
		get
		{
			return (uint)((ulong)(this.hiddenValue.Level | this.Level2 << 16) ^ (ulong)((long)this.currentCryptoKey));
		}
		set
		{
			uint num = (uint)((ulong)value ^ (ulong)((long)this.currentCryptoKey));
			this.hiddenValue.Level = (num & 0xffff);
			this.Level2 = (num & 4294901760u) >> 16;
		}
	}

	public int Energy
	{
		get
		{
			return (this.hiddenValue.Energy | this.Energy2 << 16) ^ this.currentCryptoKey;
		}
		set
		{
			int num = value ^ this.currentCryptoKey;
			this.hiddenValue.Energy = (num & 65535);
			this.Energy2 = (int)((long)num & (long)(0xffff0000L)) >> 16;
		}
	}

	public int Stamina
	{
		get
		{
			return (this.hiddenValue.Stamina | this.Stamina2 << 16) ^ this.currentCryptoKey;
		}
		set
		{
			int num = value ^ this.currentCryptoKey;
			this.hiddenValue.Stamina = (num & 65535);
			this.Stamina2 = (int)((long)num & (long)0xffff0000L) >> 16;
		}
	}

	public int Money
	{
		get
		{
			return (this.hiddenValue.Money | this.Money2 << 16) ^ this.currentCryptoKey;
		}
		set
		{
			int num = value ^ this.currentCryptoKey;
			this.hiddenValue.Money = (num & 65535);
            this.Money2 = (int)((long)num & (long)(0xffff0000L)) >> 16;
		}
	}

	public int Diamond
	{
		get
		{
			return (this.hiddenValue.Diamond | this.Diamond2 << 16) ^ this.currentCryptoKey;
		}
		set
		{
			int num = value ^ this.currentCryptoKey;
			this.hiddenValue.Diamond = (num & 65535);
            this.Diamond2 = (int)((long)num & (long)(0xffff0000L)) >> 16;
		}
	}

	public uint VipLevel
	{
		get
		{
			return (uint)((ulong)(this.hiddenValue.VipLevel | this.VipLevel2 << 16) ^ (ulong)((long)this.currentCryptoKey));
		}
		set
		{
			uint num = (uint)((ulong)value ^ (ulong)((long)this.currentCryptoKey));
			this.hiddenValue.VipLevel = (num & 65535u);
			this.VipLevel2 = (num & 4294901760u) >> 16;
		}
	}

	public uint TotalPay
	{
		get
		{
			return (uint)((ulong)(this.hiddenValue.TotalPay | this.TotalPay2 << 16) ^ (ulong)((long)this.currentCryptoKey));
		}
		set
		{
			uint num = (uint)((ulong)value ^ (ulong)((long)this.currentCryptoKey));
			this.hiddenValue.TotalPay = (num & 65535u);
			this.TotalPay2 = (num & 4294901760u) >> 16;
		}
	}

	public int D2MCount
	{
		get
		{
			return (this.hiddenValue.D2MCount | this.D2MCount2 << 16) ^ this.currentCryptoKey;
		}
		set
		{
			int num = value ^ this.currentCryptoKey;
			this.hiddenValue.D2MCount = (num & 65535);
			this.D2MCount2 = (int)((long)num & (long)0xffff0000L) >> 16;
		}
	}

	public int Honor
	{
		get
		{
			return (this.hiddenValue.Honor | this.Honor2 << 16) ^ this.currentCryptoKey;
		}
		set
		{
			int num = value ^ this.currentCryptoKey;
			this.hiddenValue.Honor = (num & 65535);
			this.Honor2 = (int)((long)num & (long)0xffff0000L) >> 16;
		}
	}

	public int Reputation
	{
		get
		{
			return (this.hiddenValue.Reputation | this.Reputation2 << 16) ^ this.currentCryptoKey;
		}
		set
		{
			int num = value ^ this.currentCryptoKey;
			this.hiddenValue.Reputation = (num & 65535);
			this.Reputation2 = (int)((long)num & (long)0xffff0000L) >> 16;
		}
	}

	public int MagicCrystal
	{
		get
		{
			return (this.hiddenValue.MagicCrystal | this.MagicCrystal2 << 16) ^ this.currentCryptoKey;
		}
		set
		{
			int num = value ^ this.currentCryptoKey;
			this.hiddenValue.MagicCrystal = (num & 65535);
			this.MagicCrystal2 = (int)((long)num & (long)0xffff0000L) >> 16;
		}
	}

	public int GuildBossCount
	{
		get
		{
			return (this.hiddenValue.GuildBossCount | this.GuildBossCount2 << 16) ^ this.currentCryptoKey;
		}
		set
		{
			int num = value ^ this.currentCryptoKey;
			this.hiddenValue.GuildBossCount = (num & 65535);
			this.GuildBossCount2 = (int)((long)num & (long)0xffff0000L) >> 16;
		}
	}

	public int KingMedal
	{
		get
		{
			return (this.hiddenValue.KingMedal | this.KingMedal2 << 16) ^ this.currentCryptoKey;
		}
		set
		{
			int num = value ^ this.currentCryptoKey;
			this.hiddenValue.KingMedal = (num & 65535);
			this.KingMedal2 = (int)((long)num & (long)0xffff0000L) >> 16;
		}
	}

	public int ConstellationLevel
	{
		get
		{
			return (this.hiddenValue.ConstellationLevel | this.ConstellationLevel2 << 16) ^ this.currentCryptoKey;
		}
		set
		{
			int num = value ^ this.currentCryptoKey;
			this.hiddenValue.ConstellationLevel = (num & 65535);
			this.ConstellationLevel2 = (int)(((long)num & (long)0xffff0000L) >> 16);
		}
	}

	public int FurtherLevel
	{
		get
		{
			return (this.hiddenValue.FurtherLevel | this.FurtherLevel2 << 16) ^ this.currentCryptoKey;
		}
		set
		{
			int num = value ^ this.currentCryptoKey;
			this.hiddenValue.FurtherLevel = (num & 65535);
			this.FurtherLevel2 = (int)(((long)num & (long)0xffff0000L) >> 16);
		}
	}

	public int ArenaHighestRank
	{
		get
		{
			return (this.hiddenValue.ArenaHighestRank | this.ArenaHighestRank2 << 16) ^ this.currentCryptoKey;
		}
		set
		{
			int num = value ^ this.currentCryptoKey;
			this.hiddenValue.ArenaHighestRank = (num & 65535);
			this.ArenaHighestRank2 = (int)(((long)num & (long)0xffff0000L) >> 16);
		}
	}

	public int AwakeLevel
	{
		get
		{
			return (this.hiddenValue.AwakeLevel | this.AwakeLevel2 << 16) ^ this.currentCryptoKey;
		}
		set
		{
			int num = value ^ this.currentCryptoKey;
			this.hiddenValue.AwakeLevel = (num & 65535);
			this.AwakeLevel2 = (int)(((long)num & (long)0xffff0000L) >> 16);
		}
	}

	public int AwakeItemFlag
	{
		get
		{
			return (this.hiddenValue.AwakeItemFlag | this.AwakeItemFlag2 << 16) ^ this.currentCryptoKey;
		}
		set
		{
			int num = value ^ this.currentCryptoKey;
			this.hiddenValue.AwakeItemFlag = (num & 65535);
			this.AwakeItemFlag2 = (int)(((long)num & (long)0xffff0000L) >> 16);
		}
	}

	public ulong ID
	{
		get
		{
			return this.hiddenValue.ID;
		}
	}

	public ulong AccountID
	{
		get
		{
			return this.hiddenValue.AccountID;
		}
	}

	public long CreateTime
	{
		get
		{
			return this.hiddenValue.CreateTime;
		}
	}

	public string Name
	{
		get
		{
			return this.hiddenValue.Name;
		}
		set
		{
			this.hiddenValue.Name = value;
		}
	}

	public int Gender
	{
		get
		{
			return this.hiddenValue.Gender;
		}
		set
		{
			this.hiddenValue.Gender = value;
		}
	}

	public int TimeStamp
	{
		get
		{
			return this.hiddenValue.TimeStamp;
		}
		set
		{
			this.hiddenValue.TimeStamp = value;
		}
	}

	public int EnergyTimeStamp
	{
		get
		{
			return this.hiddenValue.EnergyTimeStamp;
		}
		set
		{
			this.hiddenValue.EnergyTimeStamp = value;
		}
	}

	public uint DayLevel
	{
		get
		{
			return this.hiddenValue.DayLevel;
		}
		set
		{
			this.hiddenValue.DayLevel = value;
		}
	}

	public int DayTimeStamp
	{
		get
		{
			return this.hiddenValue.DayTimeStamp;
		}
		set
		{
			this.hiddenValue.DayTimeStamp = value;
		}
	}

	public int DataFlag
	{
		get
		{
			return this.hiddenValue.DataFlag;
		}
		set
		{
			this.hiddenValue.DataFlag = value;
		}
	}

	public int FreeLuckyRollCD1
	{
		get
		{
			return this.hiddenValue.FreeLuckyRollCD1;
		}
		set
		{
			this.hiddenValue.FreeLuckyRollCD1 = value;
		}
	}

	public int FreeLuckyRollCD2
	{
		get
		{
			return this.hiddenValue.FreeLuckyRollCD2;
		}
		set
		{
			this.hiddenValue.FreeLuckyRollCD2 = value;
		}
	}

	public int SignIn
	{
		get
		{
			return this.hiddenValue.SignIn;
		}
		set
		{
			this.hiddenValue.SignIn = value;
		}
	}

	public int SignInTimeStamp
	{
		get
		{
			return this.hiddenValue.SignInTimeStamp;
		}
		set
		{
			this.hiddenValue.SignInTimeStamp = value;
		}
	}

	public int LevelReward
	{
		get
		{
			return this.hiddenValue.LevelReward;
		}
		set
		{
			this.hiddenValue.LevelReward = value;
		}
	}

	public int VipWeekReward
	{
		get
		{
			return this.hiddenValue.VipWeekReward;
		}
		set
		{
			this.hiddenValue.VipWeekReward = value;
		}
	}

	public int VipPayReward
	{
		get
		{
			return this.hiddenValue.VipPayReward;
		}
		set
		{
			this.hiddenValue.VipPayReward = value;
		}
	}

	public int CardTimeStamp
	{
		get
		{
			return this.hiddenValue.CardTimeStamp;
		}
		set
		{
			this.hiddenValue.CardTimeStamp = value;
		}
	}

	public int CardFlag
	{
		get
		{
			return this.hiddenValue.CardFlag;
		}
		set
		{
			this.hiddenValue.CardFlag = value;
		}
	}

	public int CardRenew
	{
		get
		{
			return this.hiddenValue.CardRenew;
		}
		set
		{
			this.hiddenValue.CardRenew = value;
		}
	}

	public int VipSignIn
	{
		get
		{
			return this.hiddenValue.VipSignIn;
		}
		set
		{
			this.hiddenValue.VipSignIn = value;
		}
	}

	public int FreeLuckyRollCount
	{
		get
		{
			return this.hiddenValue.FreeLuckyRollCount;
		}
		set
		{
			this.hiddenValue.FreeLuckyRollCount = value;
		}
	}

	public uint OnlineDays
	{
		get
		{
			return this.hiddenValue.OnlineDays;
		}
		set
		{
			this.hiddenValue.OnlineDays = value;
		}
	}

	public int Day7Flag
	{
		get
		{
			return this.hiddenValue.Day7Flag;
		}
		set
		{
			this.hiddenValue.Day7Flag = value;
		}
	}

	public int LRTimeStamp
	{
		get
		{
			return this.hiddenValue.LRTimeStamp;
		}
		set
		{
			this.hiddenValue.LRTimeStamp = value;
		}
	}

	public int LRPetID
	{
		get
		{
			return this.hiddenValue.LRPetID;
		}
		set
		{
			this.hiddenValue.LRPetID = value;
		}
	}

	public int LRPetID1
	{
		get
		{
			return this.hiddenValue.LRPetID1;
		}
		set
		{
			this.hiddenValue.LRPetID1 = value;
		}
	}

	public int LRPetID2
	{
		get
		{
			return this.hiddenValue.LRPetID2;
		}
		set
		{
			this.hiddenValue.LRPetID2 = value;
		}
	}

	public int LRPetID3
	{
		get
		{
			return this.hiddenValue.LRPetID3;
		}
		set
		{
			this.hiddenValue.LRPetID3 = value;
		}
	}

	public int LuckyRoll2Count
	{
		get
		{
			return this.hiddenValue.LuckyRoll2Count;
		}
		set
		{
			this.hiddenValue.LuckyRoll2Count = value;
		}
	}

	public int HasGuild
	{
		get
		{
			return this.hiddenValue.HasGuild;
		}
		set
		{
			this.hiddenValue.HasGuild = value;
		}
	}

	public int ChatTimestamp
	{
		get
		{
			return this.hiddenValue.ChatTimestamp;
		}
		set
		{
			this.hiddenValue.ChatTimestamp = value;
		}
	}

	public int ChatLimitTimestamp
	{
		get
		{
			return this.hiddenValue.ChatLimitTimestamp;
		}
		set
		{
			this.hiddenValue.ChatLimitTimestamp = value;
		}
	}

	public int ChatFrequency
	{
		get
		{
			return this.hiddenValue.ChatFrequency;
		}
		set
		{
			this.hiddenValue.ChatFrequency = value;
		}
	}

	public int SuperCardTimeStamp
	{
		get
		{
			return this.hiddenValue.SuperCardTimeStamp;
		}
		set
		{
			this.hiddenValue.SuperCardTimeStamp = value;
		}
	}

	public int RedFlag
	{
		get
		{
			return this.hiddenValue.RedFlag;
		}
		set
		{
			this.hiddenValue.RedFlag = value;
		}
	}

	public int StaminaTimeStamp
	{
		get
		{
			return this.hiddenValue.StaminaTimeStamp;
		}
		set
		{
			this.hiddenValue.StaminaTimeStamp = value;
		}
	}

	public int MagicSoul
	{
		get
		{
			return this.hiddenValue.MagicSoul;
		}
		set
		{
			this.hiddenValue.MagicSoul = value;
		}
	}

	public int FireDragonScale
	{
		get
		{
			return this.hiddenValue.FireDragonScale;
		}
		set
		{
			this.hiddenValue.FireDragonScale = value;
		}
	}

	public int WarFreeTime
	{
		get
		{
			return this.hiddenValue.WarFreeTime;
		}
		set
		{
			this.hiddenValue.WarFreeTime = value;
		}
	}

	public uint Common2ShopRefresh
	{
		get
		{
			return this.hiddenValue.Common2ShopRefresh;
		}
		set
		{
			this.hiddenValue.Common2ShopRefresh = value;
		}
	}

	public uint AwakenShopRefresh
	{
		get
		{
			return this.hiddenValue.AwakenShopRefresh;
		}
		set
		{
			this.hiddenValue.AwakenShopRefresh = value;
		}
	}

	public int LopetShopRefresh
	{
		get
		{
			return this.hiddenValue.LopetShopRefresh;
		}
		set
		{
			this.hiddenValue.LopetShopRefresh = value;
		}
	}

	public int ShopCommon2TimeStamp
	{
		get
		{
			return this.hiddenValue.ShopCommon2TimeStamp;
		}
		set
		{
			this.hiddenValue.ShopCommon2TimeStamp = value;
		}
	}

	public int ShopAwakenTimeStamp
	{
		get
		{
			return this.hiddenValue.ShopAwakenTimeStamp;
		}
		set
		{
			this.hiddenValue.ShopAwakenTimeStamp = value;
		}
	}

	public int TrialMaxWave
	{
		get
		{
			return this.hiddenValue.TrialMaxWave;
		}
		set
		{
			this.hiddenValue.TrialMaxWave = value;
		}
	}

	public int TrialWave
	{
		get
		{
			return this.hiddenValue.TrialWave;
		}
		set
		{
			this.hiddenValue.TrialWave = value;
		}
	}

	public int TrialResetCount
	{
		get
		{
			return this.hiddenValue.TrialResetCount;
		}
		set
		{
			this.hiddenValue.TrialResetCount = value;
		}
	}

	public int TrialFarmTimeStamp
	{
		get
		{
			return this.hiddenValue.TrialFarmTimeStamp;
		}
		set
		{
			this.hiddenValue.TrialFarmTimeStamp = value;
		}
	}

	public int TrialOver
	{
		get
		{
			return this.hiddenValue.TrialOver;
		}
		set
		{
			this.hiddenValue.TrialOver = value;
		}
	}

	public int DailyScore
	{
		get
		{
			return this.hiddenValue.DailyScore;
		}
		set
		{
			this.hiddenValue.DailyScore = value;
		}
	}

	public int DailyRewardFlag
	{
		get
		{
			return this.hiddenValue.DailyRewardFlag;
		}
		set
		{
			this.hiddenValue.DailyRewardFlag = value;
		}
	}

	public int GuildScoreRewardFlag
	{
		get
		{
			return this.hiddenValue.GuildScoreRewardFlag;
		}
		set
		{
			this.hiddenValue.GuildScoreRewardFlag = value;
		}
	}

	public int FundFlag
	{
		get
		{
			return this.hiddenValue.FundFlag;
		}
		set
		{
			this.hiddenValue.FundFlag = value;
		}
	}

	public int WelfareFlag
	{
		get
		{
			return this.hiddenValue.WelfareFlag;
		}
		set
		{
			this.hiddenValue.WelfareFlag = value;
		}
	}

	public int DayHotFlag
	{
		get
		{
			return this.hiddenValue.DayHotFlag;
		}
		set
		{
			this.hiddenValue.DayHotFlag = value;
		}
	}

	public int StarSoul
	{
		get
		{
			return this.hiddenValue.StarSoul;
		}
		set
		{
			this.hiddenValue.StarSoul = value;
		}
	}

	public int Emblem
	{
		get
		{
			return this.hiddenValue.Emblem;
		}
		set
		{
			this.hiddenValue.Emblem = value;
		}
	}

	public int LopetSoul
	{
		get
		{
			return this.hiddenValue.LopetSoul;
		}
		set
		{
			this.hiddenValue.LopetSoul = value;
		}
	}

	public int WeekTimestamp
	{
		get
		{
			return this.hiddenValue.WeekTimestamp;
		}
		set
		{
			this.hiddenValue.WeekTimestamp = value;
		}
	}

	public int Praise
	{
		get
		{
			return this.hiddenValue.Praise;
		}
		set
		{
			this.hiddenValue.Praise = value;
		}
	}

	public int TakeFriendEnergy
	{
		get
		{
			return this.hiddenValue.TakeFriendEnergy;
		}
		set
		{
			this.hiddenValue.TakeFriendEnergy = value;
		}
	}

	public int TakeGuildGift
	{
		get
		{
			return this.hiddenValue.TakeGuildGift;
		}
		set
		{
			this.hiddenValue.TakeGuildGift = value;
		}
	}

	public int MGCount
	{
		get
		{
			return this.hiddenValue.MGCount;
		}
		set
		{
			this.hiddenValue.MGCount = value;
		}
	}

	public int NightmareCount
	{
		get
		{
			return this.hiddenValue.NightmareCount;
		}
		set
		{
			this.hiddenValue.NightmareCount = value;
		}
	}

	public int Attack
	{
		get
		{
			return this.hiddenValue.Attack;
		}
		set
		{
			this.hiddenValue.Attack = value;
		}
	}

	public int PhysicDefense
	{
		get
		{
			return this.hiddenValue.PhysicDefense;
		}
		set
		{
			this.hiddenValue.PhysicDefense = value;
		}
	}

	public int MagicDefense
	{
		get
		{
			return this.hiddenValue.MagicDefense;
		}
		set
		{
			this.hiddenValue.MagicDefense = value;
		}
	}

	public int MaxHP
	{
		get
		{
			return this.hiddenValue.MaxHP;
		}
		set
		{
			this.hiddenValue.MaxHP = value;
		}
	}

	public int AttackPreview
	{
		get
		{
			return this.hiddenValue.AttackPreview;
		}
		set
		{
			this.hiddenValue.AttackPreview = value;
		}
	}

	public int PhysicDefensePreview
	{
		get
		{
			return this.hiddenValue.PhysicDefensePreview;
		}
		set
		{
			this.hiddenValue.PhysicDefensePreview = value;
		}
	}

	public int MagicDefensePreview
	{
		get
		{
			return this.hiddenValue.MagicDefensePreview;
		}
		set
		{
			this.hiddenValue.MagicDefensePreview = value;
		}
	}

	public int MaxHPPreview
	{
		get
		{
			return this.hiddenValue.MaxHPPreview;
		}
		set
		{
			this.hiddenValue.MaxHPPreview = value;
		}
	}

	public int CultivateCount
	{
		get
		{
			return this.hiddenValue.CultivateCount;
		}
		set
		{
			this.hiddenValue.CultivateCount = value;
		}
	}

	public int MGFlag
	{
		get
		{
			return this.hiddenValue.MGFlag;
		}
		set
		{
			this.hiddenValue.MGFlag = value;
		}
	}

	public int FestivalVoucher
	{
		get
		{
			return this.hiddenValue.FestivalVoucher;
		}
		set
		{
			this.hiddenValue.FestivalVoucher = value;
		}
	}

	public ObscuredStats()
	{
		long ticks = DateTime.Now.Ticks;
		Random random = new Random((int)(ticks & (long)(0xfffffffe)) | (int)(ticks >> 32));
		this.currentCryptoKey = random.Next(268435455);
		this.hiddenValue = new Stats();
	}
}
