using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_UpdatePlayer")]
	[Serializable]
	public class MS2C_UpdatePlayer : IExtensible
	{
		private uint _Exp;

		private uint _Level;

		private int _Energy;

		private int _Stamina;

		private int _Money;

		private int _Diamond;

		private uint _StatsVersion;

		private int _EnergyTimeStamp;

		private uint _DayLevel;

		private int _DayTimeStamp;

		private int _DataFlag;

		private uint _VipLevel;

		private uint _TotalPay;

		private int _FreeLuckyRollCD1;

		private int _FreeLuckyRollCD2;

		private int _SignIn;

		private int _SignInTimeStamp;

		private int _LevelReward;

		private int _VipWeekReward;

		private int _VipPayReward;

		private int _CardTimeStamp;

		private int _CardFlag;

		private int _CardRenew;

		private int _D2MCount;

		private int _Pending;

		private int _FreeLuckyRollCount;

		private int _Honor;

		private int _Reputation;

		private uint _Common2ShopRefresh;

		private uint _AwakenShopRefresh;

		private uint _OnlineDays;

		private int _Day7Flag;

		private int _MagicCrystal;

		private int _LRTimeStamp;

		private int _LRPetID;

		private int _LRPetID1;

		private int _LRPetID2;

		private int _LRPetID3;

		private int _LuckyRoll2Count;

		private int _HasGuild;

		private int _GuildBossCount;

		private int _KingMedal;

		private int _ChatTimestamp;

		private int _ChatLimitTimestamp;

		private int _ChatFrequency;

		private int _SuperCardTimeStamp;

		private int _RedFlag;

		private int _ConstellationLevel;

		private int _FurtherLevel;

		private int _StaminaTimeStamp;

		private int _MagicSoul;

		private int _FireDragonScale;

		private int _WarFreeTime;

		private int _ShopCommon2TimeStamp;

		private int _ShopAwakenTimeStamp;

		private int _TrialMaxWave;

		private int _TrialWave;

		private int _TrialResetCount;

		private int _TrialFarmTimeStamp;

		private int _TrialOver;

		private int _DailyScore;

		private int _DailyRewardFlag;

		private int _GuildScoreRewardFlag;

		private int _FundFlag;

		private int _WelfareFlag;

		private int _DayHotFlag;

		private int _AwakeLevel;

		private int _AwakeItemFlag;

		private int _StarSoul;

		private int _WeekTimestamp;

		private int _Praise;

		private int _Emblem;

		private int _TakeFriendEnergy;

		private int _MGCount;

		private int _NightmareCount;

		private int _Attack;

		private int _PhysicDefense;

		private int _MagicDefense;

		private int _MaxHP;

		private int _AttackPreview;

		private int _PhysicDefensePreview;

		private int _MagicDefensePreview;

		private int _MaxHPPreview;

		private int _CultivateCount;

		private int _TakeGuildGift;

		private int _LopetSoul;

		private int _LopetShopRefresh;

		private int _ShopLopetTimeStamp;

		private int _MGFlag;

		private int _GuildWarHeroPraise;

		private int _FestivalVoucher;

		private int _FestivalVoucherVersion;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "Exp", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint Exp
		{
			get
			{
				return this._Exp;
			}
			set
			{
				this._Exp = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "Level", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint Level
		{
			get
			{
				return this._Level;
			}
			set
			{
				this._Level = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "Energy", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Energy
		{
			get
			{
				return this._Energy;
			}
			set
			{
				this._Energy = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "Stamina", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Stamina
		{
			get
			{
				return this._Stamina;
			}
			set
			{
				this._Stamina = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "Money", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Money
		{
			get
			{
				return this._Money;
			}
			set
			{
				this._Money = value;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "Diamond", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Diamond
		{
			get
			{
				return this._Diamond;
			}
			set
			{
				this._Diamond = value;
			}
		}

		[ProtoMember(7, IsRequired = false, Name = "StatsVersion", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint StatsVersion
		{
			get
			{
				return this._StatsVersion;
			}
			set
			{
				this._StatsVersion = value;
			}
		}

		[ProtoMember(8, IsRequired = false, Name = "EnergyTimeStamp", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int EnergyTimeStamp
		{
			get
			{
				return this._EnergyTimeStamp;
			}
			set
			{
				this._EnergyTimeStamp = value;
			}
		}

		[ProtoMember(9, IsRequired = false, Name = "DayLevel", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint DayLevel
		{
			get
			{
				return this._DayLevel;
			}
			set
			{
				this._DayLevel = value;
			}
		}

		[ProtoMember(10, IsRequired = false, Name = "DayTimeStamp", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int DayTimeStamp
		{
			get
			{
				return this._DayTimeStamp;
			}
			set
			{
				this._DayTimeStamp = value;
			}
		}

		[ProtoMember(11, IsRequired = false, Name = "DataFlag", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int DataFlag
		{
			get
			{
				return this._DataFlag;
			}
			set
			{
				this._DataFlag = value;
			}
		}

		[ProtoMember(12, IsRequired = false, Name = "VipLevel", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint VipLevel
		{
			get
			{
				return this._VipLevel;
			}
			set
			{
				this._VipLevel = value;
			}
		}

		[ProtoMember(13, IsRequired = false, Name = "TotalPay", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint TotalPay
		{
			get
			{
				return this._TotalPay;
			}
			set
			{
				this._TotalPay = value;
			}
		}

		[ProtoMember(14, IsRequired = false, Name = "FreeLuckyRollCD1", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int FreeLuckyRollCD1
		{
			get
			{
				return this._FreeLuckyRollCD1;
			}
			set
			{
				this._FreeLuckyRollCD1 = value;
			}
		}

		[ProtoMember(15, IsRequired = false, Name = "FreeLuckyRollCD2", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int FreeLuckyRollCD2
		{
			get
			{
				return this._FreeLuckyRollCD2;
			}
			set
			{
				this._FreeLuckyRollCD2 = value;
			}
		}

		[ProtoMember(16, IsRequired = false, Name = "SignIn", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int SignIn
		{
			get
			{
				return this._SignIn;
			}
			set
			{
				this._SignIn = value;
			}
		}

		[ProtoMember(17, IsRequired = false, Name = "SignInTimeStamp", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int SignInTimeStamp
		{
			get
			{
				return this._SignInTimeStamp;
			}
			set
			{
				this._SignInTimeStamp = value;
			}
		}

		[ProtoMember(18, IsRequired = false, Name = "LevelReward", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int LevelReward
		{
			get
			{
				return this._LevelReward;
			}
			set
			{
				this._LevelReward = value;
			}
		}

		[ProtoMember(19, IsRequired = false, Name = "VipWeekReward", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int VipWeekReward
		{
			get
			{
				return this._VipWeekReward;
			}
			set
			{
				this._VipWeekReward = value;
			}
		}

		[ProtoMember(20, IsRequired = false, Name = "VipPayReward", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int VipPayReward
		{
			get
			{
				return this._VipPayReward;
			}
			set
			{
				this._VipPayReward = value;
			}
		}

		[ProtoMember(21, IsRequired = false, Name = "CardTimeStamp", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int CardTimeStamp
		{
			get
			{
				return this._CardTimeStamp;
			}
			set
			{
				this._CardTimeStamp = value;
			}
		}

		[ProtoMember(22, IsRequired = false, Name = "CardFlag", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int CardFlag
		{
			get
			{
				return this._CardFlag;
			}
			set
			{
				this._CardFlag = value;
			}
		}

		[ProtoMember(23, IsRequired = false, Name = "CardRenew", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int CardRenew
		{
			get
			{
				return this._CardRenew;
			}
			set
			{
				this._CardRenew = value;
			}
		}

		[ProtoMember(24, IsRequired = false, Name = "D2MCount", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int D2MCount
		{
			get
			{
				return this._D2MCount;
			}
			set
			{
				this._D2MCount = value;
			}
		}

		[ProtoMember(25, IsRequired = false, Name = "Pending", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Pending
		{
			get
			{
				return this._Pending;
			}
			set
			{
				this._Pending = value;
			}
		}

		[ProtoMember(26, IsRequired = false, Name = "FreeLuckyRollCount", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int FreeLuckyRollCount
		{
			get
			{
				return this._FreeLuckyRollCount;
			}
			set
			{
				this._FreeLuckyRollCount = value;
			}
		}

		[ProtoMember(27, IsRequired = false, Name = "Honor", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Honor
		{
			get
			{
				return this._Honor;
			}
			set
			{
				this._Honor = value;
			}
		}

		[ProtoMember(28, IsRequired = false, Name = "Reputation", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Reputation
		{
			get
			{
				return this._Reputation;
			}
			set
			{
				this._Reputation = value;
			}
		}

		[ProtoMember(29, IsRequired = false, Name = "Common2ShopRefresh", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint Common2ShopRefresh
		{
			get
			{
				return this._Common2ShopRefresh;
			}
			set
			{
				this._Common2ShopRefresh = value;
			}
		}

		[ProtoMember(30, IsRequired = false, Name = "AwakenShopRefresh", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint AwakenShopRefresh
		{
			get
			{
				return this._AwakenShopRefresh;
			}
			set
			{
				this._AwakenShopRefresh = value;
			}
		}

		[ProtoMember(31, IsRequired = false, Name = "OnlineDays", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint OnlineDays
		{
			get
			{
				return this._OnlineDays;
			}
			set
			{
				this._OnlineDays = value;
			}
		}

		[ProtoMember(32, IsRequired = false, Name = "Day7Flag", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Day7Flag
		{
			get
			{
				return this._Day7Flag;
			}
			set
			{
				this._Day7Flag = value;
			}
		}

		[ProtoMember(33, IsRequired = false, Name = "MagicCrystal", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int MagicCrystal
		{
			get
			{
				return this._MagicCrystal;
			}
			set
			{
				this._MagicCrystal = value;
			}
		}

		[ProtoMember(34, IsRequired = false, Name = "LRTimeStamp", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int LRTimeStamp
		{
			get
			{
				return this._LRTimeStamp;
			}
			set
			{
				this._LRTimeStamp = value;
			}
		}

		[ProtoMember(35, IsRequired = false, Name = "LRPetID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int LRPetID
		{
			get
			{
				return this._LRPetID;
			}
			set
			{
				this._LRPetID = value;
			}
		}

		[ProtoMember(36, IsRequired = false, Name = "LRPetID1", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int LRPetID1
		{
			get
			{
				return this._LRPetID1;
			}
			set
			{
				this._LRPetID1 = value;
			}
		}

		[ProtoMember(37, IsRequired = false, Name = "LRPetID2", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int LRPetID2
		{
			get
			{
				return this._LRPetID2;
			}
			set
			{
				this._LRPetID2 = value;
			}
		}

		[ProtoMember(38, IsRequired = false, Name = "LRPetID3", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int LRPetID3
		{
			get
			{
				return this._LRPetID3;
			}
			set
			{
				this._LRPetID3 = value;
			}
		}

		[ProtoMember(39, IsRequired = false, Name = "LuckyRoll2Count", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int LuckyRoll2Count
		{
			get
			{
				return this._LuckyRoll2Count;
			}
			set
			{
				this._LuckyRoll2Count = value;
			}
		}

		[ProtoMember(40, IsRequired = false, Name = "HasGuild", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int HasGuild
		{
			get
			{
				return this._HasGuild;
			}
			set
			{
				this._HasGuild = value;
			}
		}

		[ProtoMember(41, IsRequired = false, Name = "GuildBossCount", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int GuildBossCount
		{
			get
			{
				return this._GuildBossCount;
			}
			set
			{
				this._GuildBossCount = value;
			}
		}

		[ProtoMember(42, IsRequired = false, Name = "KingMedal", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int KingMedal
		{
			get
			{
				return this._KingMedal;
			}
			set
			{
				this._KingMedal = value;
			}
		}

		[ProtoMember(43, IsRequired = false, Name = "ChatTimestamp", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int ChatTimestamp
		{
			get
			{
				return this._ChatTimestamp;
			}
			set
			{
				this._ChatTimestamp = value;
			}
		}

		[ProtoMember(44, IsRequired = false, Name = "ChatLimitTimestamp", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int ChatLimitTimestamp
		{
			get
			{
				return this._ChatLimitTimestamp;
			}
			set
			{
				this._ChatLimitTimestamp = value;
			}
		}

		[ProtoMember(45, IsRequired = false, Name = "ChatFrequency", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int ChatFrequency
		{
			get
			{
				return this._ChatFrequency;
			}
			set
			{
				this._ChatFrequency = value;
			}
		}

		[ProtoMember(46, IsRequired = false, Name = "SuperCardTimeStamp", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int SuperCardTimeStamp
		{
			get
			{
				return this._SuperCardTimeStamp;
			}
			set
			{
				this._SuperCardTimeStamp = value;
			}
		}

		[ProtoMember(47, IsRequired = false, Name = "RedFlag", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int RedFlag
		{
			get
			{
				return this._RedFlag;
			}
			set
			{
				this._RedFlag = value;
			}
		}

		[ProtoMember(48, IsRequired = false, Name = "ConstellationLevel", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int ConstellationLevel
		{
			get
			{
				return this._ConstellationLevel;
			}
			set
			{
				this._ConstellationLevel = value;
			}
		}

		[ProtoMember(49, IsRequired = false, Name = "FurtherLevel", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int FurtherLevel
		{
			get
			{
				return this._FurtherLevel;
			}
			set
			{
				this._FurtherLevel = value;
			}
		}

		[ProtoMember(50, IsRequired = false, Name = "StaminaTimeStamp", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int StaminaTimeStamp
		{
			get
			{
				return this._StaminaTimeStamp;
			}
			set
			{
				this._StaminaTimeStamp = value;
			}
		}

		[ProtoMember(51, IsRequired = false, Name = "MagicSoul", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int MagicSoul
		{
			get
			{
				return this._MagicSoul;
			}
			set
			{
				this._MagicSoul = value;
			}
		}

		[ProtoMember(52, IsRequired = false, Name = "FireDragonScale", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int FireDragonScale
		{
			get
			{
				return this._FireDragonScale;
			}
			set
			{
				this._FireDragonScale = value;
			}
		}

		[ProtoMember(53, IsRequired = false, Name = "WarFreeTime", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int WarFreeTime
		{
			get
			{
				return this._WarFreeTime;
			}
			set
			{
				this._WarFreeTime = value;
			}
		}

		[ProtoMember(54, IsRequired = false, Name = "ShopCommon2TimeStamp", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int ShopCommon2TimeStamp
		{
			get
			{
				return this._ShopCommon2TimeStamp;
			}
			set
			{
				this._ShopCommon2TimeStamp = value;
			}
		}

		[ProtoMember(55, IsRequired = false, Name = "ShopAwakenTimeStamp", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int ShopAwakenTimeStamp
		{
			get
			{
				return this._ShopAwakenTimeStamp;
			}
			set
			{
				this._ShopAwakenTimeStamp = value;
			}
		}

		[ProtoMember(56, IsRequired = false, Name = "TrialMaxWave", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int TrialMaxWave
		{
			get
			{
				return this._TrialMaxWave;
			}
			set
			{
				this._TrialMaxWave = value;
			}
		}

		[ProtoMember(57, IsRequired = false, Name = "TrialWave", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int TrialWave
		{
			get
			{
				return this._TrialWave;
			}
			set
			{
				this._TrialWave = value;
			}
		}

		[ProtoMember(58, IsRequired = false, Name = "TrialResetCount", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int TrialResetCount
		{
			get
			{
				return this._TrialResetCount;
			}
			set
			{
				this._TrialResetCount = value;
			}
		}

		[ProtoMember(59, IsRequired = false, Name = "TrialFarmTimeStamp", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int TrialFarmTimeStamp
		{
			get
			{
				return this._TrialFarmTimeStamp;
			}
			set
			{
				this._TrialFarmTimeStamp = value;
			}
		}

		[ProtoMember(60, IsRequired = false, Name = "TrialOver", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int TrialOver
		{
			get
			{
				return this._TrialOver;
			}
			set
			{
				this._TrialOver = value;
			}
		}

		[ProtoMember(61, IsRequired = false, Name = "DailyScore", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int DailyScore
		{
			get
			{
				return this._DailyScore;
			}
			set
			{
				this._DailyScore = value;
			}
		}

		[ProtoMember(62, IsRequired = false, Name = "DailyRewardFlag", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int DailyRewardFlag
		{
			get
			{
				return this._DailyRewardFlag;
			}
			set
			{
				this._DailyRewardFlag = value;
			}
		}

		[ProtoMember(63, IsRequired = false, Name = "GuildScoreRewardFlag", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int GuildScoreRewardFlag
		{
			get
			{
				return this._GuildScoreRewardFlag;
			}
			set
			{
				this._GuildScoreRewardFlag = value;
			}
		}

		[ProtoMember(64, IsRequired = false, Name = "FundFlag", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int FundFlag
		{
			get
			{
				return this._FundFlag;
			}
			set
			{
				this._FundFlag = value;
			}
		}

		[ProtoMember(65, IsRequired = false, Name = "WelfareFlag", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int WelfareFlag
		{
			get
			{
				return this._WelfareFlag;
			}
			set
			{
				this._WelfareFlag = value;
			}
		}

		[ProtoMember(66, IsRequired = false, Name = "DayHotFlag", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int DayHotFlag
		{
			get
			{
				return this._DayHotFlag;
			}
			set
			{
				this._DayHotFlag = value;
			}
		}

		[ProtoMember(67, IsRequired = false, Name = "AwakeLevel", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int AwakeLevel
		{
			get
			{
				return this._AwakeLevel;
			}
			set
			{
				this._AwakeLevel = value;
			}
		}

		[ProtoMember(68, IsRequired = false, Name = "AwakeItemFlag", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int AwakeItemFlag
		{
			get
			{
				return this._AwakeItemFlag;
			}
			set
			{
				this._AwakeItemFlag = value;
			}
		}

		[ProtoMember(69, IsRequired = false, Name = "StarSoul", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int StarSoul
		{
			get
			{
				return this._StarSoul;
			}
			set
			{
				this._StarSoul = value;
			}
		}

		[ProtoMember(70, IsRequired = false, Name = "WeekTimestamp", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int WeekTimestamp
		{
			get
			{
				return this._WeekTimestamp;
			}
			set
			{
				this._WeekTimestamp = value;
			}
		}

		[ProtoMember(71, IsRequired = false, Name = "Praise", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Praise
		{
			get
			{
				return this._Praise;
			}
			set
			{
				this._Praise = value;
			}
		}

		[ProtoMember(72, IsRequired = false, Name = "Emblem", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Emblem
		{
			get
			{
				return this._Emblem;
			}
			set
			{
				this._Emblem = value;
			}
		}

		[ProtoMember(73, IsRequired = false, Name = "TakeFriendEnergy", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int TakeFriendEnergy
		{
			get
			{
				return this._TakeFriendEnergy;
			}
			set
			{
				this._TakeFriendEnergy = value;
			}
		}

		[ProtoMember(74, IsRequired = false, Name = "MGCount", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int MGCount
		{
			get
			{
				return this._MGCount;
			}
			set
			{
				this._MGCount = value;
			}
		}

		[ProtoMember(75, IsRequired = false, Name = "NightmareCount", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int NightmareCount
		{
			get
			{
				return this._NightmareCount;
			}
			set
			{
				this._NightmareCount = value;
			}
		}

		[ProtoMember(76, IsRequired = false, Name = "Attack", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Attack
		{
			get
			{
				return this._Attack;
			}
			set
			{
				this._Attack = value;
			}
		}

		[ProtoMember(77, IsRequired = false, Name = "PhysicDefense", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int PhysicDefense
		{
			get
			{
				return this._PhysicDefense;
			}
			set
			{
				this._PhysicDefense = value;
			}
		}

		[ProtoMember(78, IsRequired = false, Name = "MagicDefense", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int MagicDefense
		{
			get
			{
				return this._MagicDefense;
			}
			set
			{
				this._MagicDefense = value;
			}
		}

		[ProtoMember(79, IsRequired = false, Name = "MaxHP", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int MaxHP
		{
			get
			{
				return this._MaxHP;
			}
			set
			{
				this._MaxHP = value;
			}
		}

		[ProtoMember(80, IsRequired = false, Name = "AttackPreview", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int AttackPreview
		{
			get
			{
				return this._AttackPreview;
			}
			set
			{
				this._AttackPreview = value;
			}
		}

		[ProtoMember(81, IsRequired = false, Name = "PhysicDefensePreview", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int PhysicDefensePreview
		{
			get
			{
				return this._PhysicDefensePreview;
			}
			set
			{
				this._PhysicDefensePreview = value;
			}
		}

		[ProtoMember(82, IsRequired = false, Name = "MagicDefensePreview", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int MagicDefensePreview
		{
			get
			{
				return this._MagicDefensePreview;
			}
			set
			{
				this._MagicDefensePreview = value;
			}
		}

		[ProtoMember(83, IsRequired = false, Name = "MaxHPPreview", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int MaxHPPreview
		{
			get
			{
				return this._MaxHPPreview;
			}
			set
			{
				this._MaxHPPreview = value;
			}
		}

		[ProtoMember(84, IsRequired = false, Name = "CultivateCount", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int CultivateCount
		{
			get
			{
				return this._CultivateCount;
			}
			set
			{
				this._CultivateCount = value;
			}
		}

		[ProtoMember(85, IsRequired = false, Name = "TakeGuildGift", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int TakeGuildGift
		{
			get
			{
				return this._TakeGuildGift;
			}
			set
			{
				this._TakeGuildGift = value;
			}
		}

		[ProtoMember(86, IsRequired = false, Name = "LopetSoul", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int LopetSoul
		{
			get
			{
				return this._LopetSoul;
			}
			set
			{
				this._LopetSoul = value;
			}
		}

		[ProtoMember(87, IsRequired = false, Name = "LopetShopRefresh", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int LopetShopRefresh
		{
			get
			{
				return this._LopetShopRefresh;
			}
			set
			{
				this._LopetShopRefresh = value;
			}
		}

		[ProtoMember(88, IsRequired = false, Name = "ShopLopetTimeStamp", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int ShopLopetTimeStamp
		{
			get
			{
				return this._ShopLopetTimeStamp;
			}
			set
			{
				this._ShopLopetTimeStamp = value;
			}
		}

		[ProtoMember(89, IsRequired = false, Name = "MGFlag", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int MGFlag
		{
			get
			{
				return this._MGFlag;
			}
			set
			{
				this._MGFlag = value;
			}
		}

		[ProtoMember(90, IsRequired = false, Name = "GuildWarHeroPraise", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int GuildWarHeroPraise
		{
			get
			{
				return this._GuildWarHeroPraise;
			}
			set
			{
				this._GuildWarHeroPraise = value;
			}
		}

		[ProtoMember(91, IsRequired = false, Name = "FestivalVoucher", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int FestivalVoucher
		{
			get
			{
				return this._FestivalVoucher;
			}
			set
			{
				this._FestivalVoucher = value;
			}
		}

		[ProtoMember(92, IsRequired = false, Name = "FestivalVoucherVersion", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int FestivalVoucherVersion
		{
			get
			{
				return this._FestivalVoucherVersion;
			}
			set
			{
				this._FestivalVoucherVersion = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
