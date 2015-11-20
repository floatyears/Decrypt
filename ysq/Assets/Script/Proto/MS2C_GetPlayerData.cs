using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_GetPlayerData")]
	[Serializable]
	public class MS2C_GetPlayerData : IExtensible
	{
		private uint _StatsVersion;

		private Stats _StatsData;

		private uint _PetVersion;

		private readonly List<PetData> _Pets = new List<PetData>();

		private readonly List<ItemData> _Items = new List<ItemData>();

		private uint _ItemVersion;

		private readonly List<int> _Fashions = new List<int>();

		private uint _FashionVersion;

		private uint _SocketVersion;

		private readonly List<SocketData> _Sockets = new List<SocketData>();

		private readonly List<ulong> _AssistPetID = new List<ulong>();

		private readonly List<SceneData> _Scenes = new List<SceneData>();

		private uint _SceneVersion;

		private readonly List<MapRewardData> _MapRewards = new List<MapRewardData>();

		private uint _MapRewardVersion;

		private readonly List<AchievementData> _Achievements = new List<AchievementData>();

		private uint _AchievementVersion;

		private uint _MailVersion;

		private readonly List<uint> _MailIDs = new List<uint>();

		private readonly List<int> _GuideSteps = new List<int>();

		private uint _MsgSerialID;

		private readonly List<PayData> _Pays = new List<PayData>();

		private int _CostumePartyTimestamp;

		private int _CostumePartyCount;

		private bool _HasReward;

		private uint _RoomID;

		private readonly List<BuyRecord> _BuyData = new List<BuyRecord>();

		private uint _BuyDataVersion;

		private int _WorldOpenTimeStamp;

		private uint _SevenDayVersion;

		private readonly List<SevenDayRewardData> _SDRData = new List<SevenDayRewardData>();

		private uint _ShareVersion;

		private readonly List<ShareAchievementData> _ShardData = new List<ShareAchievementData>();

		private uint _ActivityAchievementVersion;

		private readonly List<ActivityAchievementData> _AAData = new List<ActivityAchievementData>();

		private uint _ActivityValueVersion;

		private readonly List<ActivityValueData> _AVData = new List<ActivityValueData>();

		private readonly List<int> _FDSReward = new List<int>();

		private int _BuyFundNum;

		private uint _ActivityShopVersion;

		private readonly List<ActivityShopData> _ASData = new List<ActivityShopData>();

		private ActivityPayData _APData;

		private readonly List<ActivityPayShopData> _APSData = new List<ActivityPayShopData>();

		private int _Setting;

		private string _ShareParam = string.Empty;

		private bool _hasShareParam;

		private readonly List<FriendData> _FriendList = new List<FriendData>();

		private GuildWarPlayerData _GuildWarData;

		private ActivityRollEquipData _REData;

		private ActivitySpecifyPayData _SPData;

		private uint _LopetVersion;

		private readonly List<LopetData> _Lopets = new List<LopetData>();

		private ulong _AssistLopetID;

		private ActivityNationalDayData _NationalDayData;

		private readonly List<int> _FashionTimes = new List<int>();

		private ActivityGroupBuyingData _GroupBuyingData;

		private ActivityHalloweenData _HalloweenData;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "StatsVersion", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
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

		[ProtoMember(2, IsRequired = false, Name = "StatsData", DataFormat = DataFormat.Default), DefaultValue(null)]
		public Stats StatsData
		{
			get
			{
				return this._StatsData;
			}
			set
			{
				this._StatsData = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "PetVersion", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint PetVersion
		{
			get
			{
				return this._PetVersion;
			}
			set
			{
				this._PetVersion = value;
			}
		}

		[ProtoMember(4, Name = "Pets", DataFormat = DataFormat.Default)]
		public List<PetData> Pets
		{
			get
			{
				return this._Pets;
			}
		}

		[ProtoMember(5, Name = "Items", DataFormat = DataFormat.Default)]
		public List<ItemData> Items
		{
			get
			{
				return this._Items;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "ItemVersion", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint ItemVersion
		{
			get
			{
				return this._ItemVersion;
			}
			set
			{
				this._ItemVersion = value;
			}
		}

		[ProtoMember(7, Name = "Fashions", DataFormat = DataFormat.TwosComplement)]
		public List<int> Fashions
		{
			get
			{
				return this._Fashions;
			}
		}

		[ProtoMember(8, IsRequired = false, Name = "FashionVersion", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint FashionVersion
		{
			get
			{
				return this._FashionVersion;
			}
			set
			{
				this._FashionVersion = value;
			}
		}

		[ProtoMember(9, IsRequired = false, Name = "SocketVersion", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint SocketVersion
		{
			get
			{
				return this._SocketVersion;
			}
			set
			{
				this._SocketVersion = value;
			}
		}

		[ProtoMember(10, Name = "Sockets", DataFormat = DataFormat.Default)]
		public List<SocketData> Sockets
		{
			get
			{
				return this._Sockets;
			}
		}

		[ProtoMember(11, Name = "AssistPetID", DataFormat = DataFormat.TwosComplement)]
		public List<ulong> AssistPetID
		{
			get
			{
				return this._AssistPetID;
			}
		}

		[ProtoMember(12, Name = "Scenes", DataFormat = DataFormat.Default)]
		public List<SceneData> Scenes
		{
			get
			{
				return this._Scenes;
			}
		}

		[ProtoMember(13, IsRequired = false, Name = "SceneVersion", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint SceneVersion
		{
			get
			{
				return this._SceneVersion;
			}
			set
			{
				this._SceneVersion = value;
			}
		}

		[ProtoMember(14, Name = "MapRewards", DataFormat = DataFormat.Default)]
		public List<MapRewardData> MapRewards
		{
			get
			{
				return this._MapRewards;
			}
		}

		[ProtoMember(15, IsRequired = false, Name = "MapRewardVersion", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint MapRewardVersion
		{
			get
			{
				return this._MapRewardVersion;
			}
			set
			{
				this._MapRewardVersion = value;
			}
		}

		[ProtoMember(17, Name = "Achievements", DataFormat = DataFormat.Default)]
		public List<AchievementData> Achievements
		{
			get
			{
				return this._Achievements;
			}
		}

		[ProtoMember(18, IsRequired = false, Name = "AchievementVersion", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint AchievementVersion
		{
			get
			{
				return this._AchievementVersion;
			}
			set
			{
				this._AchievementVersion = value;
			}
		}

		[ProtoMember(19, IsRequired = false, Name = "MailVersion", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint MailVersion
		{
			get
			{
				return this._MailVersion;
			}
			set
			{
				this._MailVersion = value;
			}
		}

		[ProtoMember(20, Name = "MailIDs", DataFormat = DataFormat.TwosComplement)]
		public List<uint> MailIDs
		{
			get
			{
				return this._MailIDs;
			}
		}

		[ProtoMember(21, Name = "GuideSteps", DataFormat = DataFormat.TwosComplement)]
		public List<int> GuideSteps
		{
			get
			{
				return this._GuideSteps;
			}
		}

		[ProtoMember(24, IsRequired = false, Name = "MsgSerialID", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint MsgSerialID
		{
			get
			{
				return this._MsgSerialID;
			}
			set
			{
				this._MsgSerialID = value;
			}
		}

		[ProtoMember(27, Name = "Pays", DataFormat = DataFormat.Default)]
		public List<PayData> Pays
		{
			get
			{
				return this._Pays;
			}
		}

		[ProtoMember(28, IsRequired = false, Name = "CostumePartyTimestamp", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int CostumePartyTimestamp
		{
			get
			{
				return this._CostumePartyTimestamp;
			}
			set
			{
				this._CostumePartyTimestamp = value;
			}
		}

		[ProtoMember(29, IsRequired = false, Name = "CostumePartyCount", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int CostumePartyCount
		{
			get
			{
				return this._CostumePartyCount;
			}
			set
			{
				this._CostumePartyCount = value;
			}
		}

		[ProtoMember(30, IsRequired = false, Name = "HasReward", DataFormat = DataFormat.Default), DefaultValue(false)]
		public bool HasReward
		{
			get
			{
				return this._HasReward;
			}
			set
			{
				this._HasReward = value;
			}
		}

		[ProtoMember(31, IsRequired = false, Name = "RoomID", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint RoomID
		{
			get
			{
				return this._RoomID;
			}
			set
			{
				this._RoomID = value;
			}
		}

		[ProtoMember(32, Name = "BuyData", DataFormat = DataFormat.Default)]
		public List<BuyRecord> BuyData
		{
			get
			{
				return this._BuyData;
			}
		}

		[ProtoMember(33, IsRequired = false, Name = "BuyDataVersion", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint BuyDataVersion
		{
			get
			{
				return this._BuyDataVersion;
			}
			set
			{
				this._BuyDataVersion = value;
			}
		}

		[ProtoMember(34, IsRequired = false, Name = "WorldOpenTimeStamp", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int WorldOpenTimeStamp
		{
			get
			{
				return this._WorldOpenTimeStamp;
			}
			set
			{
				this._WorldOpenTimeStamp = value;
			}
		}

		[ProtoMember(35, IsRequired = false, Name = "SevenDayVersion", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint SevenDayVersion
		{
			get
			{
				return this._SevenDayVersion;
			}
			set
			{
				this._SevenDayVersion = value;
			}
		}

		[ProtoMember(36, Name = "SDRData", DataFormat = DataFormat.Default)]
		public List<SevenDayRewardData> SDRData
		{
			get
			{
				return this._SDRData;
			}
		}

		[ProtoMember(37, IsRequired = false, Name = "ShareVersion", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint ShareVersion
		{
			get
			{
				return this._ShareVersion;
			}
			set
			{
				this._ShareVersion = value;
			}
		}

		[ProtoMember(38, Name = "ShardData", DataFormat = DataFormat.Default)]
		public List<ShareAchievementData> ShardData
		{
			get
			{
				return this._ShardData;
			}
		}

		[ProtoMember(39, IsRequired = false, Name = "ActivityAchievementVersion", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint ActivityAchievementVersion
		{
			get
			{
				return this._ActivityAchievementVersion;
			}
			set
			{
				this._ActivityAchievementVersion = value;
			}
		}

		[ProtoMember(40, Name = "AAData", DataFormat = DataFormat.Default)]
		public List<ActivityAchievementData> AAData
		{
			get
			{
				return this._AAData;
			}
		}

		[ProtoMember(41, IsRequired = false, Name = "ActivityValueVersion", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint ActivityValueVersion
		{
			get
			{
				return this._ActivityValueVersion;
			}
			set
			{
				this._ActivityValueVersion = value;
			}
		}

		[ProtoMember(42, Name = "AVData", DataFormat = DataFormat.Default)]
		public List<ActivityValueData> AVData
		{
			get
			{
				return this._AVData;
			}
		}

		[ProtoMember(43, Name = "FDSReward", DataFormat = DataFormat.TwosComplement)]
		public List<int> FDSReward
		{
			get
			{
				return this._FDSReward;
			}
		}

		[ProtoMember(44, IsRequired = false, Name = "BuyFundNum", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int BuyFundNum
		{
			get
			{
				return this._BuyFundNum;
			}
			set
			{
				this._BuyFundNum = value;
			}
		}

		[ProtoMember(45, IsRequired = false, Name = "ActivityShopVersion", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint ActivityShopVersion
		{
			get
			{
				return this._ActivityShopVersion;
			}
			set
			{
				this._ActivityShopVersion = value;
			}
		}

		[ProtoMember(46, Name = "ASData", DataFormat = DataFormat.Default)]
		public List<ActivityShopData> ASData
		{
			get
			{
				return this._ASData;
			}
		}

		[ProtoMember(47, IsRequired = false, Name = "APData", DataFormat = DataFormat.Default), DefaultValue(null)]
		public ActivityPayData APData
		{
			get
			{
				return this._APData;
			}
			set
			{
				this._APData = value;
			}
		}

		[ProtoMember(48, Name = "APSData", DataFormat = DataFormat.Default)]
		public List<ActivityPayShopData> APSData
		{
			get
			{
				return this._APSData;
			}
		}

		[ProtoMember(49, IsRequired = false, Name = "Setting", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Setting
		{
			get
			{
				return this._Setting;
			}
			set
			{
				this._Setting = value;
			}
		}

		[ProtoMember(50, IsRequired = false, Name = "ShareParam", DataFormat = DataFormat.Default), DefaultValue("")]
		public string ShareParam
		{
			get
			{
				return this._ShareParam;
			}
			set
			{
				this._ShareParam = value;
			}
		}

		[ProtoMember(51, IsRequired = false, Name = "hasShareParam", DataFormat = DataFormat.Default), DefaultValue(false)]
		public bool hasShareParam
		{
			get
			{
				return this._hasShareParam;
			}
			set
			{
				this._hasShareParam = value;
			}
		}

		[ProtoMember(52, Name = "FriendList", DataFormat = DataFormat.Default)]
		public List<FriendData> FriendList
		{
			get
			{
				return this._FriendList;
			}
		}

		[ProtoMember(53, IsRequired = false, Name = "GuildWarData", DataFormat = DataFormat.Default), DefaultValue(null)]
		public GuildWarPlayerData GuildWarData
		{
			get
			{
				return this._GuildWarData;
			}
			set
			{
				this._GuildWarData = value;
			}
		}

		[ProtoMember(54, IsRequired = false, Name = "REData", DataFormat = DataFormat.Default), DefaultValue(null)]
		public ActivityRollEquipData REData
		{
			get
			{
				return this._REData;
			}
			set
			{
				this._REData = value;
			}
		}

		[ProtoMember(55, IsRequired = false, Name = "SPData", DataFormat = DataFormat.Default), DefaultValue(null)]
		public ActivitySpecifyPayData SPData
		{
			get
			{
				return this._SPData;
			}
			set
			{
				this._SPData = value;
			}
		}

		[ProtoMember(56, IsRequired = false, Name = "LopetVersion", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint LopetVersion
		{
			get
			{
				return this._LopetVersion;
			}
			set
			{
				this._LopetVersion = value;
			}
		}

		[ProtoMember(57, Name = "Lopets", DataFormat = DataFormat.Default)]
		public List<LopetData> Lopets
		{
			get
			{
				return this._Lopets;
			}
		}

		[ProtoMember(58, IsRequired = false, Name = "AssistLopetID", DataFormat = DataFormat.TwosComplement), DefaultValue(0f)]
		public ulong AssistLopetID
		{
			get
			{
				return this._AssistLopetID;
			}
			set
			{
				this._AssistLopetID = value;
			}
		}

		[ProtoMember(59, IsRequired = false, Name = "NationalDayData", DataFormat = DataFormat.Default), DefaultValue(null)]
		public ActivityNationalDayData NationalDayData
		{
			get
			{
				return this._NationalDayData;
			}
			set
			{
				this._NationalDayData = value;
			}
		}

		[ProtoMember(60, Name = "FashionTimes", DataFormat = DataFormat.TwosComplement)]
		public List<int> FashionTimes
		{
			get
			{
				return this._FashionTimes;
			}
		}

		[ProtoMember(61, IsRequired = false, Name = "GroupBuyingData", DataFormat = DataFormat.Default), DefaultValue(null)]
		public ActivityGroupBuyingData GroupBuyingData
		{
			get
			{
				return this._GroupBuyingData;
			}
			set
			{
				this._GroupBuyingData = value;
			}
		}

		[ProtoMember(62, IsRequired = false, Name = "HalloweenData", DataFormat = DataFormat.Default), DefaultValue(null)]
		public ActivityHalloweenData HalloweenData
		{
			get
			{
				return this._HalloweenData;
			}
			set
			{
				this._HalloweenData = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
