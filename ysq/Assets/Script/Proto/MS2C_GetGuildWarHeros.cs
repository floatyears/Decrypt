using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_GetGuildWarHeros")]
	[Serializable]
	public class MS2C_GetGuildWarHeros : IExtensible
	{
		private readonly List<GuildWarHeroInfo> _Data = new List<GuildWarHeroInfo>();

		private int _Praise;

		private ulong _GuildID;

		private string _GuildName = string.Empty;

		private IExtension extensionObject;

		[ProtoMember(1, Name = "Data", DataFormat = DataFormat.Default)]
		public List<GuildWarHeroInfo> Data
		{
			get
			{
				return this._Data;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "Praise", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Praise
		{
			get
			{
				return this._Praise;
			}
			set
			{
				this._Praise = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "GuildID", DataFormat = DataFormat.TwosComplement), DefaultValue(0f)]
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

		[ProtoMember(4, IsRequired = false, Name = "GuildName", DataFormat = DataFormat.Default), DefaultValue("")]
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

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
