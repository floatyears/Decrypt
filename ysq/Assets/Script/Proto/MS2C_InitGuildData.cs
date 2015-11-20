using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_InitGuildData")]
	[Serializable]
	public class MS2C_InitGuildData : IExtensible
	{
		private GuildData _Data;

		private readonly List<GuildMember> _MemberData = new List<GuildMember>();

		private IExtension extensionObject;

		[ProtoMember(2, IsRequired = false, Name = "Data", DataFormat = DataFormat.Default), DefaultValue(null)]
		public GuildData Data
		{
			get
			{
				return this._Data;
			}
			set
			{
				this._Data = value;
			}
		}

		[ProtoMember(3, Name = "MemberData", DataFormat = DataFormat.Default)]
		public List<GuildMember> MemberData
		{
			get
			{
				return this._MemberData;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
