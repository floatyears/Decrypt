using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MC2S_GetShopData")]
	[Serializable]
	public class MC2S_GetShopData : IExtensible
	{
		private uint _ShopVersion;

		private bool _Refresh;

		private int _ShopType;

		private bool _DiamondRefresh;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "ShopVersion", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
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

		[ProtoMember(2, IsRequired = false, Name = "Refresh", DataFormat = DataFormat.Default), DefaultValue(false)]
		public bool Refresh
		{
			get
			{
				return this._Refresh;
			}
			set
			{
				this._Refresh = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "ShopType", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(4, IsRequired = false, Name = "DiamondRefresh", DataFormat = DataFormat.Default), DefaultValue(false)]
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

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
