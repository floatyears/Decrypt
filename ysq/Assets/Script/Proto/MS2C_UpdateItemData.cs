using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_UpdateItemData")]
	[Serializable]
	public class MS2C_UpdateItemData : IExtensible
	{
		private ItemData _Data;

		private uint _ItemVersion;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "Data", DataFormat = DataFormat.Default), DefaultValue(null)]
		public ItemData Data
		{
			get
			{
				return this._Data;
			}
			set
			{
				this._Data = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "ItemVersion", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint ItemVersion
		{
			get
			{
				return this._ItemVersion;
			}
			set
			{
				this._ItemVersion = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
