using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_ActivityValueUpdate")]
	[Serializable]
	public class MS2C_ActivityValueUpdate : IExtensible
	{
		private ActivityValueData _Data;

		private uint _Version;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "Data", DataFormat = DataFormat.Default), DefaultValue(null)]
		public ActivityValueData Data
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

		[ProtoMember(2, IsRequired = false, Name = "Version", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint Version
		{
			get
			{
				return this._Version;
			}
			set
			{
				this._Version = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
