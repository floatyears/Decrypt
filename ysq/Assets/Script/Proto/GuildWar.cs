using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "GuildWar")]
	[Serializable]
	public class GuildWar : IExtensible
	{
		private EGuildWarId _WarID;

		private GuildWarTeam _Red;

		private GuildWarTeam _Blue;

		private readonly List<GuildWarStronghold> _Strongholds = new List<GuildWarStronghold>();

		private EGuildWarTeamId _Winner = EGuildWarTeamId.EGWTI_None;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "WarID", DataFormat = DataFormat.TwosComplement), DefaultValue(EGuildWarId.EGWI_None)]
		public EGuildWarId WarID
		{
			get
			{
				return this._WarID;
			}
			set
			{
				this._WarID = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "Red", DataFormat = DataFormat.Default), DefaultValue(null)]
		public GuildWarTeam Red
		{
			get
			{
				return this._Red;
			}
			set
			{
				this._Red = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "Blue", DataFormat = DataFormat.Default), DefaultValue(null)]
		public GuildWarTeam Blue
		{
			get
			{
				return this._Blue;
			}
			set
			{
				this._Blue = value;
			}
		}

		[ProtoMember(4, Name = "Strongholds", DataFormat = DataFormat.Default)]
		public List<GuildWarStronghold> Strongholds
		{
			get
			{
				return this._Strongholds;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "Winner", DataFormat = DataFormat.TwosComplement), DefaultValue(EGuildWarTeamId.EGWTI_None)]
		public EGuildWarTeamId Winner
		{
			get
			{
				return this._Winner;
			}
			set
			{
				this._Winner = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
