using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_GuildWarEnd")]
	[Serializable]
	public class MS2C_GuildWarEnd : IExtensible
	{
		private GuildWarClient _WarData;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "WarData", DataFormat = DataFormat.Default), DefaultValue(null)]
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

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
