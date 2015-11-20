using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "GuildWarClientTeamMember")]
	[Serializable]
	public class GuildWarClientTeamMember : IExtensible
	{
		private GuildWarTeamMember _Member;

		private RemotePlayer _Data;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "Member", DataFormat = DataFormat.Default), DefaultValue(null)]
		public GuildWarTeamMember Member
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

		[ProtoMember(2, IsRequired = false, Name = "Data", DataFormat = DataFormat.Default), DefaultValue(null)]
		public RemotePlayer Data
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

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
