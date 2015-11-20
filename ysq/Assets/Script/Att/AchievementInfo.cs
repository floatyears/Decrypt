using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Att
{
	[ProtoContract(Name = "AchievementInfo")]
	[Serializable]
	public class AchievementInfo : IExtensible
	{
		private int _ID;

		private string _Name = string.Empty;

		private string _Icon = string.Empty;

		private bool _Daily;

		private int _Score;

		private int _ConditionType;

		private int _Value;

		private readonly List<int> _RewardType = new List<int>();

		private readonly List<int> _RewardValue1 = new List<int>();

		private readonly List<int> _RewardValue2 = new List<int>();

		private int _Level;

		private int _ScoreLevel;

		private int _Score2;

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

		[ProtoMember(3, IsRequired = false, Name = "Icon", DataFormat = DataFormat.Default), DefaultValue("")]
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

		[ProtoMember(4, IsRequired = false, Name = "Daily", DataFormat = DataFormat.Default), DefaultValue(false)]
		public bool Daily
		{
			get
			{
				return this._Daily;
			}
			set
			{
				this._Daily = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "Score", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(6, IsRequired = false, Name = "ConditionType", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int ConditionType
		{
			get
			{
				return this._ConditionType;
			}
			set
			{
				this._ConditionType = value;
			}
		}

		[ProtoMember(7, IsRequired = false, Name = "Value", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Value
		{
			get
			{
				return this._Value;
			}
			set
			{
				this._Value = value;
			}
		}

		[ProtoMember(8, Name = "RewardType", DataFormat = DataFormat.TwosComplement)]
		public List<int> RewardType
		{
			get
			{
				return this._RewardType;
			}
		}

		[ProtoMember(9, Name = "RewardValue1", DataFormat = DataFormat.TwosComplement)]
		public List<int> RewardValue1
		{
			get
			{
				return this._RewardValue1;
			}
		}

		[ProtoMember(10, Name = "RewardValue2", DataFormat = DataFormat.TwosComplement)]
		public List<int> RewardValue2
		{
			get
			{
				return this._RewardValue2;
			}
		}

		[ProtoMember(11, IsRequired = false, Name = "Level", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Level
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

		[ProtoMember(12, IsRequired = false, Name = "ScoreLevel", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int ScoreLevel
		{
			get
			{
				return this._ScoreLevel;
			}
			set
			{
				this._ScoreLevel = value;
			}
		}

		[ProtoMember(13, IsRequired = false, Name = "Score2", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Score2
		{
			get
			{
				return this._Score2;
			}
			set
			{
				this._Score2 = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
