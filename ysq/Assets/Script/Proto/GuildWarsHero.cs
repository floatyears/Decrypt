using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "GuildWarsHero")]
	[Serializable]
	public class GuildWarsHero : IExtensible
	{
		private ulong _Player_id;

		private int _Praised_count;

		private string _Message = string.Empty;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "Player_id", DataFormat = DataFormat.TwosComplement), DefaultValue(0f)]
		public ulong Player_id
		{
			get
			{
				return this._Player_id;
			}
			set
			{
				this._Player_id = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "Praised_count", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Praised_count
		{
			get
			{
				return this._Praised_count;
			}
			set
			{
				this._Praised_count = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "Message", DataFormat = DataFormat.Default), DefaultValue("")]
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

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
