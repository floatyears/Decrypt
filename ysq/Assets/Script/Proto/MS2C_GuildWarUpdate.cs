using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_GuildWarUpdate")]
	[Serializable]
	public class MS2C_GuildWarUpdate : IExtensible
	{
		private EGuildResult _Result;

		private int _Version;

		private EGuildWarState _Status = EGuildWarState.EGWS_Normal;

		private int _Timestamp;

		private GuildWarClient _WarData;

		private readonly List<GuildWarAddScore> _Scores = new List<GuildWarAddScore>();

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

		[ProtoMember(3, IsRequired = false, Name = "Status", DataFormat = DataFormat.TwosComplement), DefaultValue(EGuildWarState.EGWS_Normal)]
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

		[ProtoMember(4, IsRequired = false, Name = "Timestamp", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(5, IsRequired = false, Name = "WarData", DataFormat = DataFormat.Default), DefaultValue(null)]
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

		[ProtoMember(6, Name = "Scores", DataFormat = DataFormat.Default)]
		public List<GuildWarAddScore> Scores
		{
			get
			{
				return this._Scores;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
