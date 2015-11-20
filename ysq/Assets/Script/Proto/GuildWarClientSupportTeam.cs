using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "GuildWarClientSupportTeam")]
	[Serializable]
	public class GuildWarClientSupportTeam : IExtensible
	{
		private ulong _GuildID;

		private string _GuildName = string.Empty;

		private int _Diamond;

		private float _LossPerCent;

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

		[ProtoMember(2, IsRequired = false, Name = "GuildName", DataFormat = DataFormat.Default), DefaultValue("")]
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

		[ProtoMember(3, IsRequired = false, Name = "Diamond", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Diamond
		{
			get
			{
				return this._Diamond;
			}
			set
			{
				this._Diamond = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "LossPerCent", DataFormat = DataFormat.FixedSize), DefaultValue(0f)]
		public float LossPerCent
		{
			get
			{
				return this._LossPerCent;
			}
			set
			{
				this._LossPerCent = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
