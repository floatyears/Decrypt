using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MC2S_Chat")]
	[Serializable]
	public class MC2S_Chat : IExtensible
	{
		private string _Message = string.Empty;

		private int _Channel;

		private ulong _PlayerID;

		private uint _Type;

		private bool _Voice;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "Message", DataFormat = DataFormat.Default), DefaultValue("")]
		public string Message
		{
			get
			{
				return this._Message;
			}
			set
			{
				this._Message = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "Channel", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Channel
		{
			get
			{
				return this._Channel;
			}
			set
			{
				this._Channel = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "PlayerID", DataFormat = DataFormat.TwosComplement), DefaultValue(0f)]
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

		[ProtoMember(4, IsRequired = false, Name = "Type", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint Type
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

		[ProtoMember(5, IsRequired = false, Name = "Voice", DataFormat = DataFormat.Default), DefaultValue(false)]
		public bool Voice
		{
			get
			{
				return this._Voice;
			}
			set
			{
				this._Voice = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
