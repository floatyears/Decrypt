using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Att
{
	[ProtoContract(Name = "GuildInfo")]
	[Serializable]
	public class GuildInfo : IExtensible
	{
		private int _ID;

		private int _MaxMembers;

		private int _Exp;

		private string _Academy = string.Empty;

		private int _SceneID;

		private int _BossID;

		private int _RewardExp;

		private int _BossLootID;

		private int _BossHpNum;

		private int _BossReputation;

		private int _TakeReputation;

		private int _KillerReputation;

		private string _CastleName = string.Empty;

		private readonly List<int> _RewardType = new List<int>();

		private readonly List<int> _RewardValue1 = new List<int>();

		private readonly List<int> _RewardValue2 = new List<int>();

		private int _Score;

		private readonly List<int> _Score1RewardType = new List<int>();

		private readonly List<int> _Score1RewardValue1 = new List<int>();

		private readonly List<int> _Score1RewardValue2 = new List<int>();

		private readonly List<int> _Score2RewardType = new List<int>();

		private readonly List<int> _Score2RewardValue1 = new List<int>();

		private readonly List<int> _Score2RewardValue2 = new List<int>();

		private readonly List<int> _Score3RewardType = new List<int>();

		private readonly List<int> _Score3RewardValue1 = new List<int>();

		private readonly List<int> _Score3RewardValue2 = new List<int>();

		private readonly List<int> _Score4RewardType = new List<int>();

		private readonly List<int> _Score4RewardValue1 = new List<int>();

		private readonly List<int> _Score4RewardValue2 = new List<int>();

		private string _StrongholdName = string.Empty;

		private int _StrongholdBuffID;

		private int _StrongholdScore;

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

		[ProtoMember(2, IsRequired = false, Name = "MaxMembers", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int MaxMembers
		{
			get
			{
				return this._MaxMembers;
			}
			set
			{
				this._MaxMembers = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "Exp", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Exp
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

		[ProtoMember(4, IsRequired = false, Name = "Academy", DataFormat = DataFormat.Default), DefaultValue("")]
		public string Academy
		{
			get
			{
				return this._Academy;
			}
			set
			{
				this._Academy = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "SceneID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int SceneID
		{
			get
			{
				return this._SceneID;
			}
			set
			{
				this._SceneID = value;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "BossID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int BossID
		{
			get
			{
				return this._BossID;
			}
			set
			{
				this._BossID = value;
			}
		}

		[ProtoMember(7, IsRequired = false, Name = "RewardExp", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(8, IsRequired = false, Name = "BossLootID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int BossLootID
		{
			get
			{
				return this._BossLootID;
			}
			set
			{
				this._BossLootID = value;
			}
		}

		[ProtoMember(10, IsRequired = false, Name = "BossHpNum", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int BossHpNum
		{
			get
			{
				return this._BossHpNum;
			}
			set
			{
				this._BossHpNum = value;
			}
		}

		[ProtoMember(11, IsRequired = false, Name = "BossReputation", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int BossReputation
		{
			get
			{
				return this._BossReputation;
			}
			set
			{
				this._BossReputation = value;
			}
		}

		[ProtoMember(12, IsRequired = false, Name = "TakeReputation", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int TakeReputation
		{
			get
			{
				return this._TakeReputation;
			}
			set
			{
				this._TakeReputation = value;
			}
		}

		[ProtoMember(13, IsRequired = false, Name = "KillerReputation", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int KillerReputation
		{
			get
			{
				return this._KillerReputation;
			}
			set
			{
				this._KillerReputation = value;
			}
		}

		[ProtoMember(16, IsRequired = false, Name = "CastleName", DataFormat = DataFormat.Default), DefaultValue("")]
		public string CastleName
		{
			get
			{
				return this._CastleName;
			}
			set
			{
				this._CastleName = value;
			}
		}

		[ProtoMember(23, Name = "RewardType", DataFormat = DataFormat.TwosComplement)]
		public List<int> RewardType
		{
			get
			{
				return this._RewardType;
			}
		}

		[ProtoMember(24, Name = "RewardValue1", DataFormat = DataFormat.TwosComplement)]
		public List<int> RewardValue1
		{
			get
			{
				return this._RewardValue1;
			}
		}

		[ProtoMember(25, Name = "RewardValue2", DataFormat = DataFormat.TwosComplement)]
		public List<int> RewardValue2
		{
			get
			{
				return this._RewardValue2;
			}
		}

		[ProtoMember(26, IsRequired = false, Name = "Score", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Score
		{
			get
			{
				return this._Score;
			}
			set
			{
				this._Score = value;
			}
		}

		[ProtoMember(27, Name = "Score1RewardType", DataFormat = DataFormat.TwosComplement)]
		public List<int> Score1RewardType
		{
			get
			{
				return this._Score1RewardType;
			}
		}

		[ProtoMember(28, Name = "Score1RewardValue1", DataFormat = DataFormat.TwosComplement)]
		public List<int> Score1RewardValue1
		{
			get
			{
				return this._Score1RewardValue1;
			}
		}

		[ProtoMember(29, Name = "Score1RewardValue2", DataFormat = DataFormat.TwosComplement)]
		public List<int> Score1RewardValue2
		{
			get
			{
				return this._Score1RewardValue2;
			}
		}

		[ProtoMember(30, Name = "Score2RewardType", DataFormat = DataFormat.TwosComplement)]
		public List<int> Score2RewardType
		{
			get
			{
				return this._Score2RewardType;
			}
		}

		[ProtoMember(31, Name = "Score2RewardValue1", DataFormat = DataFormat.TwosComplement)]
		public List<int> Score2RewardValue1
		{
			get
			{
				return this._Score2RewardValue1;
			}
		}

		[ProtoMember(32, Name = "Score2RewardValue2", DataFormat = DataFormat.TwosComplement)]
		public List<int> Score2RewardValue2
		{
			get
			{
				return this._Score2RewardValue2;
			}
		}

		[ProtoMember(33, Name = "Score3RewardType", DataFormat = DataFormat.TwosComplement)]
		public List<int> Score3RewardType
		{
			get
			{
				return this._Score3RewardType;
			}
		}

		[ProtoMember(34, Name = "Score3RewardValue1", DataFormat = DataFormat.TwosComplement)]
		public List<int> Score3RewardValue1
		{
			get
			{
				return this._Score3RewardValue1;
			}
		}

		[ProtoMember(35, Name = "Score3RewardValue2", DataFormat = DataFormat.TwosComplement)]
		public List<int> Score3RewardValue2
		{
			get
			{
				return this._Score3RewardValue2;
			}
		}

		[ProtoMember(36, Name = "Score4RewardType", DataFormat = DataFormat.TwosComplement)]
		public List<int> Score4RewardType
		{
			get
			{
				return this._Score4RewardType;
			}
		}

		[ProtoMember(37, Name = "Score4RewardValue1", DataFormat = DataFormat.TwosComplement)]
		public List<int> Score4RewardValue1
		{
			get
			{
				return this._Score4RewardValue1;
			}
		}

		[ProtoMember(38, Name = "Score4RewardValue2", DataFormat = DataFormat.TwosComplement)]
		public List<int> Score4RewardValue2
		{
			get
			{
				return this._Score4RewardValue2;
			}
		}

		[ProtoMember(39, IsRequired = false, Name = "StrongholdName", DataFormat = DataFormat.Default), DefaultValue("")]
		public string StrongholdName
		{
			get
			{
				return this._StrongholdName;
			}
			set
			{
				this._StrongholdName = value;
			}
		}

		[ProtoMember(40, IsRequired = false, Name = "StrongholdBuffID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int StrongholdBuffID
		{
			get
			{
				return this._StrongholdBuffID;
			}
			set
			{
				this._StrongholdBuffID = value;
			}
		}

		[ProtoMember(41, IsRequired = false, Name = "StrongholdScore", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int StrongholdScore
		{
			get
			{
				return this._StrongholdScore;
			}
			set
			{
				this._StrongholdScore = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
