using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "GuildWarClientCity")]
	[Serializable]
	public class GuildWarClientCity : IExtensible
	{
		private GuildWarCity _City;

		private string _GuildName = string.Empty;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "City", DataFormat = DataFormat.Default), DefaultValue(null)]
		public GuildWarCity City
		{
			get
			{
				return this._City;
			}
			set
			{
				this._City = value;
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

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
