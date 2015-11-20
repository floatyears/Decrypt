using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MC2S_ProcessGuildApplication")]
	[Serializable]
	public class MC2S_ProcessGuildApplication : IExtensible
	{
		private ulong _ID;

		private bool _Agree;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "ID", DataFormat = DataFormat.TwosComplement), DefaultValue(0f)]
		public ulong ID
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

		[ProtoMember(2, IsRequired = false, Name = "Agree", DataFormat = DataFormat.Default), DefaultValue(false)]
		public bool Agree
		{
			get
			{
				return this._Agree;
			}
			set
			{
				this._Agree = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
