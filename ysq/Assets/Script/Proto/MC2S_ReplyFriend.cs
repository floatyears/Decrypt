using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MC2S_ReplyFriend")]
	[Serializable]
	public class MC2S_ReplyFriend : IExtensible
	{
		private ulong _GUID;

		private bool _Agree;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "GUID", DataFormat = DataFormat.TwosComplement), DefaultValue(0f)]
		public ulong GUID
		{
			get
			{
				return this._GUID;
			}
			set
			{
				this._GUID = value;
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
