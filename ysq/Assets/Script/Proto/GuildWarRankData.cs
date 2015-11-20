using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "GuildWarRankData")]
	[Serializable]
	public class GuildWarRankData : IExtensible
	{
		private ulong _GuildID;

		private string _GuildName = string.Empty;

		private int _ore;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "GuildID", DataFormat = DataFormat.TwosComplement), DefaultValue(0f)]
		public ulong GuildID
		{
			get
			{
				return this._GuildID;
			}
			set
			{
				this._GuildID = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "GuildName", DataFormat = DataFormat.Default), DefaultValue("")]
		public string GuildName
		{
			get
			{
				return this._GuildName;
			}
			set
			{
				this._GuildName = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "ore", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int ore
		{
			get
			{
				return this._ore;
			}
			set
			{
				this._ore = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
