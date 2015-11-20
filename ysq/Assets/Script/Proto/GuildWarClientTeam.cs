using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "GuildWarClientTeam")]
	[Serializable]
	public class GuildWarClientTeam : IExtensible
	{
		private ulong _GuildID;

		private readonly List<GuildWarClientTeamMember> _Members = new List<GuildWarClientTeamMember>();

		private int _Score;

		private uint _KillNum;

		private string _GuildName = string.Empty;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "GuildID", DataFormat = DataFormat.TwosComplement), DefaultValue(0f)]
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

		[ProtoMember(2, Name = "Members", DataFormat = DataFormat.Default)]
		public List<GuildWarClientTeamMember> Members
		{
			get
			{
				return this._Members;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "Score", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Score
		{
			get
			{
				return this._Score;
			}
			set
			{
				this._Score = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "KillNum", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint KillNum
		{
			get
			{
				return this._KillNum;
			}
			set
			{
				this._KillNum = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "GuildName", DataFormat = DataFormat.Default), DefaultValue("")]
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
