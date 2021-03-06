using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_GuildWarEnter")]
	[Serializable]
	public class MS2C_GuildWarEnter : IExtensible
	{
		private EGuildResult _Result;

		private int _Version;

		private GuildWarClient _WarData;

		private GuildWarTeamMember _Player;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "Result", DataFormat = DataFormat.TwosComplement), DefaultValue(EGuildResult.EGR_Success)]
		public EGuildResult Result
		{
			get
			{
				return this._Result;
			}
			set
			{
				this._Result = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "Version", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Version
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

		[ProtoMember(3, IsRequired = false, Name = "WarData", DataFormat = DataFormat.Default), DefaultValue(null)]
		public GuildWarClient WarData
		{
			get
			{
				return this._WarData;
			}
			set
			{
				this._WarData = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "Player", DataFormat = DataFormat.Default), DefaultValue(null)]
		public GuildWarTeamMember Player
		{
			get
			{
				return this._Player;
			}
			set
			{
				this._Player = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
