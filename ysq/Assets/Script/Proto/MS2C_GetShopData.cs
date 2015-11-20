using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_GetShopData")]
	[Serializable]
	public class MS2C_GetShopData : IExtensible
	{
		private int _Result;

		private uint _ShopVersion;

		private bool _DiamondRefresh;

		private int _ShopType;

		private readonly List<ShopItemData> _Data = new List<ShopItemData>();

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "Result", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Result
		{
			get
			{
				return this._Result;
			}
			set
			{
				this._Result = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "ShopVersion", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint ShopVersion
		{
			get
			{
				return this._ShopVersion;
			}
			set
			{
				this._ShopVersion = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "DiamondRefresh", DataFormat = DataFormat.Default), DefaultValue(false)]
		public bool DiamondRefresh
		{
			get
			{
				return this._DiamondRefresh;
			}
			set
			{
				this._DiamondRefresh = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "ShopType", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int ShopType
		{
			get
			{
				return this._ShopType;
			}
			set
			{
				this._ShopType = value;
			}
		}

		[ProtoMember(5, Name = "Data", DataFormat = DataFormat.Default)]
		public List<ShopItemData> Data
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
