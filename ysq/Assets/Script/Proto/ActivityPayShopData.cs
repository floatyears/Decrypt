using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "ActivityPayShopData")]
	[Serializable]
	public class ActivityPayShopData : IExtensible
	{
		private BaseActivityData _Base;

		private readonly List<APItemData> _Data = new List<APItemData>();

		private int _PayDay;

		private int _PayTimeStamp;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "Base", DataFormat = DataFormat.Default), DefaultValue(null)]
		public BaseActivityData Base
		{
			get
			{
				return this._Base;
			}
			set
			{
				this._Base = value;
			}
		}

		[ProtoMember(2, Name = "Data", DataFormat = DataFormat.Default)]
		public List<APItemData> Data
		{
			get
			{
				return this._Data;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "PayDay", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int PayDay
		{
			get
			{
				return this._PayDay;
			}
			set
			{
				this._PayDay = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "PayTimeStamp", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int PayTimeStamp
		{
			get
			{
				return this._PayTimeStamp;
			}
			set
			{
				this._PayTimeStamp = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
