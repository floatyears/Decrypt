using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "GuildSignRecord")]
	[Serializable]
	public class GuildSignRecord : IExtensible
	{
		private ulong _PlayerID;

		private string _Name = string.Empty;

		private int _SignType;

		private int _Timestamp;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "PlayerID", DataFormat = DataFormat.TwosComplement), DefaultValue(0f)]
		public ulong PlayerID
		{
			get
			{
				return this._PlayerID;
			}
			set
			{
				this._PlayerID = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "Name", DataFormat = DataFormat.Default), DefaultValue("")]
		public string Name
		{
			get
			{
				return this._Name;
			}
			set
			{
				this._Name = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "SignType", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int SignType
		{
			get
			{
				return this._SignType;
			}
			set
			{
				this._SignType = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "Timestamp", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Timestamp
		{
			get
			{
				return this._Timestamp;
			}
			set
			{
				this._Timestamp = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
