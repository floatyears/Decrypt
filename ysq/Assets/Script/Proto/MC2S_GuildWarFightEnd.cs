using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MC2S_GuildWarFightEnd")]
	[Serializable]
	public class MC2S_GuildWarFightEnd : IExtensible
	{
		private EGuildWarId _WarID;

		private EGuildWarTeamId _TeamID = EGuildWarTeamId.EGWTI_None;

		private int _StrongholdID;

		private int _SlotIndex;

		private int _ResultKey;

		private CombatLog _Log;

		private int _HealthPct;

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

		[ProtoMember(2, IsRequired = false, Name = "TeamID", DataFormat = DataFormat.TwosComplement), DefaultValue(EGuildWarTeamId.EGWTI_None)]
		public EGuildWarTeamId TeamID
		{
			get
			{
				return this._TeamID;
			}
			set
			{
				this._TeamID = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "StrongholdID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(4, IsRequired = false, Name = "SlotIndex", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(5, IsRequired = false, Name = "ResultKey", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int ResultKey
		{
			get
			{
				return this._ResultKey;
			}
			set
			{
				this._ResultKey = value;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "Log", DataFormat = DataFormat.Default), DefaultValue(null)]
		public CombatLog Log
		{
			get
			{
				return this._Log;
			}
			set
			{
				this._Log = value;
			}
		}

		[ProtoMember(7, IsRequired = false, Name = "HealthPct", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
