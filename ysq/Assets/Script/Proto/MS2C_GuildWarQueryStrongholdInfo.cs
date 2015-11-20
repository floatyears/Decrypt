using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_GuildWarQueryStrongholdInfo")]
	[Serializable]
	public class MS2C_GuildWarQueryStrongholdInfo : IExtensible
	{
		private EGuildResult _Result;

		private int _Version;

		private GuildWarStronghold _Stronghold;

		private readonly List<GuildWarClientTeamMember> _Members = new List<GuildWarClientTeamMember>();

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

		[ProtoMember(3, IsRequired = false, Name = "Stronghold", DataFormat = DataFormat.Default), DefaultValue(null)]
		public GuildWarStronghold Stronghold
		{
			get
			{
				return this._Stronghold;
			}
			set
			{
				this._Stronghold = value;
			}
		}

		[ProtoMember(4, Name = "Members", DataFormat = DataFormat.Default)]
		public List<GuildWarClientTeamMember> Members
		{
			get
			{
				return this._Members;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
