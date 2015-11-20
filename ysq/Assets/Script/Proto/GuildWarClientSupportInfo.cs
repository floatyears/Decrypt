using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "GuildWarClientSupportInfo")]
	[Serializable]
	public class GuildWarClientSupportInfo : IExtensible
	{
		private EGuildWarId _WarID;

		private GuildWarClientSupportTeam _Red;

		private GuildWarClientSupportTeam _Blue;

		private EGuildWarTeamId _SupportTeamID = EGuildWarTeamId.EGWTI_None;

		private int _SupportDiamond;

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
		public GuildWarClientSupportTeam Red
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
		public GuildWarClientSupportTeam Blue
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

		[ProtoMember(4, IsRequired = false, Name = "SupportTeamID", DataFormat = DataFormat.TwosComplement), DefaultValue(EGuildWarTeamId.EGWTI_None)]
		public EGuildWarTeamId SupportTeamID
		{
			get
			{
				return this._SupportTeamID;
			}
			set
			{
				this._SupportTeamID = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "SupportDiamond", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int SupportDiamond
		{
			get
			{
				return this._SupportDiamond;
			}
			set
			{
				this._SupportDiamond = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
