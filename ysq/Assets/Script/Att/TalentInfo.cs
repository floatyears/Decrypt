using ProtoBuf;
using System;
using System.ComponentModel;

namespace Att
{
	[ProtoContract(Name = "TalentInfo")]
	[Serializable]
	public class TalentInfo : IExtensible
	{
		private int _ID;

		private string _Name = string.Empty;

		private string _Desc = string.Empty;

		private int _TargetType;

		private int _TargetValue;

		private int _CombatValue;

		private int _EffectType;

		private int _Value1;

		private int _Value2;

		private int _Value3;

		private int _Value4;

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

		[ProtoMember(5, IsRequired = false, Name = "TargetType", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int TargetType
		{
			get
			{
				return this._TargetType;
			}
			set
			{
				this._TargetType = value;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "TargetValue", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int TargetValue
		{
			get
			{
				return this._TargetValue;
			}
			set
			{
				this._TargetValue = value;
			}
		}

		[ProtoMember(7, IsRequired = false, Name = "CombatValue", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(8, IsRequired = false, Name = "EffectType", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int EffectType
		{
			get
			{
				return this._EffectType;
			}
			set
			{
				this._EffectType = value;
			}
		}

		[ProtoMember(9, IsRequired = false, Name = "Value1", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Value1
		{
			get
			{
				return this._Value1;
			}
			set
			{
				this._Value1 = value;
			}
		}

		[ProtoMember(10, IsRequired = false, Name = "Value2", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Value2
		{
			get
			{
				return this._Value2;
			}
			set
			{
				this._Value2 = value;
			}
		}

		[ProtoMember(11, IsRequired = false, Name = "Value3", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Value3
		{
			get
			{
				return this._Value3;
			}
			set
			{
				this._Value3 = value;
			}
		}

		[ProtoMember(12, IsRequired = false, Name = "Value4", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Value4
		{
			get
			{
				return this._Value4;
			}
			set
			{
				this._Value4 = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
