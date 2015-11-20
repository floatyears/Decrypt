using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_GuildWarStrongholdUpdate")]
	[Serializable]
	public class MS2C_GuildWarStrongholdUpdate : IExtensible
	{
		private int _Version;

		private int _StrongholdID;

		private int _SlotIndex;

		private int _HealthPct;

		private GuildWarClientTeamMember _Member;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "Version", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(2, IsRequired = false, Name = "StrongholdID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int StrongholdID
		{
			get
			{
				return this._StrongholdID;
			}
			set
			{
				this._StrongholdID = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "SlotIndex", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int SlotIndex
		{
			get
			{
				return this._SlotIndex;
			}
			set
			{
				this._SlotIndex = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "HealthPct", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int HealthPct
		{
			get
			{
				return this._HealthPct;
			}
			set
			{
				this._HealthPct = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "Member", DataFormat = DataFormat.Default), DefaultValue(null)]
		public GuildWarClientTeamMember Member
		{
			get
			{
				return this._Member;
			}
			set
			{
				this._Member = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
