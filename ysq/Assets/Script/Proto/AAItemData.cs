using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "AAItemData")]
	[Serializable]
	public class AAItemData : IExtensible
	{
		private int _ID;

		private int _Type;

		private int _Value;

		private int _CurValue;

		private bool _Flag;

		private int _GiftID;

		private readonly List<RewardData> _Data = new List<RewardData>();

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "ID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(2, IsRequired = false, Name = "Type", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(3, IsRequired = false, Name = "Value", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(4, IsRequired = false, Name = "CurValue", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int CurValue
		{
			get
			{
				return this._CurValue;
			}
			set
			{
				this._CurValue = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "Flag", DataFormat = DataFormat.Default), DefaultValue(false)]
		public bool Flag
		{
			get
			{
				return this._Flag;
			}
			set
			{
				this._Flag = value;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "GiftID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int GiftID
		{
			get
			{
				return this._GiftID;
			}
			set
			{
				this._GiftID = value;
			}
		}

		[ProtoMember(7, Name = "Data", DataFormat = DataFormat.Default)]
		public List<RewardData> Data
		{
			get
			{
				return this._Data;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
