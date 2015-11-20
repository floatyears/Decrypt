using ProtoBuf;
using System;
using System.ComponentModel;

namespace Att
{
	[ProtoContract(Name = "ShareAchievementInfo")]
	[Serializable]
	public class ShareAchievementInfo : IExtensible
	{
		private int _ID;

		private string _Name = string.Empty;

		private string _Desc = string.Empty;

		private string _ShardMsg = string.Empty;

		private int _ConditionType;

		private int _Value;

		private int _RewardDiamond;

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

		[ProtoMember(3, IsRequired = false, Name = "Desc", DataFormat = DataFormat.Default), DefaultValue("")]
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

		[ProtoMember(4, IsRequired = false, Name = "ShardMsg", DataFormat = DataFormat.Default), DefaultValue("")]
		public string ShardMsg
		{
			get
			{
				return this._ShardMsg;
			}
			set
			{
				this._ShardMsg = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "ConditionType", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(6, IsRequired = false, Name = "Value", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(7, IsRequired = false, Name = "RewardDiamond", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
