using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "ActivityShopData")]
	[Serializable]
	public class ActivityShopData : IExtensible
	{
		private BaseActivityData _Base;

		private int _Type;

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

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
