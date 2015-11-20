using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_AddGuest")]
	[Serializable]
	public class MS2C_AddGuest : IExtensible
	{
		private CostumePartyGuest _Data;

		private int _slot;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "Data", DataFormat = DataFormat.Default), DefaultValue(null)]
		public CostumePartyGuest Data
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

		[ProtoMember(2, IsRequired = false, Name = "slot", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int slot
		{
			get
			{
				return this._slot;
			}
			set
			{
				this._slot = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
