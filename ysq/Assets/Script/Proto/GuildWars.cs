using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "GuildWars")]
	[Serializable]
	public class GuildWars : IExtensible
	{
		private EGuildWarState _Status = EGuildWarState.EGWS_Normal;

		private int _Timestamp;

		private readonly List<GuildWar> _Wars = new List<GuildWar>();

		private GuildWar _War;

		private readonly List<GuildWarCity> _Citys = new List<GuildWarCity>();

		private int _ResetCityTimestamp;

		private readonly List<GuildWarsHero> _Heros = new List<GuildWarsHero>();

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "Status", DataFormat = DataFormat.TwosComplement), DefaultValue(EGuildWarState.EGWS_Normal)]
		public EGuildWarState Status
		{
			get
			{
				return this._Status;
			}
			set
			{
				this._Status = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "Timestamp", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Timestamp
		{
			get
			{
				return this._Timestamp;
			}
			set
			{
				this._Timestamp = value;
			}
		}

		[ProtoMember(3, Name = "Wars", DataFormat = DataFormat.Default)]
		public List<GuildWar> Wars
		{
			get
			{
				return this._Wars;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "War", DataFormat = DataFormat.Default), DefaultValue(null)]
		public GuildWar War
		{
			get
			{
				return this._War;
			}
			set
			{
				this._War = value;
			}
		}

		[ProtoMember(5, Name = "Citys", DataFormat = DataFormat.Default)]
		public List<GuildWarCity> Citys
		{
			get
			{
				return this._Citys;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "ResetCityTimestamp", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int ResetCityTimestamp
		{
			get
			{
				return this._ResetCityTimestamp;
			}
			set
			{
				this._ResetCityTimestamp = value;
			}
		}

		[ProtoMember(7, Name = "Heros", DataFormat = DataFormat.Default)]
		public List<GuildWarsHero> Heros
		{
			get
			{
				return this._Heros;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
