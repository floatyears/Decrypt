using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Att
{
	[ProtoContract(Name = "SceneInfo")]
	[Serializable]
	public class SceneInfo : IExtensible
	{
		private int _ID;

		private string _Name = string.Empty;

		private string _ResLoc = string.Empty;

		private string _Desc = string.Empty;

		private int _Type;

		private int _PreID;

		private int _NextID;

		private int _MapID;

		private int _MinLevel;

		private int _AtmosphereType;

		private string _Sound = string.Empty;

		private string _SoundETC = string.Empty;

		private int _Difficulty;

		private float _LootHPMPCoef;

		private int _MinLootHPMPCount;

		private int _MaxLootHPMPCount;

		private int _MinLootMoney;

		private int _MaxLootMoney;

		private bool _DayReset;

		private float _LimitTime;

		private float _BaseScore;

		private int _DayTimes;

		private bool _RecountTimes;

		private int _CostValue;

		private int _RewardExp;

		private int _RewardMoney;

		private int _SubType;

		private int _LootHPValue;

		private int _LootMPValue;

		private int _LootType;

		private int _UIIndex;

		private bool _Resurrect;

		private string _Icon = string.Empty;

		private readonly List<int> _LootItem = new List<int>();

		private readonly List<int> _LootMinCount = new List<int>();

		private readonly List<int> _LootMaxCount = new List<int>();

		private readonly List<int> _LootRate = new List<int>();

		private readonly List<int> _Enemy = new List<int>();

		private int _UIActorRotation;

		private bool _UIBoss;

		private int _PreID2;

		private int _AssistantPetID;

		private int _AssistantAttID;

		private int _RespawnInfoID;

		private string _WeatherEffect = string.Empty;

		private int _WEMode;

		private bool _Wall;

		private int _StartID;

		private int _CombatValue;

		private float _Zoom;

		private int _RewardDiamond;

		private int _RewardEmblem;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = true, Name = "ID", DataFormat = DataFormat.TwosComplement)]
		public int ID
		{
			get
			{
				return this._ID;
			}
			set
			{
				this._ID = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "Name", DataFormat = DataFormat.Default), DefaultValue("")]
		public string Name
		{
			get
			{
				return this._Name;
			}
			set
			{
				this._Name = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "ResLoc", DataFormat = DataFormat.Default), DefaultValue("")]
		public string ResLoc
		{
			get
			{
				return this._ResLoc;
			}
			set
			{
				this._ResLoc = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "Desc", DataFormat = DataFormat.Default), DefaultValue("")]
		public string Desc
		{
			get
			{
				return this._Desc;
			}
			set
			{
				this._Desc = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "Type", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Type
		{
			get
			{
				return this._Type;
			}
			set
			{
				this._Type = value;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "PreID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int PreID
		{
			get
			{
				return this._PreID;
			}
			set
			{
				this._PreID = value;
			}
		}

		[ProtoMember(7, IsRequired = false, Name = "NextID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int NextID
		{
			get
			{
				return this._NextID;
			}
			set
			{
				this._NextID = value;
			}
		}

		[ProtoMember(8, IsRequired = false, Name = "MapID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int MapID
		{
			get
			{
				return this._MapID;
			}
			set
			{
				this._MapID = value;
			}
		}

		[ProtoMember(9, IsRequired = false, Name = "MinLevel", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int MinLevel
		{
			get
			{
				return this._MinLevel;
			}
			set
			{
				this._MinLevel = value;
			}
		}

		[ProtoMember(10, IsRequired = false, Name = "AtmosphereType", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int AtmosphereType
		{
			get
			{
				return this._AtmosphereType;
			}
			set
			{
				this._AtmosphereType = value;
			}
		}

		[ProtoMember(11, IsRequired = false, Name = "Sound", DataFormat = DataFormat.Default), DefaultValue("")]
		public string Sound
		{
			get
			{
				return this._Sound;
			}
			set
			{
				this._Sound = value;
			}
		}

		[ProtoMember(12, IsRequired = false, Name = "SoundETC", DataFormat = DataFormat.Default), DefaultValue("")]
		public string SoundETC
		{
			get
			{
				return this._SoundETC;
			}
			set
			{
				this._SoundETC = value;
			}
		}

		[ProtoMember(13, IsRequired = false, Name = "Difficulty", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Difficulty
		{
			get
			{
				return this._Difficulty;
			}
			set
			{
				this._Difficulty = value;
			}
		}

		[ProtoMember(14, IsRequired = false, Name = "LootHPMPCoef", DataFormat = DataFormat.FixedSize), DefaultValue(0f)]
		public float LootHPMPCoef
		{
			get
			{
				return this._LootHPMPCoef;
			}
			set
			{
				this._LootHPMPCoef = value;
			}
		}

		[ProtoMember(15, IsRequired = false, Name = "MinLootHPMPCount", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int MinLootHPMPCount
		{
			get
			{
				return this._MinLootHPMPCount;
			}
			set
			{
				this._MinLootHPMPCount = value;
			}
		}

		[ProtoMember(16, IsRequired = false, Name = "MaxLootHPMPCount", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int MaxLootHPMPCount
		{
			get
			{
				return this._MaxLootHPMPCount;
			}
			set
			{
				this._MaxLootHPMPCount = value;
			}
		}

		[ProtoMember(17, IsRequired = false, Name = "MinLootMoney", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int MinLootMoney
		{
			get
			{
				return this._MinLootMoney;
			}
			set
			{
				this._MinLootMoney = value;
			}
		}

		[ProtoMember(18, IsRequired = false, Name = "MaxLootMoney", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int MaxLootMoney
		{
			get
			{
				return this._MaxLootMoney;
			}
			set
			{
				this._MaxLootMoney = value;
			}
		}

		[ProtoMember(19, IsRequired = false, Name = "DayReset", DataFormat = DataFormat.Default), DefaultValue(false)]
		public bool DayReset
		{
			get
			{
				return this._DayReset;
			}
			set
			{
				this._DayReset = value;
			}
		}

		[ProtoMember(20, IsRequired = false, Name = "LimitTime", DataFormat = DataFormat.FixedSize), DefaultValue(0f)]
		public float LimitTime
		{
			get
			{
				return this._LimitTime;
			}
			set
			{
				this._LimitTime = value;
			}
		}

		[ProtoMember(21, IsRequired = false, Name = "BaseScore", DataFormat = DataFormat.FixedSize), DefaultValue(0f)]
		public float BaseScore
		{
			get
			{
				return this._BaseScore;
			}
			set
			{
				this._BaseScore = value;
			}
		}

		[ProtoMember(22, IsRequired = false, Name = "DayTimes", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int DayTimes
		{
			get
			{
				return this._DayTimes;
			}
			set
			{
				this._DayTimes = value;
			}
		}

		[ProtoMember(23, IsRequired = false, Name = "RecountTimes", DataFormat = DataFormat.Default), DefaultValue(false)]
		public bool RecountTimes
		{
			get
			{
				return this._RecountTimes;
			}
			set
			{
				this._RecountTimes = value;
			}
		}

		[ProtoMember(24, IsRequired = false, Name = "CostValue", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int CostValue
		{
			get
			{
				return this._CostValue;
			}
			set
			{
				this._CostValue = value;
			}
		}

		[ProtoMember(25, IsRequired = false, Name = "RewardExp", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int RewardExp
		{
			get
			{
				return this._RewardExp;
			}
			set
			{
				this._RewardExp = value;
			}
		}

		[ProtoMember(26, IsRequired = false, Name = "RewardMoney", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int RewardMoney
		{
			get
			{
				return this._RewardMoney;
			}
			set
			{
				this._RewardMoney = value;
			}
		}

		[ProtoMember(27, IsRequired = false, Name = "SubType", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int SubType
		{
			get
			{
				return this._SubType;
			}
			set
			{
				this._SubType = value;
			}
		}

		[ProtoMember(28, IsRequired = false, Name = "LootHPValue", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int LootHPValue
		{
			get
			{
				return this._LootHPValue;
			}
			set
			{
				this._LootHPValue = value;
			}
		}

		[ProtoMember(29, IsRequired = false, Name = "LootMPValue", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int LootMPValue
		{
			get
			{
				return this._LootMPValue;
			}
			set
			{
				this._LootMPValue = value;
			}
		}

		[ProtoMember(30, IsRequired = false, Name = "LootType", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int LootType
		{
			get
			{
				return this._LootType;
			}
			set
			{
				this._LootType = value;
			}
		}

		[ProtoMember(31, IsRequired = false, Name = "UIIndex", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int UIIndex
		{
			get
			{
				return this._UIIndex;
			}
			set
			{
				this._UIIndex = value;
			}
		}

		[ProtoMember(32, IsRequired = false, Name = "Resurrect", DataFormat = DataFormat.Default), DefaultValue(false)]
		public bool Resurrect
		{
			get
			{
				return this._Resurrect;
			}
			set
			{
				this._Resurrect = value;
			}
		}

		[ProtoMember(33, IsRequired = false, Name = "Icon", DataFormat = DataFormat.Default), DefaultValue("")]
		public string Icon
		{
			get
			{
				return this._Icon;
			}
			set
			{
				this._Icon = value;
			}
		}

		[ProtoMember(34, Name = "LootItem", DataFormat = DataFormat.TwosComplement)]
		public List<int> LootItem
		{
			get
			{
				return this._LootItem;
			}
		}

		[ProtoMember(35, Name = "LootMinCount", DataFormat = DataFormat.TwosComplement)]
		public List<int> LootMinCount
		{
			get
			{
				return this._LootMinCount;
			}
		}

		[ProtoMember(36, Name = "LootMaxCount", DataFormat = DataFormat.TwosComplement)]
		public List<int> LootMaxCount
		{
			get
			{
				return this._LootMaxCount;
			}
		}

		[ProtoMember(37, Name = "LootRate", DataFormat = DataFormat.TwosComplement)]
		public List<int> LootRate
		{
			get
			{
				return this._LootRate;
			}
		}

		[ProtoMember(38, Name = "Enemy", DataFormat = DataFormat.TwosComplement)]
		public List<int> Enemy
		{
			get
			{
				return this._Enemy;
			}
		}

		[ProtoMember(39, IsRequired = false, Name = "UIActorRotation", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int UIActorRotation
		{
			get
			{
				return this._UIActorRotation;
			}
			set
			{
				this._UIActorRotation = value;
			}
		}

		[ProtoMember(40, IsRequired = false, Name = "UIBoss", DataFormat = DataFormat.Default), DefaultValue(false)]
		public bool UIBoss
		{
			get
			{
				return this._UIBoss;
			}
			set
			{
				this._UIBoss = value;
			}
		}

		[ProtoMember(41, IsRequired = false, Name = "PreID2", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int PreID2
		{
			get
			{
				return this._PreID2;
			}
			set
			{
				this._PreID2 = value;
			}
		}

		[ProtoMember(42, IsRequired = false, Name = "AssistantPetID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int AssistantPetID
		{
			get
			{
				return this._AssistantPetID;
			}
			set
			{
				this._AssistantPetID = value;
			}
		}

		[ProtoMember(43, IsRequired = false, Name = "AssistantAttID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int AssistantAttID
		{
			get
			{
				return this._AssistantAttID;
			}
			set
			{
				this._AssistantAttID = value;
			}
		}

		[ProtoMember(44, IsRequired = false, Name = "RespawnInfoID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int RespawnInfoID
		{
			get
			{
				return this._RespawnInfoID;
			}
			set
			{
				this._RespawnInfoID = value;
			}
		}

		[ProtoMember(45, IsRequired = false, Name = "WeatherEffect", DataFormat = DataFormat.Default), DefaultValue("")]
		public string WeatherEffect
		{
			get
			{
				return this._WeatherEffect;
			}
			set
			{
				this._WeatherEffect = value;
			}
		}

		[ProtoMember(46, IsRequired = false, Name = "WEMode", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int WEMode
		{
			get
			{
				return this._WEMode;
			}
			set
			{
				this._WEMode = value;
			}
		}

		[ProtoMember(47, IsRequired = false, Name = "Wall", DataFormat = DataFormat.Default), DefaultValue(false)]
		public bool Wall
		{
			get
			{
				return this._Wall;
			}
			set
			{
				this._Wall = value;
			}
		}

		[ProtoMember(48, IsRequired = false, Name = "StartID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int StartID
		{
			get
			{
				return this._StartID;
			}
			set
			{
				this._StartID = value;
			}
		}

		[ProtoMember(49, IsRequired = false, Name = "CombatValue", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int CombatValue
		{
			get
			{
				return this._CombatValue;
			}
			set
			{
				this._CombatValue = value;
			}
		}

		[ProtoMember(50, IsRequired = false, Name = "Zoom", DataFormat = DataFormat.FixedSize), DefaultValue(0f)]
		public float Zoom
		{
			get
			{
				return this._Zoom;
			}
			set
			{
				this._Zoom = value;
			}
		}

		[ProtoMember(51, IsRequired = false, Name = "RewardDiamond", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int RewardDiamond
		{
			get
			{
				return this._RewardDiamond;
			}
			set
			{
				this._RewardDiamond = value;
			}
		}

		[ProtoMember(52, IsRequired = false, Name = "RewardEmblem", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int RewardEmblem
		{
			get
			{
				return this._RewardEmblem;
			}
			set
			{
				this._RewardEmblem = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
