using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_GuildWarFightEnd")]
	[Serializable]
	public class MS2C_GuildWarFightEnd : IExtensible
	{
		private int _Result;

		private int _Version;

		private int _ElapsedTime;

		private GuildWarTeamMember _Player;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "Result", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Result
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

		[ProtoMember(3, IsRequired = false, Name = "ElapsedTime", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int ElapsedTime
		{
			get
			{
				return this._ElapsedTime;
			}
			set
			{
				this._ElapsedTime = value;
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
