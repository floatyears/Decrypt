using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_GuildBossUpdate")]
	[Serializable]
	public class MS2C_GuildBossUpdate : IExtensible
	{
		private GuildBossData _Data;

		private int _GMOpenTime;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "Data", DataFormat = DataFormat.Default), DefaultValue(null)]
		public GuildBossData Data
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

		[ProtoMember(2, IsRequired = false, Name = "GMOpenTime", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int GMOpenTime
		{
			get
			{
				return this._GMOpenTime;
			}
			set
			{
				this._GMOpenTime = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
