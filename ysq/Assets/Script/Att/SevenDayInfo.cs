using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Att
{
	[ProtoContract(Name = "SevenDayInfo")]
	[Serializable]
	public class SevenDayInfo : IExtensible
	{
		private int _ID;

		private string _Name = string.Empty;

		private int _DayIndex;

		private int _PageIndex;

		private bool _ShowProgress;

		private int _ConditionType;

		private int _Value;

		private readonly List<int> _RewardType = new List<int>();

		private readonly List<int> _RewardValue1 = new List<int>();

		private readonly List<int> _RewardValue2 = new List<int>();

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

		[ProtoMember(3, IsRequired = false, Name = "DayIndex", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int DayIndex
		{
			get
			{
				return this._DayIndex;
			}
			set
			{
				this._DayIndex = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "PageIndex", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int PageIndex
		{
			get
			{
				return this._PageIndex;
			}
			set
			{
				this._PageIndex = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "ShowProgress", DataFormat = DataFormat.Default), DefaultValue(false)]
		public bool ShowProgress
		{
			get
			{
				return this._ShowProgress;
			}
			set
			{
				this._ShowProgress = value;
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

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
